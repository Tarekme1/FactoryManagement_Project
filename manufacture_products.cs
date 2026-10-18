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

namespace VP_Midterm_Project
{
    public partial class manufacture_products : Form
    {
        internal string stdName;

        public manufacture_products()
        {
            InitializeComponent();
        }

        private void manufacture_products_Load(object sender, EventArgs e)
        {
            string manufacturerID = stdName;
            string connectionString = DatabaseSettings.ConnectionString;
            string product_name = "";

            using (SqlConnection connnection = new SqlConnection(connectionString))
            {
                connnection.Open();
                string queryyy = "SELECT product_name FROM Products WHERE factory_id = @manufacturerID";
                SqlCommand commanddd = new SqlCommand(queryyy, connnection);
                commanddd.Parameters.AddWithValue("@manufacturerID", manufacturerID);

                SqlDataReader readerrrr = commanddd.ExecuteReader();
                while (readerrrr.Read())
                {
                    product_name = readerrrr.GetString(0);
                    comboBox1.Items.Add(product_name);
                }
                readerrrr.Close();
                connnection.Close();
            }

            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT name FROM Warehouses WHERE factory_id = @manufacturerId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@manufacturerId", manufacturerID);

            string queryy = "SELECT location FROM Warehouses WHERE factory_id = @manufacturerId";
            SqlCommand commandd = new SqlCommand(queryy, connection);
            commandd.Parameters.AddWithValue("@manufacturerId", manufacturerID);

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
                comboBox2.Items.Add($"{nameArray[k]} {locationArray[k]}");
                k++;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string manufacturer_factory_id = stdName;

            EmployeePage employeePage = new EmployeePage();
            employeePage.stdName = manufacturer_factory_id;
            employeePage.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string productt_name = comboBox1.Text;

            string warehouseName1 = comboBox2.Text;
            int index = warehouseName1.IndexOfAny("0123456789".ToCharArray());
            string warehouseName = index >= 0 ? warehouseName1.Substring(0, index + 1) : warehouseName1;

            int product_quantity = int.Parse(textBox1.Text);
            string warehouseID = "";
            string product_id = "";

            string connectionString = DatabaseSettings.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string querrry = "SELECT warehouse_id FROM Warehouses WHERE name = @warehouseName";
                SqlCommand command = new SqlCommand(querrry, connection);
                command.Parameters.AddWithValue("@warehouseName", warehouseName);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    warehouseID = reader.GetString(0);
                }
                reader.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string querry = "SELECT product_id FROM Products WHERE product_name = @productt_name";
                SqlCommand command = new SqlCommand(querry, connection);
                command.Parameters.AddWithValue("@productt_name", productt_name);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    product_id = reader.GetString(0);
                }
                reader.Close();
            }

            string query = "UPDATE Inventory SET quantity = quantity + @product_quantity, last_updated = GETDATE() WHERE product_id = @product_id AND warehouse_id = @warehouseID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_quantity", product_quantity);
                    command.Parameters.AddWithValue("@product_id", product_id);
                    command.Parameters.AddWithValue("@warehouseID", warehouseID);

                    int rowsAffected = command.ExecuteNonQuery();

                    MessageBox.Show($"{product_quantity} quantity of {productt_name.TrimEnd()} are successfully shipped to {warehouseName.TrimEnd()}");

                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("ERROR!");
                    }
                }

                connection.Close();
            }
        }

    }
}
