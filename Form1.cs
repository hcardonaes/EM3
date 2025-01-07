using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace EM3
{
    public partial class Form1 : Form
    {
        private string currentTableName = "personajes"; // Tabla por defecto

        public Form1()
        {
            InitializeComponent();
            LoadData();
            InitializeMenu();
            InitializeCrudButtons();
        }

        private void LoadData()
        {
            string connectionString = "Data Source=Millennium.db;Version=3;";
            string query = $"SELECT * FROM {currentTableName}";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    AdjustDataGridViewSize();
                }
            }
        }

        private void OpenTable(string tableName)
        {
            currentTableName = tableName; // Actualiza la tabla actual
            LoadData();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=Millennium.db;Version=3;";
            string columnNames = "";
            string columnValues = "";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Obtener los nombres de las columnas de la tabla actual
                string query = $"PRAGMA table_info({currentTableName})";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string columnName = reader["name"].ToString();
                            if (columnName != "Id") // Suponiendo que "Id" es una columna autoincremental
                            {
                                if (!string.IsNullOrEmpty(columnNames))
                                {
                                    columnNames += ", ";
                                    columnValues += ", ";
                                }
                                columnNames += columnName;
                                columnValues += $"'{GetDefaultValueForColumn(columnName)}'";
                            }
                        }
                    }
                }

                // Construir la consulta INSERT INTO
                query = $"INSERT INTO {currentTableName} ({columnNames}) VALUES ({columnValues})";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            LoadData();
        }

        private string GetDefaultValueForColumn(string columnName)
        {
            // Aquí puedes definir valores predeterminados para cada columna según sea necesario
            // Por ejemplo, puedes devolver una cadena vacía para columnas de texto, 0 para columnas numéricas, etc.
            // En este ejemplo, se devuelve una cadena vacía para todas las columnas

            // Obtener el tipo de columna desde la base de datos
            string connectionString = "Data Source=Millennium.db;Version=3;";
            string columnType = "";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"PRAGMA table_info({currentTableName})";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == columnName)
                            {
                                columnType = reader["type"].ToString().ToLower();
                                break;
                            }
                        }
                    }
                }
            }

            // Devolver valores predeterminados según el tipo de columna
            switch (columnType)
            {
                case "text":
                case "varchar":
                case "char":
                    return ""; // Cadena vacía para columnas de texto
                case "integer":
                case "int":
                case "smallint":
                case "bigint":
                    return "0"; // 0 para columnas numéricas
                case "real":
                case "double":
                case "float":
                    return "0.0"; // 0.0 para columnas de punto flotante
                case "date":
                case "datetime":
                    return DateTime.Now.ToString("yyyy-MM-dd"); // Fecha actual para columnas de fecha
                case "boolean":
                case "bool":
                    return "0"; // 0 para columnas booleanas (falso)
                default:
                    return ""; // Valor predeterminado para otros tipos de columnas
            }
        }



        private void BtnRead_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string connectionString = "Data Source=Millennium.db;Version=3;";
                string id = dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
                string query = $"UPDATE {currentTableName} SET Nombre = 'Personaje Actualizado', Edad = 1 WHERE Id = {id}";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                LoadData();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string connectionString = "Data Source=Millennium.db;Version=3;";
                string id = dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
                string query = $"DELETE FROM {currentTableName} WHERE Id = {id}";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                LoadData();
            }
        }


        private string GetDebuggerDisplay()
        {
            return ToString();
        }

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 297);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1064, 266);
            dataGridView1.TabIndex = 0;
            // 
            // Form1
            // 
            ClientSize = new Size(1088, 641);
            Controls.Add(dataGridView1);
            Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }
    }
}
