using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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

        public MainForm(bool isAdmin)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            this.bindingSource = new BindingSource();
            LoadTables();
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
                    var recordsNode = new TreeNode("Учет");
                    var sysInfoNode = new TreeNode("System Information");

                    foreach (DataRow row in tables.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        if (string.IsNullOrWhiteSpace(tableName)) continue;

                        if (tableName.StartsWith("с_"))
                        {
                            refsNode.Nodes.Add(new TreeNode(tableName.Substring(2))); // Убираем префикс "с_"
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

                    treeViewTables.Nodes.Add(refsNode);
                    treeViewTables.Nodes.Add(recordsNode);
                    if (isAdmin)
                    {
                        treeViewTables.Nodes.Add(sysInfoNode);
                    }

                    refsNode.Expand();
                    recordsNode.Expand();
                    sysInfoNode.Expand();
                }
                Logger.Log("Таблицы успешно загружены.");
            }
            catch (Exception ex)
            {
                Logger.Log("Ошибка при загрузке таблиц", ex);
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
                if (e.Node.Parent.Text == "Справочники")
                {
                    tableName = "с_" + tableName;
                }

                LoadTableData(tableName);
            }
        }

        private void ReplaceForeignKeysWithNames(DataTable dataTable, SQLiteConnection connection)
        {
            // Карта соответствий столбец - таблица
            var foreignKeyMap = new Dictionary<string, (string TableName, string NameColumn)>
    {
        { "Поставщик_id", ("с_Поставщики", "Название") },
        { "Покупатель_id", ("с_Покупатели", "Название") }
    };

            foreach (var columnMapping in foreignKeyMap)
            {
                string foreignKeyColumn = columnMapping.Key;
                string relatedTableName = columnMapping.Value.TableName;
                string nameColumn = columnMapping.Value.NameColumn;

                if (!dataTable.Columns.Contains(foreignKeyColumn))
                    continue; // Если столбец отсутствует, пропускаем

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

                    // Добавляем столбец с названием
                    string nameColumnAlias = foreignKeyColumn.Replace("_id", "_Название");
                    if (!dataTable.Columns.Contains(nameColumnAlias))
                    {
                        dataTable.Columns.Add(nameColumnAlias, typeof(string));
                    }

                    // Заменяем id на названия
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

                        // Скрытие столбцов "_id"
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            if (column.Name.EndsWith("_id", StringComparison.OrdinalIgnoreCase))
                                column.Visible = false;
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
                return treeViewTables.SelectedNode.Text;
            }
            return string.Empty; // Возвращаем пустую строку, если ничего не выбрано
        }


        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем имя выбранной таблицы из дерева
                string selectedTableName = GetSelectedTableName();
                if (string.IsNullOrEmpty(selectedTableName)) return;

                // Открываем форму для добавления новой записи
                AddRecordForm addRecordForm = new AddRecordForm();
                addRecordForm.ShowDialog();

                // После добавления записи, можно перезагрузить данные в основной форме, если это необходимо
                LoadTableData(selectedTableName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы добавления: {ex.Message}");
            }
        }

        private void toolStripRefreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем имя текущей выбранной таблицы
                string selectedTableName = GetSelectedTableName();

                // Если таблица выбрана
                if (!string.IsNullOrEmpty(selectedTableName))
                {
                    // Перезагружаем данные для выбранной таблицы
                    LoadTableData(selectedTableName);
                    Logger.Log($"Данные таблицы {selectedTableName} успешно обновлены.");
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите таблицу для обновления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Logger.Log("Ошибка при обновлении данных", ex);
                MessageBox.Show($"Произошла ошибка при обновлении данных. Проверьте логи для подробностей.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void переподключитсяToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
