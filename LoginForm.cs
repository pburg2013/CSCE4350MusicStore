using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MusicStore
{
    public partial class LoginForm : Form
    {
        string username { get; set; }
        string password { get; set; }
        string fname { get; set; }
        string query { get; set; }
        string lname { get; set; }
        MySqlConnection myConnection { get; set; }
        MySqlCommand commandDatabase { get; set; }
        MySqlDataReader reader { get; set; }

        public LoginForm()
        {
            InitializeComponent();
            string myConnString = "datasource=127.0.0.1;port=3307;username=root;password=password;database=musicstore;";
            //starting the connection
            myConnection = new MySqlConnection(myConnString);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //getting the text from the text boxes
            username = this.usernameBox.Text;
            password = this.passwordBox.Text;
            query = "select UserID from user where Username = '"+username+"' and Password = '"+password+ "' ;";

            //open the connection
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            reader = commandDatabase.ExecuteReader();
            if (reader.HasRows)
            {
                //user name and password is correct
                reader.Read();
                replyBox.Text = "Welcome back: " + username + "! ";
                SellPage sellpage = new SellPage(username, reader.GetInt32(0));
                sellpage.FormClosed += new FormClosedEventHandler(sellpage_closed);
                this.Hide();
                sellpage.Show();
                

            }
            else
                replyBox.Text = "Invalid username or password try again!";

            myConnection.Close();

        }
        private void sellpage_closed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fname = this.fnameBox.Text;
            lname = this.lnameBox.Text;
            username = this.unameBox.Text;
            password = this.passBox.Text;

            //first test to see if the username already exists
            query = "select * from user where Username = '" + username + "';";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            reader = commandDatabase.ExecuteReader();
            if (reader.HasRows)
            {
                replyBox.Text = "Username already taken";
                myConnection.Close();

            }else
            {
                myConnection.Close();
                query = "INSERT INTO USER (Username, Password, FirstName, LastName) VALUES ( @username,@password,@fname,@lname );";
               
                try
                {
                    myConnection.Open();
                    using (commandDatabase = new MySqlCommand(query, myConnection))
                    {
                        commandDatabase.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
                        commandDatabase.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;
                        commandDatabase.Parameters.Add("@fname", MySqlDbType.VarChar).Value = fname;
                        commandDatabase.Parameters.Add("@lname", MySqlDbType.VarChar).Value = lname;
                        int x = commandDatabase.ExecuteNonQuery();
                        if (x > 0)
                        {
                            replyBox.Text = "You have successfully signed up! ";
                        }
                        else
                        {
                            replyBox.Text = "Sign up not successful..";
                        }


                    }
                
                }catch(Exception ex)
                {
                    MessageBox.Show("error" + ex.Message);
                }


                myConnection.Close();
            }
          

        }
    }
}
