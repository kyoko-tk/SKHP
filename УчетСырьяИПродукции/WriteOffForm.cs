using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static SQLiteViewer.MainForm;

namespace SQLiteViewer
{
    public partial class WriteOffForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";

        public WriteOffForm()
        {
            InitializeComponent();
            LoadComboBoxes(); // Загружаем данные для ComboBox
        }

        // Загружаем данные в ComboBox для выбора покупателя, склада и продукции
        private void LoadComboBoxes()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Загрузка покупателей в ComboBox
                    string query = "SELECT Покупатель_id, Название FROM с_Покупатели";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        List<KeyValuePair<int, string>> buyers = new List<KeyValuePair<int, string>>();
                        while (reader.Read())
                        {
                            buyers.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0), // Покупатель_id
                                reader.GetString(1)  // Название покупателя
                            ));
                        }

                        comboBoxBuyer.DataSource = new BindingSource(buyers, null);
                        comboBoxBuyer.DisplayMember = "Value";
                        comboBoxBuyer.ValueMember = "Key";
                    }

                    // Загрузка складов в ComboBox
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

                        comboBoxWarehouse.DataSource = new BindingSource(warehouses, null);
                        comboBoxWarehouse.DisplayMember = "Value";
                        comboBoxWarehouse.ValueMember = "Key";
                    }

                    // Загрузка продукции в ComboBox
                    query = "SELECT Продукция_id, Название, Цена FROM с_Продукция";
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

                        comboBoxProduct.DataSource = new BindingSource(products, null);
                        comboBoxProduct.DisplayMember = "Value";
                        comboBoxProduct.ValueMember = "Key";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик изменения выбранной продукции для автозаполнения цены
        private void comboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxProduct.SelectedValue != null)
                {
                    // Получаем цену выбранной продукции
                    int productId = (int)comboBoxProduct.SelectedValue;
                    double price = GetProductPrice(productId);

                    // Автоматически подставляем цену в соответствующее поле
                    textBoxPrice.Text = price.ToString("F2");

                    // Обновляем сумму на основе текущего количества
                    double quantity = 0;
                    if (double.TryParse(textBoxQuantity.Text, out quantity))
                    {
                        UpdateSum(quantity);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке цены: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Получаем цену продукции из базы данных
        private double GetProductPrice(int productId)
        {
            double price = 0;
            try
            {
                string query = "SELECT Цена FROM с_Продукция WHERE Продукция_id = @ProductId";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);

                        // Логируем, что мы пытаемся выполнить запрос с нужным параметром
                        Logger.Log($"Executing query to get price for product with ID: {productId}");

                        var result = command.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            price = Convert.ToDouble(result);
                            Logger.Log($"Price for product {productId} is {price}");
                        }
                        else
                        {
                            // Логируем, если цена не найдена
                            Logger.Log($"Price for product {productId} not found or is NULL.");
                            MessageBox.Show("Цена для выбранного продукта не найдена в базе данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Logger.Log($"Error fetching product price: {ex.Message}");
                MessageBox.Show($"Ошибка при получении цены: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return price;
        }

        // Пересчитываем сумму на основе количества и цены
        private void RecalculateSum()
        {
            try
            {
                double quantity = double.Parse(textBoxQuantity.Text);
                double price = double.Parse(textBoxPrice.Text);
                double sum = quantity * price;
                textBoxSum.Text = sum.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при пересчете суммы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик для ввода количества
        private void textBoxQuantity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double quantity = 0;
                if (double.TryParse(textBoxQuantity.Text, out quantity))
                {
                    // Если количество корректное, пересчитываем сумму
                    UpdateSum(quantity);
                }
                else
                {
                    // Если введено некорректное значение
                    textBoxSum.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при пересчете суммы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateSum(double quantity)
        {
            try
            {
                double price = 0;
                if (double.TryParse(textBoxPrice.Text, out price))
                {
                    // Рассчитываем сумму
                    double sum = quantity * price;
                    textBoxSum.Text = sum.ToString("F2");
                }
                else
                {
                    textBoxSum.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении суммы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Сохранение записи о выбытии
        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                int buyerId = (int)comboBoxBuyer.SelectedValue;
                int warehouseId = (int)comboBoxWarehouse.SelectedValue;
                int productId = (int)comboBoxProduct.SelectedValue;

                double quantity = double.Parse(textBoxQuantity.Text);
                double price = double.Parse(textBoxPrice.Text);
                double sum = double.Parse(textBoxSum.Text);

                // Создаем временную базу данных
                string tempDatabase = "temp.db";
                File.Copy("DB.db", tempDatabase, true);

                // Используем временную базу данных для сохранения записи
                using (var connection = new SQLiteConnection($"Data Source={tempDatabase};Version=3;"))
                {
                    connection.Open();

                    // Начинаем транзакцию
                    using (var transaction = connection.BeginTransaction())
                    {
                        // Сохраняем запись о выбытии
                        SaveExitRecord(buyerId, warehouseId, productId, quantity, price, connection);

                        // Завершаем транзакцию
                        transaction.Commit();
                    }

                    connection.Close();
                }

                // Если все прошло успешно, заменяем оригинальную базу на временную
                File.Copy(tempDatabase, "DB.db", true);
                File.Delete(tempDatabase); // Удаляем временную базу данных

                // Удаляем последнюю строку после успешного сохранения
                RemoveLastLineFromTextBox();

                MessageBox.Show("Выбытие успешно добавлено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveLastLineFromTextBox()
        {
            if (textBoxSum.Text.Length > 0)
            {
                var lines = textBoxSum.Lines.ToList();
                lines.RemoveAt(lines.Count - 1); // Удаляем последнюю строку
                textBoxSum.Lines = lines.ToArray(); // Обновляем содержимое TextBox
            }
        }

        // Сохранение записи о выбытии в базу данных
        private void SaveExitRecord(int buyerId, int warehouseId, int productId, double quantity, double price, SQLiteConnection connection)
        {
            try
            {
                // Преобразуем текущую дату в строку в формате YYYY-MM-DD
                string date = DateTime.Now.ToString("yyyy-MM-dd");

                string query = "INSERT INTO Выбытие (Покупатель_id, Склад_id, Продукция_id, Количество, Дата, Цена) " +
                               "VALUES (@BuyerId, @WarehouseId, @ProductId, @Quantity, @Date, @Price)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    // Устанавливаем параметры
                    command.Parameters.AddWithValue("@BuyerId", buyerId);
                    command.Parameters.AddWithValue("@WarehouseId", warehouseId);
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Date", date); // Используем строку даты в нужном формате
                    command.Parameters.AddWithValue("@Price", price);

                    // Выполнение запроса
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
