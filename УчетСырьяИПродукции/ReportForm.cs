using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace SQLiteViewer
{
    public partial class ReportForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";
        private string reportType;

        public ReportForm(string reportType)
        {
            InitializeComponent();
            this.reportType = reportType;
        }

        private void buttonGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                // Получение дат
                DateTime startDate = dateTimePickerStartDate.Value;
                DateTime endDate = dateTimePickerEndDate.Value;

                // Проверка на корректность дат
                if (startDate > endDate)
                {
                    MessageBox.Show("Дата начала не может быть больше даты окончания.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Диалог выбора пути для сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = $"{reportType}_Report_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.docx",
                    Filter = "Word Documents|*.docx",
                    DefaultExt = "docx",
                    AddExtension = true
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    GenerateReport(startDate, endDate, filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации отчёта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateReport(DateTime startDate, DateTime endDate, string filePath)
        {
            List<ReportEntry> entries = GetReportEntries(reportType, startDate, endDate);

            if (entries.Count == 0)
            {
                MessageBox.Show("Нет данных для выбранного промежутка.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CreateWordReport(filePath, reportType, startDate, endDate, entries);

            MessageBox.Show($"Отчёт успешно сохранён по пути:\n{filePath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<ReportEntry> GetReportEntries(string reportType, DateTime startDate, DateTime endDate)
        {
            List<ReportEntry> entries = new List<ReportEntry>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "";
                if (reportType == "Поставки")
                {
                    query = @"
                SELECT 
                    p.Название AS Participant, -- Поставщик
                    w.Название AS Warehouse, 
                    pr.Название AS Product, 
                    ps.Колличество AS Quantity, 
                    ps.Дата AS Date 
                FROM Поставки ps
                JOIN с_Поставщики p ON ps.Поставщик_id = p.Поставщик_id
                JOIN с_Склады w ON ps.Склад_id = w.Склад_id
                JOIN с_Продукция pr ON ps.Сырье_id = pr.Продукция_id
                WHERE ps.Дата BETWEEN @startDate AND @endDate
                LIMIT 1000"; // Ограничиваем количество записей
                }
                else if (reportType == "Выбытие")
                {
                    query = @"
                SELECT 
                    p.Название AS Participant, -- Покупатель
                    w.Название AS Warehouse, 
                    pr.Название AS Product, 
                    v.Количество AS Quantity, 
                    v.Дата AS Date 
                FROM Выбытие v
                JOIN с_Покупатели p ON v.Покупатель_id = p.Покупатель_id
                JOIN с_Склады w ON v.Склад_id = w.Склад_id
                JOIN с_Продукция pr ON v.Продукция_id = pr.Продукция_id
                WHERE v.Дата BETWEEN @startDate AND @endDate
                LIMIT 1000"; // Ограничиваем количество записей
                }
                else
                {
                    throw new ArgumentException("Неверный тип отчёта.");
                }

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@startDate", startDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@endDate", endDate.ToString("yyyy-MM-dd"));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entry = new ReportEntry
                            {
                                Participant = reader["Participant"] as string,
                                Warehouse = reader["Warehouse"] as string,
                                Product = reader["Product"] as string,
                                Quantity = Convert.ToDouble(reader["Quantity"]),
                                Date = Convert.ToDateTime(reader["Date"])
                            };
                            entries.Add(entry);
                        }
                    }
                }
            }

            return entries;
        }

        private void CreateWordReport(string filePath, string reportType, DateTime startDate, DateTime endDate, List<ReportEntry> entries)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                var mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document(new Body());
                var body = mainPart.Document.Body;

                // Заголовок отчёта
                Paragraph title = new Paragraph(
                    new ParagraphProperties(
                        new Justification { Val = JustificationValues.Center }
                    ),
                    new Run(
                        new RunProperties(
                            new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                            new FontSize { Val = "24" } // Размер шрифта 12 пунктов
                        ),
                        new Text($"Отчёт по {reportType}")
                    )
                );
                body.AppendChild(title);

                // Период
                body.AppendChild(new Paragraph(
                    new Run(
                        new RunProperties(
                            new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                            new FontSize { Val = "24" }
                        ),
                        new Text($"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}")
                    )
                ));

                // Таблица
                Table table = new Table();

                // Настройка границ таблицы
                TableProperties tblProps = new TableProperties(
                    new TableBorders(
                        new TopBorder { Val = BorderValues.Single, Size = 4 },   // Толщина рамок 1
                        new BottomBorder { Val = BorderValues.Single, Size = 4 },
                        new LeftBorder { Val = BorderValues.Single, Size = 4 },
                        new RightBorder { Val = BorderValues.Single, Size = 4 },
                        new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                        new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                    )
                );
                table.AppendChild(tblProps);

                // Заголовок таблицы
                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateTableCell("№", true),
                    CreateTableCell("Склад", true),
                    CreateTableCell("Продукция", true),
                    CreateTableCell("Количество", true),
                    CreateTableCell("Дата", true)
                );
                table.Append(headerRow);

                // Данные таблицы
                int count = 1;
                double totalQuantity = 0;
                foreach (var entry in entries)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        CreateTableCell(count.ToString()),
                        CreateTableCell(entry.Warehouse),
                        CreateTableCell(entry.Product),
                        CreateTableCell(entry.Quantity.ToString()),
                        CreateTableCell(entry.Date.ToString("dd.MM.yyyy"))
                    );
                    table.Append(dataRow);

                    totalQuantity += entry.Quantity;
                    count++;
                }

                // Итоговая строка (без границ)
                TableRow totalRow = new TableRow();
                totalRow.Append(
                    CreateTableCell(""),
                    CreateTableCell(""),
                    CreateTableCell("ИТОГ", isBold: true),
                    CreateTableCell(totalQuantity.ToString(), isBold: true),
                    CreateTableCell("")
                );

                // Удаляем границы для итоговой строки
                foreach (var cell in totalRow.Elements<TableCell>())
                {
                    cell.Append(new TableCellProperties(
                        new TableCellBorders(
                            new TopBorder { Val = BorderValues.Nil },
                            new BottomBorder { Val = BorderValues.Nil },
                            new LeftBorder { Val = BorderValues.Nil },
                            new RightBorder { Val = BorderValues.Nil }
                        )
                    ));
                }

                table.Append(totalRow);

                body.Append(table);
                wordDoc.Save();
            }
        }

        private TableCell CreateTableCell(string text, bool isBold = false)
        {
            return new TableCell(
                new Paragraph(
                    new Run(
                        new RunProperties(
                            new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman" },
                            new FontSize { Val = "24" },
                            isBold ? new Bold() : null
                        ),
                        new Text(text)
                    )
                )
            );
        }
        public class ReportEntry
        {
            public string Participant { get; set; } // Общий участник: Supplier (для Поставки) или Buyer (для Выбытия)
            public string Warehouse { get; set; }
            public string Product { get; set; }
            public double Quantity { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
