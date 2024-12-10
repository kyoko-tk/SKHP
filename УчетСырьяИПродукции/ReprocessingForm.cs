using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLiteViewer
{
    public partial class ReprocessingForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";

        public ReprocessingForm()
        {
            InitializeComponent();
            LoadComboBoxes(); // Загружаем данные для ComboBox
            textBoxRawMaterialQuantity.TextChanged += TextBoxRawMaterialQuantity_TextChanged; // Обработчик для изменения сырья
        }

        // Обработчик для вычисления и обновления "Количество продукции"
        private void TextBoxRawMaterialQuantity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, что введено числовое значение в "Количество сырья"
                if (double.TryParse(textBoxRawMaterialQuantity.Text, out double rawMaterialQuantity))
                {
                    // Если количество сырья больше 0, вычисляем "Количество продукции"
                    if (rawMaterialQuantity > 0)
                    {
                        double productQuantity = rawMaterialQuantity * 0.80; // Продукция = сырье * 0.80
                        textBoxProductQuantity.Text = productQuantity.ToString("0.##"); // Форматируем для отображения
                    }
                    else
                    {
                        textBoxProductQuantity.Clear(); // Если количество сырья меньше или равно 0, очищаем поле
                    }
                }
                else
                {
                    textBoxProductQuantity.Clear(); // Если введено неверное значение, очищаем поле
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вычислении количества продукции: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBoxes()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Загрузка сырья в ComboBox
                    string query = "SELECT Сырье_id, Название FROM с_Сырье";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        List<KeyValuePair<int, string>> rawMaterials = new List<KeyValuePair<int, string>>();
                        while (reader.Read())
                        {
                            rawMaterials.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0), // Сырье_id
                                reader.GetString(1)  // Название сырья
                            ));
                        }

                        comboBoxRawMaterial.DataSource = new BindingSource(rawMaterials, null);
                        comboBoxRawMaterial.DisplayMember = "Value";
                        comboBoxRawMaterial.ValueMember = "Key";
                    }

                    // Загрузка складов в ComboBox (для выбора "Место переработки")
                    query = "SELECT Склад_id, Название FROM с_Склады";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        List<KeyValuePair<int, string>> warehouses = new List<KeyValuePair<int, string>>();
                        while (reader.Read())
                        {
                            warehouses.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0), // Склад_id
                                reader.GetString(1)  // Название склада
                            ));
                        }

                        comboBoxFromWarehouse.DataSource = new BindingSource(warehouses, null);
                        comboBoxFromWarehouse.DisplayMember = "Value";
                        comboBoxFromWarehouse.ValueMember = "Key";
                    }

                    // Загрузка продукции в ComboBox (для выбора "Продукция")
                    query = "SELECT Продукция_id, Название FROM с_Продукция";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        List<KeyValuePair<int, string>> products = new List<KeyValuePair<int, string>>();
                        while (reader.Read())
                        {
                            products.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0), // Продукция_id
                                reader.GetString(1)  // Название продукции
                            ));
                        }

                        comboBoxToProduct.DataSource = new BindingSource(products, null);
                        comboBoxToProduct.DisplayMember = "Value";
                        comboBoxToProduct.ValueMember = "Key";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                int rawMaterialId = (int)comboBoxRawMaterial.SelectedValue;
                int warehouseId = (int)comboBoxFromWarehouse.SelectedValue;
                int productId = (int)comboBoxToProduct.SelectedValue;

                double rawMaterialQuantity = double.Parse(textBoxRawMaterialQuantity.Text);
                double productQuantity = double.Parse(textBoxProductQuantity.Text);

                // Проверка на доступность сырья на складе (условие: есть ли на складе нужное количество)
                double availableQuantity = GetAvailableRawMaterialQuantity(warehouseId, rawMaterialId);
                if (rawMaterialQuantity > availableQuantity)
                {
                    MessageBox.Show("На складе недостаточно сырья для переработки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Создаем временную базу данных
                string tempDatabase = "temp.db";
                File.Copy("DB.db", tempDatabase, true);

                // Используем временную базу данных для внесения изменений
                using (var connection = new SQLiteConnection($"Data Source={tempDatabase};Version=3;"))
                {
                    connection.Open();

                    // Сохраняем запись о переработке в базе данных
                    SaveReprocessingRecord(rawMaterialId, warehouseId, productId, rawMaterialQuantity, productQuantity, connection);

                    connection.Close();
                }

                // Если все прошло успешно, заменяем оригинальную базу на временную
                File.Copy(tempDatabase, "DB.db", true);
                File.Delete(tempDatabase); // Удаляем временную базу данных

                MessageBox.Show("Переработка успешно добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveReprocessingRecord(int rawMaterialId, int warehouseId, int productId, double rawMaterialQuantity, double productQuantity, SQLiteConnection connection)
        {
            string query = @"
        INSERT INTO Переработка (Сырье_id, Продукция_id, КоличествоСырья, КоличествоПродукции, МестоПереработки_id) 
        VALUES (@RawMaterialId, @ProductId, @RawMaterialQuantity, @ProductQuantity, @WarehouseId)"; // Добавлен параметр @WarehouseId для МестоПереработки_id

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RawMaterialId", rawMaterialId);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@RawMaterialQuantity", rawMaterialQuantity);
                command.Parameters.AddWithValue("@ProductQuantity", productQuantity);
                command.Parameters.AddWithValue("@WarehouseId", warehouseId); // Передаем warehouseId как МестоПереработки_id

                command.ExecuteNonQuery();
            }
        }

        private double GetAvailableRawMaterialQuantity(int warehouseId, int rawMaterialId)
        {
            // Получаем доступное количество сырья на складе
            double availableQuantity = 0;

            string query = "SELECT SUM(Колличество) FROM Поставки WHERE Склад_id = @WarehouseId AND Сырье_id = @RawMaterialId";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WarehouseId", warehouseId);
                    command.Parameters.AddWithValue("@RawMaterialId", rawMaterialId);

                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        availableQuantity = Convert.ToDouble(result);
                    }
                }
            }

            return availableQuantity;
        }
    }
}
