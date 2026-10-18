using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Xml.Linq;

namespace VP_Midterm_Project
{
    public partial class WarehousePage : Form
    {
        internal string stdName;

        public WarehousePage()
        {
            InitializeComponent();
        }

        private void WarehousePage_Load(object sender, EventArgs e)
        {
            string manufacturerId = stdName;

            string connectionString = DatabaseSettings.ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT name FROM Warehouses WHERE factory_id = @manufacturerId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@manufacturerId", manufacturerId);

            string queryy = "SELECT location FROM Warehouses WHERE factory_id = @manufacturerId";
            SqlCommand commandd = new SqlCommand(queryy, connection);
            commandd.Parameters.AddWithValue("@manufacturerId", manufacturerId);

            string[] nameArray = new string[5];
            string[] locationArray = new string[5];
            int i = 0;
            int j = 0;
            int k = 0;

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string warehouseName = reader.GetString(0);
                nameArray[i] = warehouseName;
                i++;
            }
            reader.Close();

            SqlDataReader readerr = commandd.ExecuteReader();
            while (readerr.Read())
            {
                string warehouselocation = readerr.GetString(0);
                locationArray[j] = warehouselocation;
                j++;
            }
            readerr.Close();

            while (k < i)
            {
                comboBox1.Items.Add($"{nameArray[k]} {locationArray[k]}");
                k++;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mfID = stdName;
            string warehouseName1 = comboBox1.Text;
            int index = warehouseName1.IndexOfAny("0123456789".ToCharArray()); 
            string warehouseName = index >= 0 ? warehouseName1.Substring(0, index + 1) : warehouseName1;

            warehouse_inventory warehouse_Inventory = new warehouse_inventory();
            warehouse_Inventory.stdName = warehouseName + "XXX" + mfID;
            warehouse_Inventory.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mfID = stdName;

            EmployeePage employeePage = new EmployeePage();
            employeePage.stdName = mfID;
            employeePage.Show();
            this.Hide();
        }
    }
}
