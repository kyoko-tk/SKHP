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

namespace SQLiteViewer
{
    public partial class LoginForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";

        public LoginForm()
        {
            InitializeComponent();
            // Назначаем обработчик события KeyDown для текстовых полей
            textBoxUsername.KeyDown += new KeyEventHandler(TextBox_KeyDown);
            textBoxPassword.KeyDown += new KeyEventHandler(TextBox_KeyDown);
        }

        private string HashPassword(string password)
        {
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
            string username = textBoxUsername.Text;
            string password = HashPassword(textBoxPassword.Text);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT role FROM users WHERE username = @username AND password = @password";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    object role = command.ExecuteScalar();
                    if (role != null)
                    {
                        string userRole = role.ToString();
                        MessageBox.Show("Авторизация успешна! Роль: " + userRole);

                        if (userRole == "admin")
                        {
                            MainForm mainForm = new MainForm(true); // Передаем true для прав администратора
                            this.Hide();
                            mainForm.ShowDialog();
                        }
                        else
                        {
                            MainForm mainForm = new MainForm(false);
                            this.Hide();
                            mainForm.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверное имя пользователя или пароль.");
                    }
                }
            }
        }

        private void ButtonRegister_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = HashPassword(textBoxPassword.Text);
            string role = "user"; // По умолчанию роль "user", может быть изменено на "admin" вручную

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO users (username, password, role) VALUES (@username, @password, @role)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@role", role);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Регистрация успешна!");
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
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
