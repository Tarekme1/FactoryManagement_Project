using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VP_Midterm_Project
{
    public partial class quantity : Form
    {
        internal object stdName;

        public quantity()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            string quantity1 = textBox.Text;
            int quantity;

            if (!OrderQuantityValidator.TryGetPositiveQuantity(quantity1, out quantity))
            {
                MessageBox.Show("Please enter a whole quantity greater than zero.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string variable = (string)stdName; 
            string[] words = variable.Split();
            string customerID = words[0];
            string productID = words[1];
            int quantityInInventory = 0;

            //Get the LOCATION of the CUSTOMER FACTORY using customer ID
            string locationOfCustomerFactory = string.Empty;

            string retrieveLocationQuery = "SELECT location FROM CustomerFactories WHERE customer_factory_id = @customerID";

            using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
            {
                SqlCommand retrieveLocationCommand = new SqlCommand(retrieveLocationQuery, connection);
                retrieveLocationCommand.Parameters.AddWithValue("@customerID", customerID);
                connection.Open();
                using (SqlDataReader reader = retrieveLocationCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        locationOfCustomerFactory = reader["location"].ToString();
                    }
                }
            }

            //Get the factory ID using product ID (MF ID)
            string factoryID = string.Empty;

            string retrieveFactoryIDQuery = "SELECT factory_id FROM Products WHERE product_id = @productID";

            using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
            {
                SqlCommand retrieveFactoryIDCommand = new SqlCommand(retrieveFactoryIDQuery, connection);
                retrieveFactoryIDCommand.Parameters.AddWithValue("@productID", productID);
                connection.Open();
                using (SqlDataReader reader = retrieveFactoryIDCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        factoryID = reader["factory_id"].ToString();
                    }
                }
            }

            //Get WarehouseID using Location and FactoryID 
            string warehouseID = string.Empty;

            string warehouseIDQuery = "SELECT warehouse_id FROM Warehouses WHERE location = @location AND factory_id = @factoryID";

            using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
            {
                SqlCommand warehouseIDQueryCommand = new SqlCommand(warehouseIDQuery, connection);
                warehouseIDQueryCommand.Parameters.AddWithValue("@location", locationOfCustomerFactory);
                warehouseIDQueryCommand.Parameters.AddWithValue("@factoryID", factoryID);
                connection.Open();
                using (SqlDataReader readerW = warehouseIDQueryCommand.ExecuteReader())
                {
                    if (readerW.Read())
                    {
                        warehouseID = readerW["warehouse_id"].ToString();
                    }
                }
            }

            //Checking the quantity in INVENTORY 
            string query = "SELECT quantity FROM Inventory WHERE product_id = @productID AND warehouse_id = @warehouseID";
            using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@productID", productID);
                command.Parameters.AddWithValue("@warehouseID", warehouseID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    object obj = reader.GetValue(0);
                    quantityInInventory = (int)obj;
                }
            }

            //Get the PRODUCT NAME using product ID
            string productName = string.Empty;

            string retrieveProductNameQuery = "SELECT product_name FROM Products WHERE product_id = @productID";

            using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
            {
                SqlCommand retrieveProductNameCommand = new SqlCommand(retrieveProductNameQuery, connection);
                retrieveProductNameCommand.Parameters.AddWithValue("@productID", productID);
                connection.Open();
                using (SqlDataReader reader = retrieveProductNameCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        productName = reader["product_name"].ToString();
                    }
                }
            }

            //If quantity SUFFICEINT, order. Else, INFORM customer and manufacturer
            if (quantityInInventory >= quantity)
            {
                // Sufficient quantity in inventory, update the Inventory table
                int newQuantity = quantityInInventory - quantity;
                string updateInventoryQuery = "UPDATE Inventory SET quantity = @newQuantity, last_updated = GETDATE() WHERE product_id = @productID AND warehouse_id = @warehouseID";
                //Updating INVENTORY
                using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
                {
                    SqlCommand updateCommand = new SqlCommand(updateInventoryQuery, connection);
                    updateCommand.Parameters.AddWithValue("@newQuantity", newQuantity);
                    updateCommand.Parameters.AddWithValue("@productID", productID);
                    updateCommand.Parameters.AddWithValue("@warehouseID", warehouseID);
                    connection.Open();
                    updateCommand.ExecuteNonQuery();
                }

                int latestOrderID = 0;
                string queryyy = "SELECT TOP 1 order_id FROM Orders ORDER BY order_id DESC";
                using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
                {
                    SqlCommand commanddd = new SqlCommand(queryyy, connection);
                    connection.Open();
                    SqlDataReader readerrr = commanddd.ExecuteReader();
                    if (readerrr.Read())
                    {
                        object objj = readerrr.GetValue(0);
                        latestOrderID = (int)objj;
                        latestOrderID++;
                    }
                }

                //Inserting into ORDERS
                string insertOrdersQuery = "INSERT INTO Orders (order_id, quantity, product_id, customer_id, order_date) VALUES (@latestOrderID, @quantity, @productID, @customerID, GETDATE())";
                using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
                {
                    SqlCommand insertCommand = new SqlCommand(insertOrdersQuery, connection);
                    insertCommand.Parameters.AddWithValue("@quantity", quantity);
                    insertCommand.Parameters.AddWithValue("@productID", productID);
                    insertCommand.Parameters.AddWithValue("@customerID", customerID);
                    insertCommand.Parameters.AddWithValue("@latestOrderID", latestOrderID);
                    connection.Open();
                    insertCommand.ExecuteNonQuery();
                }
                MessageBox.Show("Order placed successfully");
            }
            else
            {
                // Insufficient quantity in inventory
                MessageBox.Show("Not enough quantity available in inventory. The manufacturer will be informed about this");

                int newQuantity = quantity - quantityInInventory;
                string message = $"A customer placed an order for {quantity} {productName.TrimEnd()} from your warehouse in {locationOfCustomerFactory.TrimEnd()} but the avaialbe quantity was only {quantityInInventory}! So you need to manufacture {newQuantity} of it more and send it to your warehouse in {locationOfCustomerFactory.TrimEnd()}";
                string updateMessageQuery = "UPDATE ManufacturingFactories SET message = @message WHERE factory_id = @factoryID";

                using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
                {
                    SqlCommand updateMessageCommand = new SqlCommand(updateMessageQuery, connection);
                    updateMessageCommand.Parameters.AddWithValue("@message", message);
                    updateMessageCommand.Parameters.AddWithValue("@factoryID", factoryID);
                    connection.Open();
                    updateMessageCommand.ExecuteNonQuery();
                }
            }
        }
    

        private void quantity_Load(object sender, EventArgs e)
        {

        }

    }
}
