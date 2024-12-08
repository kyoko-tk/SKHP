using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;

namespace SQLiteViewer
{
    public partial class FormAccessControl : Form
    {
        private Permissions permissions;
        private string currentUsername;

        public FormAccessControl()
        {
            InitializeComponent();
            permissions = Permissions.LoadPermissions();  // Загружаем текущие настройки
            LoadUsers();
        }

        // Загружаем всех пользователей
        public List<string> GetAllUsers()
        {
            var users = new List<string>();
            string query = "SELECT username FROM users";  // Строка запроса для получения списка пользователей

            using (var connection = new SQLiteConnection("Data Source=DB.db;Version=3;"))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(reader["username"].ToString());
                        }
                    }
                }
            }
            return users;
        }

        // Загружаем все таблицы
        public List<string> GetAllTables()
        {
            var tables = new List<string>();
            string query = "SELECT name FROM sqlite_master WHERE type='table'";

            using (var connection = new SQLiteConnection("Data Source=DB.db;Version=3;"))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader["name"].ToString());
                        }
                    }
                }
            }
            return tables;
        }

        // Загружаем пользователей в ComboBox
        private void LoadUsers()
        {
            List<string> users = GetAllUsers();
            comboBoxUsers.Items.Clear();
            foreach (var user in users)
            {
                comboBoxUsers.Items.Add(user);
            }

            if (comboBoxUsers.Items.Count > 0)
            {
                comboBoxUsers.SelectedIndex = 0; // Выбираем первого пользователя по умолчанию
            }
        }

        // Загружаем настройки для выбранного пользователя
        private void LoadSettings()
        {
            if (string.IsNullOrEmpty(currentUsername)) return;

            // Загружаем настройки для выбранного пользователя
            if (permissions.UserSettings.ContainsKey(currentUsername))
            {
                var userPermissions = permissions.UserSettings[currentUsername];

                // Сначала очищаем список, чтобы избежать дублирования
                listBoxTables.Items.Clear();

                // Загружаем таблицы из базы данных
                List<string> allTables = GetAllTables();
                foreach (var table in allTables)
                {
                    listBoxTables.Items.Add(table);
                }

                // Помечаем скрытые таблицы как выбранные в listBox
                foreach (var table in userPermissions.HiddenTables)
                {
                    int index = listBoxTables.Items.IndexOf(table);
                    if (index >= 0)
                    {
                        listBoxTables.SetSelected(index, true); // Помечаем таблицу как скрытую
                    }
                }

                // Загружаем доступ к кнопкам
                checkBoxCanAdd.Checked = userPermissions.ButtonVisibility["canAdd"];
                checkBoxCanDelete.Checked = userPermissions.ButtonVisibility["canDelete"];
                checkBoxCanSave.Checked = userPermissions.ButtonVisibility["canSave"];
            }
        }

        // Обработчик изменения выбора пользователя в ComboBox
        private void comboBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Сохраняем настройки для предыдущего пользователя перед изменением
            if (!string.IsNullOrEmpty(currentUsername))
            {
                SaveSettingsForCurrentUser();
            }

            // Загружаем настройки для нового пользователя
            currentUsername = comboBoxUsers.SelectedItem.ToString();
            LoadSettings(); // Загружаем настройки для нового пользователя
        }

        // Сохраняем настройки для текущего пользователя
        private void SaveSettingsForCurrentUser()
        {
            if (string.IsNullOrEmpty(currentUsername)) return;

            var userPermissions = new UserPermissions
            {
                // Сохраняем скрытые таблицы
                HiddenTables = listBoxTables.SelectedItems.Cast<string>().ToList(),

                // Сохраняем настройки для кнопок
                ButtonVisibility = new Dictionary<string, bool>
                {
                    { "canAdd", checkBoxCanAdd.Checked },
                    { "canDelete", checkBoxCanDelete.Checked },
                    { "canSave", checkBoxCanSave.Checked }
                }
            };

            permissions.UserSettings[currentUsername] = userPermissions;
            permissions.SavePermissions();
            MessageBox.Show("Изменения сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обработчик кнопки сохранения
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveSettingsForCurrentUser();
        }
    }

    // Класс для хранения настроек прав доступа
    public class Permissions
    {
        public Dictionary<string, UserPermissions> UserSettings { get; set; }

        private static string permissionsFilePath = "permissions.json";

        public Permissions()
        {
            // Инициализация по умолчанию
            UserSettings = new Dictionary<string, UserPermissions>();
        }

        // Загрузка данных из JSON файла
        public static Permissions LoadPermissions()
        {
            if (File.Exists(permissionsFilePath))
            {
                var json = File.ReadAllText(permissionsFilePath);
                return JsonConvert.DeserializeObject<Permissions>(json);
            }

            return new Permissions(); // Возвращаем пустой объект, если файл не существует
        }

        // Сохранение данных в JSON файл
        public void SavePermissions()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(permissionsFilePath, json);
        }
    }

    // Класс для хранения настроек доступа для каждого пользователя
    public class UserPermissions
    {
        public List<string> HiddenTables { get; set; }
        public Dictionary<string, bool> ButtonVisibility { get; set; }

        public UserPermissions()
        {
            HiddenTables = new List<string>();
            ButtonVisibility = new Dictionary<string, bool>
            {
                { "canAdd", true },
                { "canDelete", true },
                { "canSave", true }
            };
        }
    }
}
