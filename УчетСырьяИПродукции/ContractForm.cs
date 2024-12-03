using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SQLiteViewer
{
    public partial class ContractForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";
        private DataTable suppliersTable;
        private DataTable customersTable;

        public ContractForm()
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

                        // Добавляем пустую строку на первую позицию
                        DataRow newRow = suppliersTable.NewRow();
                        newRow["Поставщик_id"] = DBNull.Value;
                        newRow["Название"] = "";  // Пустое название для первого элемента
                        suppliersTable.Rows.InsertAt(newRow, 0);

                        comboBoxПоставщики.DataSource = suppliersTable;
                        comboBoxПоставщики.DisplayMember = "Название";
                        comboBoxПоставщики.ValueMember = "Поставщик_id";
                        comboBoxПоставщики.SelectedIndex = 0; // Устанавливаем пустое значение по умолчанию
                    }

                    // Загрузка покупателей
                    var customersQuery = "SELECT Покупатель_id, Название FROM с_Покупатели";
                    using (var command = new SQLiteCommand(customersQuery, connection))
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        customersTable = new DataTable();
                        adapter.Fill(customersTable);

                        // Добавляем пустую строку на первую позицию
                        DataRow newRow = customersTable.NewRow();
                        newRow["Покупатель_id"] = DBNull.Value;
                        newRow["Название"] = "";  // Пустое название для первого элемента
                        customersTable.Rows.InsertAt(newRow, 0);

                        comboBoxПокупатели.DataSource = customersTable;
                        comboBoxПокупатели.DisplayMember = "Название";
                        comboBoxПокупатели.ValueMember = "Покупатель_id";
                        comboBoxПокупатели.SelectedIndex = 0; // Устанавливаем пустое значение по умолчанию
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

                // Получаем выбранные значения из ComboBox
                object selectedПоставщикId = comboBoxПоставщики.SelectedValue;
                object selectedПокупательId = comboBoxПокупатели.SelectedValue;

                // Если оба поля пусты, не разрешаем сохранение
                if (selectedПоставщикId == DBNull.Value && selectedПокупательId == DBNull.Value)
                {
                    MessageBox.Show("Необходимо выбрать либо поставщика, либо покупателя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверяем, что выбран только один из вариантов (либо поставщик, либо покупатель)
                if (selectedПоставщикId != DBNull.Value && selectedПокупательId != DBNull.Value)
                {
                    MessageBox.Show("Вы можете выбрать только поставщика или покупателя, но не оба одновременно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Преобразуем значения в целые числа или DBNull
                int? поставщикId = selectedПоставщикId != DBNull.Value ? Convert.ToInt32(selectedПоставщикId) : (int?)null;
                int? покупательId = selectedПокупательId != DBNull.Value ? Convert.ToInt32(selectedПокупательId) : (int?)null;

                // Преобразуем дату в строку формата "yyyy-MM-dd", чтобы сохранить только дату без времени
                string датаЗаключения = dateTimePickerДатаЗаключения.Value.ToString("yyyy-MM-dd");

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Договоры (Номер, Поставщик_id, Покупатель_id, ДатаЗаключения) " +
                                   "VALUES (@Номер, @Поставщик_id, @Покупатель_id, @ДатаЗаключения)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        // Устанавливаем параметры
                        command.Parameters.AddWithValue("@Номер", номер);
                        command.Parameters.AddWithValue("@Поставщик_id", поставщикId.HasValue ? поставщикId.Value : (object)DBNull.Value); // Если null, записываем DBNull
                        command.Parameters.AddWithValue("@Покупатель_id", покупательId.HasValue ? покупательId.Value : (object)DBNull.Value); // Если null, записываем DBNull
                        command.Parameters.AddWithValue("@ДатаЗаключения", датаЗаключения);

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
