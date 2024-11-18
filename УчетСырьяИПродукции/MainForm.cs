using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SQLiteViewer
{
    public partial class MainForm : Form
    {
        private readonly string connectionString = "Data Source=DB.db;Version=3;";
        private bool isAdmin;
        private Dictionary<string, string> foreignKeyMappings;
        private HashSet<string> primaryKeyIdFields;
        private readonly string logFilePath = "app_log.txt";

        public MainForm(bool isAdmin)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;

            foreignKeyMappings = AnalyzeDatabaseSchema();
            LoadTables();

            this.FormClosed += MainForm_FormClosed;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void LoadTables()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                DataTable tables = connection.GetSchema("Tables");

                TreeNode refsNode = new TreeNode("Справочники");
                TreeNode recordsNode = new TreeNode("Учет");
                TreeNode sysInfoNode = new TreeNode("System Information");

                foreach (DataRow row in tables.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    if (string.IsNullOrWhiteSpace(tableName)) continue;

                    if (tableName.StartsWith("с_"))
                    {
                        refsNode.Nodes.Add(new TreeNode(tableName.Substring(2)));
                    }
                    else if (tableName == "users" || tableName == "sqlite_sequence")
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

        private void LogMessage(string message)
        {
            string logEntry = $"{DateTime.Now}: {message}";
            Console.WriteLine(logEntry);  // Для вывода в консоль (опционально)

            try
            {
                using (var writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи лога: {ex.Message}");
            }
        }

        private Dictionary<string, string> AnalyzeDatabaseSchema()
        {
            var mappings = new Dictionary<string, string>();
            primaryKeyIdFields = new HashSet<string>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string schemaQuery = "SELECT sql FROM sqlite_master WHERE type='table'";
                using (var command = new SQLiteCommand(schemaQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tableSQL = reader.GetString(0);
                        LogMessage($"Analyzing table schema: {tableSQL}");
                        ParseTableSchema(tableSQL, mappings);
                    }
                }
            }
            return mappings;
        }

        private void ParseTableSchema(string tableSQL, Dictionary<string, string> mappings)
        {
            var tableNameMatch = Regex.Match(tableSQL, @"CREATE TABLE\s+`?(\w+)`?");
            if (!tableNameMatch.Success) return;
            string tableName = tableNameMatch.Groups[1].Value;

            var foreignKeyMatches = Regex.Matches(tableSQL, @"FOREIGN KEY\s*\(`?(\w+)`?\)\s*REFERENCES\s+`?(\w+)`?\s*\(`?(\w+)`?\)");
            foreach (Match fkMatch in foreignKeyMatches)
            {
                string foreignKeyColumn = fkMatch.Groups[1].Value;
                string referencedTable = fkMatch.Groups[2].Value;
                string referencedColumn = fkMatch.Groups[3].Value;

                // Добавляем в словарь: текущая таблица + столбец -> связанная таблица + колонка
                string fkMapping = $"{tableName}.{foreignKeyColumn}";
                mappings[fkMapping] = $"{referencedTable}.{referencedColumn}";
                LogMessage($"Detected FOREIGN KEY mapping: {fkMapping} -> {referencedTable}.{referencedColumn}");
            }
        }


        private void LoadTableData(string tableName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = BuildSelectQuery(tableName);

                using (var command = new SQLiteCommand(query, connection))
                using (var adapter = new SQLiteDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView.DataSource = dataTable;

                    LogMessage($"Loaded data for table: {tableName}");
                }
            }
        }

        private void HidePrimaryKeys(DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                string fullColumnName = $"{dataTable.TableName}.{column.ColumnName}";
                if (primaryKeyIdFields.Contains(fullColumnName) && !isAdmin)
                {
                    dataGridView.Columns[column.ColumnName].Visible = false;
                    LogMessage($"Hiding column for non-admin: {fullColumnName}");
                }
            }
        }


        private string BuildSelectQuery(string tableName)
        {
            string query = $"SELECT {tableName}.*";
            string joinClause = "";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (var fkMapping in foreignKeyMappings)
                {
                    // Проверяем, относится ли внешний ключ к текущей таблице
                    if (fkMapping.Key.StartsWith($"{tableName}."))
                    {
                        var column = fkMapping.Key.Split('.')[1]; // Имя столбца внешнего ключа
                        var refInfo = fkMapping.Value.Split('.'); // Разделяем на таблицу и столбец
                        string refTable = refInfo[0];
                        string refColumn = refInfo[1];

                        // Добавляем JOIN и столбец для отображения названия
                        joinClause += $" LEFT JOIN {refTable} ON {tableName}.{column} = {refTable}.{refColumn}";
                        query += $", {refTable}.Название AS `{column}_Name`";
                    }
                }
            }

            query += $" FROM {tableName}{joinClause}";
            LogMessage($"Constructed query for table '{tableName}': {query}");
            return query;
        }

    }
}
