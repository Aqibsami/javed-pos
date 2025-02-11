using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace javed_pos
{
    public partial class Form1 : Form
    {
        // Path to your SQLite database file
        private string dbPath = "Data Source=C:\\Users\\Aqibsami\\source\\repos\\your_database.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT name FROM sqlite_master WHERE type='table' AND name='Users';";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            MessageBox.Show("Database connection successful and Users table exists!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Users table does not exist in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lblMessage_Click(object sender, EventArgs e)
        {
        }

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle the PasswordChar property based on the CheckBox state
            txtPassword.PasswordChar = login_showPass.Checked ? '\0' : '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Check if username and password are not empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Query the database to check if the user exists
            string query = "SELECT Role FROM Users WHERE Username = @username AND Password = @password";

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string role = result.ToString();

                            // Redirect to the appropriate form based on the role
                            if (role == "Admin")
                            {
                                AdminForm adminForm = new AdminForm();
                                adminForm.Show();
                                this.Hide(); // Hide the login form
                            }
                            else if (role == "Staff")
                            {
                                StaffForm staffForm = new StaffForm();
                                staffForm.Show();
                                this.Hide(); // Hide the login form
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("SQLite error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
