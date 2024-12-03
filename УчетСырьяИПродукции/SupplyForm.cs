using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SQLiteViewer
{
    public partial class SupplyForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";
        private DataTable договорыTable;
        private DataTable складыTable;
        private DataTable сырьеTable;

        public SupplyForm()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        // Загрузка данных в ComboBox
        private void LoadComboBoxData()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Загрузка договоров
                    var договорыQuery = "SELECT Договор_id, Номер FROM Договоры";
                    using (var command = new SQLiteCommand(договорыQuery, connection))
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        договорыTable = new DataTable();
                        adapter.Fill(договорыTable);
                        comboBoxДоговор.DataSource = договорыTable;
                        comboBoxДоговор.DisplayMember = "Номер";
                        comboBoxДоговор.ValueMember = "Договор_id";
                    }

                    // Загрузка складов
                    var складыQuery = "SELECT Склад_id, Название FROM с_Склады";
                    using (var command = new SQLiteCommand(складыQuery, connection))
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        складыTable = new DataTable();
                        adapter.Fill(складыTable);
                        comboBoxСклад.DataSource = складыTable;
                        comboBoxСклад.DisplayMember = "Название";
                        comboBoxСклад.ValueMember = "Склад_id";
                    }

                    // Загрузка сырья
                    var сырьеQuery = "SELECT Сырье_id, Название FROM с_Сырье";
                    using (var command = new SQLiteCommand(сырьеQuery, connection))
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        сырьеTable = new DataTable();
                        adapter.Fill(сырьеTable);
                        comboBoxСырье.DataSource = сырьеTable;
                        comboBoxСырье.DisplayMember = "Название";
                        comboBoxСырье.ValueMember = "Сырье_id";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Сохранение данных в базу
        private void SaveRecord()
        {
            try
            {
                // Проверка введенных данных
                if (comboBoxДоговор.SelectedValue == null || comboBoxСклад.SelectedValue == null || comboBoxСырье.SelectedValue == null)
                {
                    MessageBox.Show("Все поля (договор, склад, сырье) должны быть заполнены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxОбъем.Text, out double объем) || объем <= 0)
                {
                    MessageBox.Show("Объем должен быть положительным числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string датаПоставки = dateTimePickerДатаПоставки.Value.ToString("yyyy-MM-dd");
                int договорId = Convert.ToInt32(comboBoxДоговор.SelectedValue);
                int складId = Convert.ToInt32(comboBoxСклад.SelectedValue);
                int сырьеId = Convert.ToInt32(comboBoxСырье.SelectedValue);

                // Сохранение в базу данных
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Поставки (Договор_id, Склад_id, Сырье_id, Объем, ДатаПоставки) " +
                                   "VALUES (@Договор_id, @Склад_id, @Сырье_id, @Объем, @ДатаПоставки)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Договор_id", договорId);
                        command.Parameters.AddWithValue("@Склад_id", складId);
                        command.Parameters.AddWithValue("@Сырье_id", сырьеId);
                        command.Parameters.AddWithValue("@Объем", объем);
                        command.Parameters.AddWithValue("@ДатаПоставки", датаПоставки);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Поставка успешно добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Закрываем форму после успешного добавления
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик кнопки "Сохранить"
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }
    }
}
