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
    public partial class MovementForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";

        public MovementForm()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            try
            {
                // Загружаем склады в ComboBox (для выбора "Со склада" и "На склад")
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Склад_id, Название FROM с_Склады"; // Запрос на получение всех складов
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

                        comboBoxToWarehouse.DataSource = new BindingSource(warehouses, null);
                        comboBoxToWarehouse.DisplayMember = "Value";
                        comboBoxToWarehouse.ValueMember = "Key";
                    }
                }

                // Загружаем сырьё в ComboBox (для выбора "Что перещещают")
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Сырье_id, Название FROM с_Сырье"; // Запрос на получение всех сырьевых позиций
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonConfirmMovement_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на то, что ComboBox не пуст
                if (comboBoxFromWarehouse.SelectedItem == null || comboBoxToWarehouse.SelectedItem == null ||
                    comboBoxRawMaterial.SelectedItem == null || string.IsNullOrEmpty(textBoxQuantity.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Получаем выбранные значения из ComboBox
                var fromWarehouseId = ((KeyValuePair<int, string>)comboBoxFromWarehouse.SelectedItem).Key;
                var toWarehouseId = ((KeyValuePair<int, string>)comboBoxToWarehouse.SelectedItem).Key;
                var rawMaterialId = ((KeyValuePair<int, string>)comboBoxRawMaterial.SelectedItem).Key;
                var quantity = Convert.ToDecimal(textBoxQuantity.Text);

                // Проверка, есть ли достаточно сырья на складе
                if (!IsSufficientStock(fromWarehouseId, rawMaterialId, quantity))
                {
                    MessageBox.Show("Недостаточно сырья на складе для перемещения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Сохраняем запись о перемещении
                SaveMovementRecord(fromWarehouseId, toWarehouseId, rawMaterialId, quantity);

                MessageBox.Show("Перемещение успешно сохранено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении перемещения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsSufficientStock(int warehouseId, int rawMaterialId, decimal quantity)
        {
            decimal availableQuantity = 0;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT SUM(Колличество) FROM Поставки WHERE Склад_id = @WarehouseId AND Сырье_id = @RawMaterialId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@WarehouseId", warehouseId);
                        command.Parameters.AddWithValue("@RawMaterialId", rawMaterialId);

                        availableQuantity = Convert.ToDecimal(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке наличия сырья: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return availableQuantity >= quantity;
        }

        private void SaveMovementRecord(int fromWarehouseId, int toWarehouseId, int rawMaterialId, decimal quantity)
        {
            try
            {
                // Создаем временную базу данных
                string tempDatabase = "temp.db";
                File.Copy("DB.db", tempDatabase, true);

                // Используем временную базу данных для внесения изменений
                using (var connection = new SQLiteConnection($"Data Source={tempDatabase};Version=3;"))
                {
                    connection.Open();
                    string query = @"
                INSERT INTO Перемещения (СоСклада_id, НаСклад_id, Сырье_id, Количество, Дата) 
                VALUES (@FromWarehouseId, @ToWarehouseId, @RawMaterialId, @Quantity, @Date)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FromWarehouseId", fromWarehouseId);
                        command.Parameters.AddWithValue("@ToWarehouseId", toWarehouseId);
                        command.Parameters.AddWithValue("@RawMaterialId", rawMaterialId);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@Date", DateTime.Now);

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                // Если все прошло успешно, заменяем оригинальную базу на временную
                File.Copy(tempDatabase, "DB.db", true);
                File.Delete(tempDatabase); // Удаляем временную базу данных

                MessageBox.Show("Перемещение успешно добавлено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении перемещения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
