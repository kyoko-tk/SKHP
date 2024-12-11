using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SQLiteViewer
{
    public partial class UserManagementForm : Form
    {
        public UserManagementForm()
        {
            InitializeComponent();
            LoadData();
        }

        // Метод для загрузки данных из базы данных
        private void LoadData()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=DB.db;Version=3;"))
            {
                conn.Open();

                // Заполнение ComboBox для подразделений
                string query = "SELECT Подразделение_id, Название FROM с_Подразделения";
                SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn);
                var dt = new System.Data.DataTable();
                da.Fill(dt);
                comboBoxDepartment.DataSource = dt;
                comboBoxDepartment.DisplayMember = "Название"; // Показываем Название
                comboBoxDepartment.ValueMember = "Подразделение_id"; // ID будет скрыт, но доступен как Value

                // Заполнение ComboBox для должностей
                query = "SELECT Должность_id, Название FROM с_Должности";
                da = new SQLiteDataAdapter(query, conn);
                dt = new System.Data.DataTable();
                da.Fill(dt);
                comboBoxPosition.DataSource = dt;
                comboBoxPosition.DisplayMember = "Название"; // Показываем Название
                comboBoxPosition.ValueMember = "Должность_id"; // ID будет скрыт, но доступен как Value

                // Заполнение ComboBox для выбора пользователя при удалении
                query = "SELECT username FROM users";
                da = new SQLiteDataAdapter(query, conn);
                dt = new System.Data.DataTable();
                da.Fill(dt);
                comboBoxDeleteUser.DataSource = dt;
                comboBoxDeleteUser.DisplayMember = "username"; // Показываем username

                // Заполнение ComboBox для выбора роли
                comboBoxRole.Items.Add("admin");
                comboBoxRole.Items.Add("user");
                comboBoxRole.SelectedIndex = 1; // По умолчанию роль user
            }
        }

        // Метод для хеширования пароля
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Обработчик события для добавления пользователя
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string fio = textBoxFullName.Text;
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            int departmentId = Convert.ToInt32(comboBoxDepartment.SelectedValue ?? 0); // Получаем ID выбранного подразделения
            int positionId = Convert.ToInt32(comboBoxPosition.SelectedValue ?? 0); // Получаем ID выбранной должности
            string role = comboBoxRole.SelectedItem.ToString(); // Получаем выбранную роль

            // Проверка на уникальность username в временной базе данных
            string tempDatabase = "temp.db";
            File.Copy("DB.db", tempDatabase, true); // Копируем оригинальную базу в временную

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={tempDatabase};Version=3;"))
                {
                    conn.Open();

                    // Проверка на уникальность username
                    string query = "SELECT COUNT(*) FROM users WHERE username = @username";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                        if (userCount > 0)
                        {
                            MessageBox.Show("Это имя пользователя уже существует. Пожалуйста, выберите другое.");
                            return;
                        }
                    }

                    // Хеширование пароля
                    string hashedPassword = HashPassword(password);

                    // Вставка в таблицу пользователей
                    query = "INSERT INTO users (username, password, role) VALUES (@username, @password, @role)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.ExecuteNonQuery();
                    }

                    // Получаем user_id последнего добавленного пользователя
                    query = "SELECT last_insert_rowid()";
                    int userId;
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        userId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Добавление записи в таблицу сотрудников
                    query = "INSERT INTO с_Сотрудники (ФИО, Подразделение_id, Должность_id, user_id) VALUES (@fio, @departmentId, @positionId, @userId)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fio", fio);
                        cmd.Parameters.AddWithValue("@departmentId", departmentId);
                        cmd.Parameters.AddWithValue("@positionId", positionId);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();
                    }

                    // Подтверждаем транзакцию и сохраняем изменения
                    conn.Close();
                }

                // Если все прошло успешно, заменяем оригинальную базу на временную
                File.Copy(tempDatabase, "DB.db", true); // Копируем временную базу на место оригинальной
                File.Delete(tempDatabase); // Удаляем временную базу данных

                MessageBox.Show("Пользователь успешно добавлен!");
                LoadData(); // Перезагружаем данные на форме
            }
            catch (Exception ex)
            {
                // В случае ошибки восстанавливаем оригинальную базу данных
                File.Copy("DB.db", tempDatabase, true); // Восстановление временной базы
                File.Delete(tempDatabase);
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик события для удаления пользователя
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем выбранный элемент из comboBoxDeleteUser
                DataRowView selectedItem = comboBoxDeleteUser.SelectedItem as DataRowView;
                if (selectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите пользователя для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string usernameToDelete = selectedItem["username"].ToString(); // Получаем username из выбранного элемента

                // Создаем временную базу данных
                string tempDatabase = "temp.db";
                File.Copy("DB.db", tempDatabase, true);

                // Используем временную базу данных для удаления пользователя
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={tempDatabase};Version=3;"))
                {
                    conn.Open();

                    // Начинаем транзакцию
                    using (var transaction = conn.BeginTransaction())
                    {
                        // Получаем user_id по username
                        string query = "SELECT user_id FROM users WHERE username = @username";
                        int userId = -1;
                        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", usernameToDelete);
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                userId = Convert.ToInt32(result);
                            }
                            else
                            {
                                MessageBox.Show($"Пользователь с именем {usernameToDelete} не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        // Удаление из таблицы сотрудников
                        query = "DELETE FROM с_Сотрудники WHERE user_id = @userId";
                        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);
                            cmd.ExecuteNonQuery();
                        }

                        // Удаление из таблицы users
                        query = "DELETE FROM users WHERE username = @username";
                        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", usernameToDelete);
                            cmd.ExecuteNonQuery();
                        }

                        // Подтверждаем транзакцию
                        transaction.Commit();
                    }

                    conn.Close();
                }

                // Если все прошло успешно, заменяем оригинальную базу на временную
                File.Copy(tempDatabase, "DB.db", true);
                File.Delete(tempDatabase); // Удаляем временную базу данных

                MessageBox.Show("Пользователь успешно удален!");
                LoadData(); // Перезагружаем данные на форме
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
