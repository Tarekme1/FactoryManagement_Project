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
    public partial class ManufacturerLoginForm : Form
    {
        private SqlConnection connection;

        public ManufacturerLoginForm()
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

            if (ValidateLogin(username, password))
            {
                string manufacturer_factory_Id = "";
                using (SqlCommand cmd = new SqlCommand("SELECT factory_id FROM ManufacturerAccounts WHERE man_username = @username AND man_password = @password", connection))
                {
                    cmd.Parameters.AddWithValue("@username", usernametextbox.Text);
                    cmd.Parameters.AddWithValue("@password", passwordtextbox.Text);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            manufacturer_factory_Id = reader.GetString(0);
                        }
                    }
                    connection.Close();
                }

                // Open the main form and hide the login form
                EmployeePage employeePage = new EmployeePage();
                employeePage.stdName = manufacturer_factory_Id;
                employeePage.Show();
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
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM ManufacturerAccounts WHERE man_username = @username AND man_password = @password", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            connection.Open();
            int result = (int)command.ExecuteScalar();
            connection.Close();

            return result == 1;
        }

        private void back_Click(object sender, EventArgs e)
        {
            StartingForm startingForm = new StartingForm();
            startingForm.Show();
            this.Hide();
        }

        private void ManufacturerLoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
