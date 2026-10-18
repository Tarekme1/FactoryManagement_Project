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
    public partial class CustomerLoginForm : Form
    {
        private SqlConnection connection;

        public CustomerLoginForm()
        {
            InitializeComponent();
            connection = new SqlConnection(DatabaseSettings.ConnectionString);
        }

        private void LoginToYourAccount_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Click(object sender, EventArgs e)
        {
            string username = usernametextbox.Text;
            string password = passwordtextbox.Text;
            string customerId = "";

            if (ValidateLogin(username, password))
            {
                // Query the database for the customerId
                string query = "SELECT cus_factory_id FROM CustomerAccounts WHERE cus_username=@username AND cus_password=@password";
                using (SqlConnection connection = new SqlConnection(DatabaseSettings.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        customerId = reader["cus_factory_id"].ToString();
                    }
                    reader.Close();
                }

                // Open the customer form and pass the customerId
                CustomerPage customerPage = new CustomerPage();
                customerPage.stdName = customerId;
                customerPage.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            // Query the database to check if the username and password are valid
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM CustomerAccounts WHERE cus_username = @username AND cus_password = @password", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            connection.Open();
            int result = (int)command.ExecuteScalar();
            connection.Close();

            return result == 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartingForm startingForm = new StartingForm();
            startingForm.Show();
            this.Hide();
        }

        private void CustomerLoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
