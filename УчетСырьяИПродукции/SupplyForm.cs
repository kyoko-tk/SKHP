using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace SQLiteViewer
{
    public partial class SupplyForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";
        private DataTable поставщикиTable;
        private DataTable складыTable;
        private DataTable сырьеTable;

        public SupplyForm()
        {
            InitializeComponent();
            SetBusyTimeout();
            LoadComboBoxData();
        }

        // Установка busy_timeout для предотвращения блокировок
        private void SetBusyTimeout()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("PRAGMA busy_timeout = 3000;", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Загрузка данных в ComboBox
        private void LoadComboBoxData()
        {
            try
            {
                // Загрузка данных в таблицы
                LoadDataIntoTable("SELECT Поставщик_id, Название FROM с_Поставщики", ref поставщикиTable, comboBoxПоставщик, "Название", "Поставщик_id");
                LoadDataIntoTable("SELECT Склад_id, Название FROM с_Склады", ref складыTable, comboBoxСклад, "Название", "Склад_id");
                LoadDataIntoTable("SELECT Сырье_id, Название FROM с_Сырье", ref сырьеTable, comboBoxСырье, "Название", "Сырье_id");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataIntoTable(string query, ref DataTable table, ComboBox comboBox, string displayMember, string valueMember)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                using (var adapter = new SQLiteDataAdapter(command))
                {
                    table = new DataTable();
                    adapter.Fill(table);
                    comboBox.DataSource = table;
                    comboBox.DisplayMember = displayMember;
                    comboBox.ValueMember = valueMember;
                }
            }
        }

        private void SaveRecord()
        {
            try
            {
                // Проверка введенных данных
                if (comboBoxПоставщик.SelectedValue == null || comboBoxСклад.SelectedValue == null || comboBoxСырье.SelectedValue == null)
                {
                    MessageBox.Show("Все поля (Поставщик, склад, сырье) должны быть заполнены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxОбъем.Text, out double объем) || объем <= 0)
                {
                    MessageBox.Show("Количество должно быть положительным числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string дата = dateTimePickerДата.Value.ToString("yyyy-MM-dd");
                int поставщикId = Convert.ToInt32(comboBoxПоставщик.SelectedValue);
                int складId = Convert.ToInt32(comboBoxСклад.SelectedValue);
                int сырьеId = Convert.ToInt32(comboBoxСырье.SelectedValue);

                // Пути к исходной и временной базам данных
                string tempDatabase = "temp.db";

                // Создаем копию базы данных
                File.Copy("DB.db", tempDatabase, true);

                // Работаем с временной базой данных
                using (var tempConnection = new SQLiteConnection($"Data Source={tempDatabase};Version=3;"))
                {
                    tempConnection.Open();

                    using (var transaction = tempConnection.BeginTransaction())
                    {
                        string query = "INSERT INTO Поставки (Поставщик_id, Склад_id, Сырье_id, Колличество, Дата) " +
                                       "VALUES (@Поставщик_id, @Склад_id, @Сырье_id, @Колличество, @Дата)";

                        using (var command = new SQLiteCommand(query, tempConnection, transaction))
                        {
                            command.Parameters.AddWithValue("@Поставщик_id", поставщикId);
                            command.Parameters.AddWithValue("@Склад_id", складId);
                            command.Parameters.AddWithValue("@Сырье_id", сырьеId);
                            command.Parameters.AddWithValue("@Колличество", объем);
                            command.Parameters.AddWithValue("@Дата", дата);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }

                // Заменяем оригинальную базу временной
                File.Copy(tempDatabase, "DB.db", true);
                File.Delete(tempDatabase); // Удаляем временную базу

                MessageBox.Show("Поставка успешно добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Закрываем форму после успешного добавления
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Общая ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик кнопки "Сохранить"
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }
    }
}
