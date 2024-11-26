using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SQLiteViewer
{
    public partial class AddRecordForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";
        private DataTable suppliersTable;
        private DataTable customersTable;

        public AddRecordForm()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        // Загрузка данных в ComboBox для выбора поставщиков и покупателей
        private void LoadComboBoxData()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Загрузка поставщиков
                    var suppliersQuery = "SELECT Поставщик_id, Название FROM с_Поставщики";
                    using (var command = new SQLiteCommand(suppliersQuery, connection))
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        suppliersTable = new DataTable();
                        adapter.Fill(suppliersTable);
                        comboBoxПоставщики.DataSource = suppliersTable;
                        comboBoxПоставщики.DisplayMember = "Название";
                        comboBoxПоставщики.ValueMember = "Поставщик_id";
                    }

                    // Загрузка покупателей
                    var customersQuery = "SELECT Покупатель_id, Название FROM с_Покупатели";
                    using (var command = new SQLiteCommand(customersQuery, connection))
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        customersTable = new DataTable();
                        adapter.Fill(customersTable);
                        comboBoxПокупатели.DataSource = customersTable;
                        comboBoxПокупатели.DisplayMember = "Название";
                        comboBoxПокупатели.ValueMember = "Покупатель_id";
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
                string номер = textBoxНомер.Text;
                if (string.IsNullOrWhiteSpace(номер))
                {
                    MessageBox.Show("Номер договора не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Получаем выбранные значения из ComboBox и проверяем их на null.
                object selectedПоставщикId = comboBoxПоставщики.SelectedValue;
                object selectedПокупательId = comboBoxПокупатели.SelectedValue;

                if (selectedПоставщикId == null || selectedПокупательId == null)
                {
                    MessageBox.Show("Поставщик и Покупатель должны быть выбраны.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Преобразуем значения в целые числа
                int поставщикId = Convert.ToInt32(selectedПоставщикId);
                int покупательId = Convert.ToInt32(selectedПокупательId);

                // Преобразуем дату в строку формата "yyyy-MM-dd", чтобы сохранить только дату без времени
                string датаЗаключения = dateTimePickerДатаЗаключения.Value.ToString("yyyy-MM-dd");

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Договоры (Номер, Поставщик_id, Покупатель_id, ДатаЗаключения) " +
                                   "VALUES (@Номер, @Поставщик_id, @Покупатель_id, @ДатаЗаключения)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        // Убедитесь, что параметры передаются корректно и в правильном формате
                        command.Parameters.AddWithValue("@Номер", номер);
                        command.Parameters.AddWithValue("@Поставщик_id", поставщикId);
                        command.Parameters.AddWithValue("@Покупатель_id", покупательId);
                        command.Parameters.AddWithValue("@ДатаЗаключения", датаЗаключения); // передаем строку с датой

                        // Выполнение запроса
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запись успешно добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Закрыть форму после успешного добавления
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Обработчик нажатия кнопки "Сохранить"
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Проверка, чтобы все обязательные поля были заполнены
            if (string.IsNullOrEmpty(textBoxНомер.Text))
            {
                MessageBox.Show("Номер договора не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Сохранить запись в базу данных
            SaveRecord();
        }
    }
}
