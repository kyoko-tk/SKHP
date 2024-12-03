using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SQLiteViewer.MainForm;

namespace SQLiteViewer
{
    public partial class LoginForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";
        public User AuthenticatedUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            // Назначаем обработчик события KeyDown для текстовых полей
            textBoxUsername.KeyDown += new KeyEventHandler(TextBox_KeyDown);
            textBoxPassword.KeyDown += new KeyEventHandler(TextBox_KeyDown);

        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";  // Если пароль пустой, возвращаем пустую строку

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Имя пользователя не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string hashedPassword = HashPassword(password);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Измененный запрос для получения роли и user_id, а также извлечение данных из таблицы "с_Сотрудники"
                string query = @"
                SELECT users.role, users.user_id, сотрудники.ФИО
                FROM users 
                JOIN с_Сотрудники сотрудники ON users.user_id = сотрудники.user_id
                WHERE users.username = @username AND (users.password = @password OR @password = '')";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", hashedPassword);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string userRole = reader["role"].ToString();
                            int userId = Convert.ToInt32(reader["user_id"]);
                            string employeeName = reader["ФИО"].ToString();

                            // Сохраняем информацию о текущем пользователе
                            AuthenticatedUser = new User
                            {
                                Username = username,
                                Role = userRole,
                                UserId = userId,
                                EmployeeName = employeeName  // Добавляем ФИО сотрудника
                            };

                            MessageBox.Show("Авторизация успешна! Роль: " + userRole);

                            // Передаем объект AuthenticatedUser в MainForm
                            MainForm mainForm = new MainForm(userRole == "admin", AuthenticatedUser); // Передаем и текущего пользователя
                            this.Hide();
                            mainForm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Неверное имя пользователя или пароль.");
                        }
                    }
                }
            }
        }

    private void ButtonRegister_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Имя пользователя не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string hashedPassword = HashPassword(password);
            string role = "user"; // По умолчанию роль "user", может быть изменено на "admin" вручную

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Проверим, существует ли уже такой пользователь
                string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                using (var checkCommand = new SQLiteCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@username", username);
                    long userCount = (long)checkCommand.ExecuteScalar();
                    if (userCount > 0)
                    {
                        MessageBox.Show("Пользователь с таким именем уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string query = "INSERT INTO users (username, password, role) VALUES (@username, @password, @role)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", hashedPassword);
                    command.Parameters.AddWithValue("@role", role);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Регистрация успешна!");
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Ошибка при регистрации: " + ex.Message);
                    }
                }
            }
        }

        // Обработчик события KeyDown для нажатия Enter
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // подавляет звуковой сигнал при нажатии Enter
                ButtonLogin_Click(sender, e); // Вызываем метод для авторизации
            }
        }
    }
}
