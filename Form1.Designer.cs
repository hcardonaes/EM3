using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Forms;

namespace EM3
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public partial class Form1 : Form
    {
        private DataGridView dataGridView1;
        private Button btnCreate;
        private Button btnRead;
        private Button btnUpdate;
        private Button btnDelete;

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

        private void AdjustDataGridViewSize()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void InitializeCrudButtons()
        {
            // Crear botones CRUD
            btnCreate = new Button { Text = "Crear", Left = 300, Top = 300, Width = 80 };
            btnRead = new Button { Text = "Leer", Left = 400, Top = 300, Width = 80 };
            btnUpdate = new Button { Text = "Actualizar", Left = 500, Top = 300, Width = 80 };
            btnDelete = new Button { Text = "Eliminar", Left = 600, Top = 300, Width = 80 };

            // Agregar eventos a los botones
            btnCreate.Click += BtnCreate_Click;
            btnRead.Click += BtnRead_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;

            // Agregar botones al formulario
            this.Controls.Add(btnCreate);
            this.Controls.Add(btnRead);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
        }

        private DataGridView dataGridView2;
    }
}
