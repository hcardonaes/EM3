using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Forms;

namespace EM3
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public partial class MainForm : Form
    {
        // Constructor de la clase MainForm
        public MainForm()
        {
            InitializeComponent();
            LoadData();
            InitializeMenu();
        }

        private void LoadData()
        {
            string connectionString = "Data Source=Millennium.db;Version=3;";
            string query = "SELECT * FROM personajes"; // Cambia "instituciones" por la tabla que desees mostrar

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable; // Asegúrate de que dataGridView1 es el nombre de tu DataGridView
                }
            }
        }

        private void InitializeMenu()
        {
            // Crear el menú
            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem databaseMenuItem = new ToolStripMenuItem("Base de Datos");
            menuStrip.Items.Add(databaseMenuItem);

            // Obtener las tablas de la base de datos
            string connectionString = "Data Source=Millennium.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                DataTable tables = connection.GetSchema("Tables");

                foreach (DataRow row in tables.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    ToolStripMenuItem tableMenuItem = new ToolStripMenuItem(tableName);
                    tableMenuItem.Click += (sender, e) => OpenTable(tableName);
                    databaseMenuItem.DropDownItems.Add(tableMenuItem);
                }
            }
            // Agregar el menú al formulario
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void OpenTable(string tableName)
        {
            string connectionString = "Data Source=Millennium.db;Version=3;";
            string query = $"SELECT * FROM {tableName}";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }
        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
