using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SQLiteViewer
{
    public partial class MainForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";
        private bool isAdmin;
        private BindingSource bindingSource = new BindingSource();
        private User authenticatedUser;
        private ToolStripStatusLabel toolStripStatusLabelUser;
        private Permissions permissions;  // Объект для разграничения прав

        public MainForm(bool isAdmin, User user)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            authenticatedUser = user; // Устанавливаем текущего пользователя
            permissions = Permissions.LoadPermissions();

            toolStripStatusLabelUser = new ToolStripStatusLabel
            {
                Text = $"Пользователь: {authenticatedUser.EmployeeName} ({authenticatedUser.Username})"
            };
            statusStrip1.Items.Add(toolStripStatusLabelUser);

            LoadTables(); // Загрузка всех таблиц
            ApplyAccessControl(); // Применяем доступы
            ApplyHiddenTables();  // Применение скрытия таблиц
        }

        public static class Logger
        {
            private static readonly string logFilePath = "logs.txt";

            public static void Log(string message)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                    }
                }
                catch
                {
                    // Игнорируем ошибки записи в лог, чтобы не нарушить выполнение программы
                }
            }

            public static void Log(string message, Exception ex)
            {
                Log($"{message} | Exception: {ex.Message}");
            }
        }

        public class User
        {
            public string Username { get; set; }
            public string Role { get; set; }
            public int UserId { get; set; }
            public string EmployeeName { get; set; }
        }

        private void LoadTables()
        {
            try
            {
                Logger.Log("Загрузка таблиц...");
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    DataTable tables = connection.GetSchema("Tables");

                    var refsNode = new TreeNode("Справочники");
                    var recordsNode = new TreeNode("Документы");
                    var sysInfoNode = new TreeNode("System Information");

                    foreach (DataRow row in tables.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        if (string.IsNullOrWhiteSpace(tableName)) continue;

                        if (tableName.StartsWith("с_"))
                        {
                            refsNode.Nodes.Add(new TreeNode(tableName.Substring(2)));
                        }
                        else if (tableName == "users" || tableName == "sqlite_sequence" || tableName == "ИсторияИзменений")
                        {
                            if (isAdmin)
                            {
                                sysInfoNode.Nodes.Add(new TreeNode(tableName));
                            }
                        }
                        else
                        {
                            recordsNode.Nodes.Add(new TreeNode(tableName));
                        }
                    }

                    treeViewTables.Nodes.Clear();
                    treeViewTables.Nodes.Add(refsNode);
                    treeViewTables.Nodes.Add(recordsNode);
                    if (isAdmin)
                    {
                        treeViewTables.Nodes.Add(sysInfoNode);
                    }

                    refsNode.Expand();
                    recordsNode.Expand();
                    sysInfoNode.Expand();

                    Logger.Log("Таблицы успешно загружены.");
                }

                ApplyHiddenTables(); // Применяем скрытие таблиц
            }
            catch (Exception ex)
            {
                Logger.Log("Ошибка при загрузке таблиц", ex);
                MessageBox.Show("Ошибка при загрузке таблиц. Проверьте журнал логов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyAccessControl()
        {
            Logger.Log($"Текущий пользователь: {authenticatedUser.Username}");

            if (permissions.UserSettings.ContainsKey(authenticatedUser.Username))
            {
                var userPermissions = permissions.UserSettings[authenticatedUser.Username];

                Logger.Log($"Найдено разграничение доступа для {authenticatedUser.Username}.");

                // Скрываем таблицы
                HideTables(userPermissions.HiddenTables);

                // Применяем доступ к кнопкам
                ApplyButtonVisibility(userPermissions.ButtonVisibility);
            }
            else
            {
                Logger.Log($"Разграничение доступа для {authenticatedUser.Username} не найдено.");
            }
        }
        private void HideTables(List<string> hiddenTables)
        {
            Logger.Log($"Скрываем таблицы для {authenticatedUser.Username}: {string.Join(", ", hiddenTables)}");

            foreach (TreeNode node in treeViewTables.Nodes)
            {
                foreach (TreeNode childNode in node.Nodes)
                {
                    if (hiddenTables.Contains(childNode.Text))
                    {
                        childNode.Remove();
                    }
                }
            }
        }

        private void ApplyButtonVisibility(Dictionary<string, bool> buttonVisibility)
        {
            Logger.Log($"Применяем доступ к кнопкам для {authenticatedUser.Username}: {JsonConvert.SerializeObject(buttonVisibility)}");

            bindingNavigatorAddNewItem.Visible = buttonVisibility.TryGetValue("canAdd", out var canAdd) && canAdd;
            bindingNavigatorDelete.Visible = buttonVisibility.TryGetValue("canDelete", out var canDelete) && canDelete;
            toolStripButtonSave.Visible = buttonVisibility.TryGetValue("canSave", out var canSave) && canSave;

            Logger.Log("Доступ к кнопкам успешно применен.");
        }

        private void ApplyHiddenTables()
        {
            try
            {
                Logger.Log("Применяем скрытие таблиц...");
                Logger.Log($"Текущий пользователь: {authenticatedUser.Username}");
                Logger.Log("Доступные пользователи в HiddenTables: " + string.Join(", ", permissions.HiddenTables.Keys));
                Logger.Log("Доступные пользователи в ButtonVisibility: " + string.Join(", ", permissions.ButtonVisibility.Keys));

                if (permissions.HiddenTables.ContainsKey(authenticatedUser.Username))
                {
                    Logger.Log($"Найдено разграничение доступа для {authenticatedUser.Username} в HiddenTables.");
                }
                else
                {
                    Logger.Log($"Разграничение доступа для {authenticatedUser.Username} в HiddenTables не найдено.");
                }

                if (permissions.ButtonVisibility.ContainsKey(authenticatedUser.Username))
                {
                    Logger.Log($"Найдено разграничение доступа для {authenticatedUser.Username} в ButtonVisibility.");
                }
                else
                {
                    Logger.Log($"Разграничение доступа для {authenticatedUser.Username} в ButtonVisibility не найдено.");
                }

                if (permissions.HiddenTables.ContainsKey(authenticatedUser.Username))
                {
                    var hiddenTables = permissions.HiddenTables[authenticatedUser.Username];
                    Logger.Log($"Скрываем таблицы: {string.Join(", ", hiddenTables)}");

                    foreach (TreeNode parentNode in treeViewTables.Nodes)
                    {
                        foreach (TreeNode childNode in parentNode.Nodes.Cast<TreeNode>().ToList())
                        {
                            if (hiddenTables.Contains(childNode.Text))
                            {
                                Logger.Log($"Скрыта таблица: {childNode.Text}");
                                parentNode.Nodes.Remove(childNode);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Ошибка при скрытии таблиц", ex);
                MessageBox.Show("Ошибка при применении скрытых таблиц.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class Permissions
        {
            public Dictionary<string, List<string>> HiddenTables { get; set; }
            public Dictionary<string, Dictionary<string, bool>> ButtonVisibility { get; set; }

            private static string permissionsFilePath = "permissions.json";
            public Dictionary<string, UserPermissions> UserSettings { get; set; }

            public Permissions()
            {
                HiddenTables = new Dictionary<string, List<string>>();
                ButtonVisibility = new Dictionary<string, Dictionary<string, bool>>();
                UserSettings = new Dictionary<string, UserPermissions>();
            }

            public class UserPermissions
            {
                public List<string> HiddenTables { get; set; }
                public Dictionary<string, bool> ButtonVisibility { get; set; }

                public UserPermissions()
                {
                    HiddenTables = new List<string>();
                    ButtonVisibility = new Dictionary<string, bool>();
                }
            }

            public static Permissions LoadPermissions()
            {
                if (File.Exists(permissionsFilePath))
                {
                    var json = File.ReadAllText(permissionsFilePath);
                    try
                    {
                        var root = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, UserPermissions>>>(json);

                        if (root != null && root.ContainsKey("UserSettings"))
                        {
                            return new Permissions
                            {
                                UserSettings = root["UserSettings"]
                            };
                        }

                        Logger.Log("Ошибка: JSON не содержит секции 'UserSettings'.");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Ошибка при разборе JSON: {ex.Message}");
                    }
                }

                return new Permissions(); // Возвращаем пустой объект, если файл не существует
            }

            public void SavePermissions()
            {
                var root = new Dictionary<string, Dictionary<string, UserPermissions>>
        {
            { "UserSettings", UserSettings }
        };

                var json = JsonConvert.SerializeObject(root, Formatting.Indented);
                File.WriteAllText(permissionsFilePath, json);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void TreeViewTables_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent != null && !string.IsNullOrEmpty(e.Node.Text))
            {
                string tableName = e.Node.Text;

                // Если узел принадлежит группе "Справочники", добавляем префикс "с_"
                if (e.Node.Parent.Text == "Справочники")
                {
                    tableName = "с_" + tableName;
                }

                // Проверка, скрыта ли таблица для текущего пользователя
                if (permissions.HiddenTables.ContainsKey(authenticatedUser.Username) &&
                    permissions.HiddenTables[authenticatedUser.Username].Contains(tableName))
                {
                    // Если таблица скрыта для пользователя, прекращаем обработку
                    MessageBox.Show("У вас нет доступа к этой таблице.", "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Загрузка данных в таблицу
                LoadTableData(tableName);
                bindingNavigator.Visible = true;

                // Проверяем, выбрана ли таблица "Договоры"
                toolStripDropDownButton1.Visible = tableName == "Договоры";
            }
        }

        private void ReplaceForeignKeysWithNames(DataTable dataTable, SQLiteConnection connection)
        {
            // Карта соответствий столбец - таблица
            var foreignKeyMap = new Dictionary<string, (string TableName, string NameColumn)>
    {
        { "Поставщик_id", ("с_Поставщики", "Название") },
        { "Покупатель_id", ("с_Покупатели", "Название") },
        { "Договор_id", ("Договоры", "Номер") },
        { "Склад_id", ("с_Склады", "Название") },
        { "Сырье_id", ("с_Сырье", "Название") },
        { "Подразделение_id", ("с_Подразделения", "Название") },
        { "Заведующий_id", ("с_Сотрудники", "ФИО") },
        { "Должность_id", ("с_Должности", "Название") }

    };

            // Получение списка столбцов для обработки (без первого столбца)
            var columnsToProcess = dataTable.Columns.Cast<DataColumn>()
                                       .Skip(1) // Пропускаем первый столбец
                                       .Select(c => c.ColumnName)
                                       .Where(c => foreignKeyMap.ContainsKey(c))
                                       .ToList();

            foreach (var foreignKeyColumn in columnsToProcess)
            {
                var (relatedTableName, nameColumn) = foreignKeyMap[foreignKeyColumn];

                // Создание словаря id -> название
                var idToNameMap = new Dictionary<int, string>();
                string query = $"SELECT {foreignKeyColumn}, {nameColumn} FROM \"{relatedTableName}\"";

                try
                {
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            idToNameMap[id] = name;
                        }
                    }

                    // Определение имени нового столбца
                    string nameColumnAlias = foreignKeyColumn.Replace("_id", "");

                    // Проверяем, существует ли столбец, если нет — добавляем его сразу за исходным
                    if (!dataTable.Columns.Contains(nameColumnAlias))
                    {
                        int foreignKeyIndex = dataTable.Columns.IndexOf(foreignKeyColumn);
                        dataTable.Columns.Add(nameColumnAlias, typeof(string));
                        dataTable.Columns[nameColumnAlias].SetOrdinal(foreignKeyIndex + 1);
                    }

                    // Заменяем id в строках на соответствующее имя
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row[foreignKeyColumn] != DBNull.Value)
                        {
                            int id = Convert.ToInt32(row[foreignKeyColumn]);
                            row[nameColumnAlias] = idToNameMap.ContainsKey(id)
                                ? (object)idToNameMap[id]
                                : DBNull.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Ошибка при обработке столбца {foreignKeyColumn}: {ex.Message}");
                }
            }
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                Logger.Log($"Загрузка данных из таблицы: {tableName}");
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM \"{tableName}\"";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Замена внешних ключей на названия
                        ReplaceForeignKeysWithNames(dataTable, connection);

                        // Привязываем данные к BindingSource
                        bindingSource.DataSource = dataTable;
                        dataGridView.DataSource = bindingSource;
                        bindingNavigator.BindingSource = bindingSource;

                        // Скрытие столбцов "_id" только для пользователей без прав администратора
                        if (!isAdmin)
                        {
                            foreach (DataGridViewColumn column in dataGridView.Columns)
                            {
                                if (column.Name.EndsWith("_id", StringComparison.OrdinalIgnoreCase))
                                    column.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Ошибка при загрузке таблицы {tableName}: {ex.Message}");
                MessageBox.Show($"Ошибка загрузки таблицы {tableName}. Проверьте логи для подробностей.");
            }
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {
            try
            {
                // Обновление состояния BindingNavigator
                if (bindingSource != null)
                {
                    // Обновляем BindingNavigator, если он связан с BindingSource
                    bindingNavigator.BindingSource = bindingSource;

                    // Обновление текста с количеством записей
                    bindingNavigator.CountItem.Text = $"из {bindingSource.Count}";
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Ошибка при обновлении элементов BindingNavigator", ex);
            }
        }

        private string GetSelectedTableName()
        {
            // Проверяем, выбран ли узел в TreeView
            if (treeViewTables.SelectedNode != null)
            {
                // Получаем имя выбранной таблицы
                string selectedTableName = treeViewTables.SelectedNode.Text;

                // Дополнительно: логируем выбор таблицы для отладки
                Logger.Log  ($"Выбрана таблица: {selectedTableName}");

                return selectedTableName;
            }

            // Дополнительно: логируем отсутствие выбора
            Logger.Log("Таблица не выбрана.");

            return string.Empty; // Возвращаем пустую строку, если ничего не выбрано
        }


        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем имя выбранной таблицы
                string selectedTableName = GetSelectedTableName();
                if (string.IsNullOrEmpty(selectedTableName)) return;

                // Если таблица "Договоры", открываем специальное окно для договоров
                if (selectedTableName.Equals("Договоры", StringComparison.OrdinalIgnoreCase))
                {
                    ContractForm contractForm = new ContractForm();
                    if (contractForm.ShowDialog() == DialogResult.OK)
                    {
                        // Перезагружаем данные после добавления записи
                        LoadTableData(selectedTableName);
                    }
                }
                // Если таблица "Поставки", открываем специальное окно для поставок
                else if (selectedTableName.Equals("Поставки", StringComparison.OrdinalIgnoreCase))
                {
                    SupplyForm supplyForm = new SupplyForm();
                    if (supplyForm.ShowDialog() == DialogResult.OK)
                    {
                        // Перезагружаем данные после добавления записи
                        LoadTableData(selectedTableName);
                    }
                }
                // Для всех остальных таблиц (кроме "Договоры" и "Поставки")
                else
                {
                    // Получаем источник данных
                    var dataTable = (DataTable)bindingSource.DataSource;
                    if (dataTable == null)
                    {
                        MessageBox.Show("Источник данных не найден для текущей таблицы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Добавляем новую пустую строку
                    DataRow newRow = dataTable.NewRow();
                    dataTable.Rows.Add(newRow);

                    // Сохраняем изменения, если необходимо
                    bindingSource.EndEdit();
                    MessageBox.Show("Пустая строка успешно добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void toolStripRefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем имя текущей выбранной таблицы
                string selectedTableName = GetSelectedTableName();

                if (string.IsNullOrEmpty(selectedTableName))
                {
                    MessageBox.Show("Пожалуйста, выберите таблицу для обновления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверяем, является ли выбранная таблица справочником
                if (IsDictionaryTable())
                {
                    selectedTableName = "с_" + selectedTableName; // Добавляем префикс "с_" для справочников
                }

                // Перезагружаем данные для выбранной таблицы
                LoadTableData(selectedTableName);
                Logger.Log($"Данные таблицы \"{selectedTableName}\" успешно обновлены.");
            }
            catch (Exception ex)
            {
                // Логируем и отображаем ошибку
                Logger.Log("Ошибка при обновлении данных", ex);
                MessageBox.Show("Произошла ошибка при обновлении данных. Проверьте логи для подробностей.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Вспомогательный метод для проверки, является ли таблица справочником
        private bool IsDictionaryTable()
        {
            return treeViewTables.SelectedNode?.Parent != null
                   && treeViewTables.SelectedNode.Parent.Text.Equals("Справочники", StringComparison.OrdinalIgnoreCase);
        }


        private void переподключитсяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Завершаем текущую сессию
                Logger.Log("Отключение текущего пользователя...");
                // Очистка данных сессии
                authenticatedUser = null; // Переменная, хранящая данные текущего пользователя
                Logger.Log("Сессия успешно завершена.");

                // 2. Показ формы входа
                Logger.Log("Показываем форму авторизации...");
                using (var loginForm = new LoginForm()) // Создаем экземпляр формы авторизации
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // Получаем данные о новом пользователе
                        authenticatedUser = loginForm.AuthenticatedUser;
                    }
                    else
                    {
                        Logger.Log("Пользователь отменил авторизацию.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибки, если они возникли
                Logger.Log($"Ошибка при переподключении: {ex.Message}");
                MessageBox.Show("Произошла ошибка при попытке переподключения. Повторите попытку позже.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void печатьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon...");
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем имя выбранной таблицы
                string selectedTableName = GetSelectedTableName();
                if (string.IsNullOrEmpty(selectedTableName))
                {
                    MessageBox.Show("Выберите таблицу для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Получаем DataTable из BindingSource
                var dataTable = (DataTable)bindingSource.DataSource;
                if (dataTable == null)
                {
                    MessageBox.Show("Ошибка: данные не найдены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool anyChanges = false;

                // Перебираем строки, обрабатываем только изменённые
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row.RowState == DataRowState.Modified) // Только изменённые строки
                    {
                        anyChanges = true;
                        SaveRowToSQLite(selectedTableName, row);
                    }
                }

                // Подтверждаем изменения
                if (anyChanges)
                {
                    dataTable.AcceptChanges(); // Подтверждаем изменения в DataTable
                    MessageBox.Show("Изменения успешно сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Нет изменений для сохранения.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Ошибка при сохранении данных", ex);
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveRowToSQLite(string tableName, DataRow row)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Формируем часть SQL для обновления данных
                    string setClause = string.Join(", ",
                        row.Table.Columns.Cast<DataColumn>()
                            .Where(c => !c.ColumnName.EndsWith("_id")) // Исключаем ID из списка обновляемых полей
                            .Select(c => $"{c.ColumnName} = @{c.ColumnName}"));

                    // Ищем колонку ID
                    string idColumnName = row.Table.Columns.Cast<DataColumn>()
                        .FirstOrDefault(c => c.ColumnName.EndsWith("_id"))?.ColumnName;

                    if (idColumnName == null)
                        throw new Exception("Столбец ID не найден.");

                    // SQL-запрос для обновления строки
                    string updateQuery = $"UPDATE {tableName} SET {setClause} WHERE {idColumnName} = @{idColumnName}";

                    using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                    {
                        // Добавляем параметры для всех колонок
                        foreach (DataColumn column in row.Table.Columns)
                        {
                            object value = column.ColumnName.EndsWith("_id")
                                ? row[column, DataRowVersion.Original] // Для ID используем оригинальное значение
                                : row[column]; // Для остальных колонок используем текущее значение
                            command.Parameters.AddWithValue($"@{column.ColumnName}", value ?? DBNull.Value);
                        }

                        // Выполняем команду
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows == 0)
                        {
                            throw new Exception($"Не удалось обновить запись ID = {row[idColumnName]}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Ошибка при сохранении строки в SQLite", ex);
                throw new Exception($"Ошибка при сохранении строки: {ex.Message}");
            }
        }

        private void bindingNavigatorDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, выбрана ли таблица
                string selectedTableName = GetSelectedTableName();
                if (string.IsNullOrEmpty(selectedTableName))
                {
                    MessageBox.Show("Выберите таблицу для удаления записи.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверяем, выбрана ли строка в DataGridView
                if (dataGridView.CurrentRow == null || dataGridView.CurrentRow.IsNewRow)
                {
                    MessageBox.Show("Выберите строку для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Получаем выбранную строку
                var selectedRow = dataGridView.CurrentRow;

                // Проверяем наличие первичного ключа
                string idColumnName = dataGridView.Columns.Cast<DataGridViewColumn>()
                    .FirstOrDefault(c => c.Name.EndsWith("_id", StringComparison.OrdinalIgnoreCase))?.Name;

                if (idColumnName == null)
                {
                    MessageBox.Show("Таблица не имеет первичного ключа. Удаление невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (selectedRow.Cells[idColumnName].Value == DBNull.Value)
                {
                    MessageBox.Show("Невозможно удалить строку без идентификатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int recordId = Convert.ToInt32(selectedRow.Cells[idColumnName].Value);

                // Подтверждаем удаление
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;

                // Удаление из базы данных
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = $"DELETE FROM \"{selectedTableName}\" WHERE {idColumnName} = @recordId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@recordId", recordId);
                        command.ExecuteNonQuery();
                    }
                }

                // Удаление из DataTable
                var dataTable = (DataTable)bindingSource.DataSource;

                if (dataTable.PrimaryKey.Length == 0)
                {
                    // Если первичный ключ не установлен, удаляем строку по значению ID
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row[idColumnName] != DBNull.Value && Convert.ToInt32(row[idColumnName]) == recordId)
                        {
                            row.Delete();
                            break;
                        }
                    }
                }
                else
                {
                    // Если первичный ключ установлен
                    var rowToDelete = dataTable.Rows.Find(recordId);
                    if (rowToDelete != null)
                    {
                        rowToDelete.Delete();
                    }
                }

                dataTable.AcceptChanges();

                MessageBox.Show("Запись успешно удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.Log($"Запись с ID {recordId} успешно удалена из таблицы {selectedTableName}.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Ошибка при удалении записи: {ex.Message}", ex);
                MessageBox.Show($"Произошла ошибка при удалении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void поставщикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем DataTable из BindingSource
                if (bindingSource?.DataSource is DataTable dataTable)
                {
                    // Применяем фильтр, скрывающий строки без данных в столбце "Поставщик"
                    dataTable.DefaultView.RowFilter = "[Поставщик] IS NOT NULL AND [Поставщик] <> ''";

                    // Скрываем столбец "Покупатель"
                    if (dataGridView.Columns["Покупатель"] != null)
                    {
                        dataGridView.Columns["Покупатель"].Visible = false;
                    }

                    // Отображаем столбец "Поставщик"
                    if (dataGridView.Columns["Поставщик"] != null)
                    {
                        dataGridView.Columns["Поставщик"].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Ошибка при фильтрации по поставщику", ex);
                MessageBox.Show("Произошла ошибка при фильтрации по поставщику.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void покупательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем DataTable из BindingSource
                if (bindingSource?.DataSource is DataTable dataTable)
                {
                    // Применяем фильтр, скрывающий строки без данных в столбце "Покупатель"
                    dataTable.DefaultView.RowFilter = "[Покупатель] IS NOT NULL AND [Покупатель] <> ''";

                    // Скрываем столбец "Поставщик"
                    if (dataGridView.Columns["Поставщик"] != null)
                    {
                        dataGridView.Columns["Поставщик"].Visible = false;
                    }

                    // Отображаем столбец "Покупатель"
                    if (dataGridView.Columns["Покупатель"] != null)
                    {
                        dataGridView.Columns["Покупатель"].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Ошибка при фильтрации по покупателю", ex);
                MessageBox.Show("Произошла ошибка при фильтрации по покупателю.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем DataTable из BindingSource
                if (bindingSource?.DataSource is DataTable dataTable)
                {
                    // Сбрасываем фильтр
                    dataTable.DefaultView.RowFilter = string.Empty;

                    // Отображаем оба столбца
                    if (dataGridView.Columns["Поставщик"] != null)
                    {
                        dataGridView.Columns["Поставщик"].Visible = true;
                    }

                    if (dataGridView.Columns["Покупатель"] != null)
                    {
                        dataGridView.Columns["Покупатель"].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Ошибка при очистке фильтрации", ex);
                MessageBox.Show("Произошла ошибка при очистке фильтрации.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            // Если текст равен "Поиск", очищаем поле
            if (toolStripTextBox1.Text == "Поиск")
            {
                toolStripTextBox1.Text = string.Empty;
            }
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // Если нажата клавиша Enter
            if (e.KeyCode == Keys.Enter)
            {
                toolStripSearch.PerformClick(); // Вызываем метод, как при клике на кнопку поиска
                e.Handled = true;  // Останавливаем дальнейшую обработку
                e.SuppressKeyPress = true; // Подавляем звук клавиши
            }
        }

        private void toolStripSearch_Click(object sender, EventArgs e)
        {
            // Получаем текст для поиска
            string searchText = toolStripTextBox1.Text.Trim();

            // Если поле пустое, сбрасываем фильтр и загружаем данные заново
            if (string.IsNullOrEmpty(searchText))
            {
                try
                {
                    // Сбрасываем фильтр и загружаем данные
                    bindingSource.Filter = null;
                    LoadTableData(GetSelectedTableName());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            try
            {
                // Убедимся, что источник данных - DataTable
                if (bindingSource.DataSource is DataTable dataTable)
                {
                    // Применяем фильтр к BindingSource
                    string filterExpression = string.Empty;

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (column.DataType == typeof(string)) // Проверяем только текстовые столбцы
                        {
                            // Добавляем фильтрацию по каждому текстовому столбцу
                            if (!string.IsNullOrEmpty(filterExpression))
                            {
                                filterExpression += " OR ";
                            }
                            filterExpression += $"[{column.ColumnName}] LIKE '%{searchText}%'";
                        }
                    }

                    // Применяем фильтр
                    if (!string.IsNullOrEmpty(filterExpression))
                    {
                        bindingSource.Filter = filterExpression;
                    }
                    else
                    {
                        MessageBox.Show("Текст для поиска не найден в таблице.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Данные для поиска не загружены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void разграничениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем роль пользователя
                if (authenticatedUser.Role != "admin")
                {
                    MessageBox.Show("У вас нет прав доступа к админ-панели.", "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Открываем форму админ-панели
                using (var formAccessControl = new FormAccessControl())
                {
                    formAccessControl.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при открытии админ-панели: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
