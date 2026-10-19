using System;
using System.Collections;
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
    public partial class CustomerOrderPage : Form
    {
        internal string stdName;

        public CustomerOrderPage()
        {
            InitializeComponent();
        }

        private void CustomerOrderPage_Load(object sender, EventArgs e)
        {
            // retrieve category ID from Categories table
            string input = stdName; // category variable has the selected category name
            int cfIndex = input.IndexOf("CF");

            // Extract the text part and the CF number part
            string categoryName = input.Substring(0, cfIndex);
            string customerID = input.Substring(cfIndex, input.Length - cfIndex);
;
            string categoryId = "";
            string connectionString = DatabaseSettings.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT category_id FROM Categories WHERE category_name = @CategoryName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryName", categoryName);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    categoryId = reader.GetString(0);
                }
                reader.Close();
            }

            // retrieve matching products from Products table using category ID
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT product_name FROM products WHERE category_id = @category_id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@category_id", categoryId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productName = reader.GetString(0);
                            listBox.Items.Add(productName);
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT product_price FROM products WHERE category_id = @category_id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@category_id", categoryId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double productprice = reader.GetDouble(0);
                            string productprice2 = productprice.ToString();
                            listBox1.Items.Add("$" + productprice2);
                        }
                    }
                }
            }


        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string productName = listBox.SelectedItem.ToString();
            float productPrice = 0;

            string input = stdName; // category variable has the selected category name
            int cfIndex = input.IndexOf("CF");
            // Extract the text part and the CF number part
            string categoryName = input.Substring(0, cfIndex);
            string customerID = input.Substring(cfIndex, input.Length - cfIndex);
            string productID = "";

            string query = "SELECT product_price FROM Products WHERE product_name = @ProductName";
            using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", productName);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    object obj = reader.GetValue(0);
                    productPrice = (float)Convert.ToDouble(obj);
                }
            }

            string queryy = "SELECT product_id FROM Products WHERE product_name = @ProductName";
            using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
            {
                SqlCommand commandd = new SqlCommand(queryy, connection);
                commandd.Parameters.AddWithValue("@ProductName", productName);
                connection.Open();
                object readerr = commandd.ExecuteScalar();
                if (readerr != null)
                {
                    productID = readerr.ToString();
                }
            }

            quantity quantity1 = new quantity();
            quantity1.stdName = customerID.Trim() + " " + productID;
            quantity1.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Price_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomerLoginForm form = new CustomerLoginForm();
            form.Show();
            this.Hide();
        }
    }
}
