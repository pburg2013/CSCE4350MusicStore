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
        List<int> itemIDs = new List<int>();
        MySqlConnection myConnection { get; set; }
        MySqlCommand commandDatabase { get; set; }
        MySqlDataReader reader { get; set; }

        public LoginForm(List<int> items)
        {
            itemIDs = items; 
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
            query = "select UserID from user where Username = '"+username+"' and Password = md5('"+password+ "') ;";

            //open the connection
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            reader = commandDatabase.ExecuteReader();
            if (reader.HasRows)
            {
                //user name and password is correct
                reader.Read();
                replyBox.Text = "Welcome back: " + username + "! ";
                //  SellPage sellpage = new SellPage(username, reader.GetInt32(0));
                Account userpage = new Account(username, reader.GetInt32(0), itemIDs);
                userpage.FormClosed += new FormClosedEventHandler(sellpage_closed);
                this.Hide();
                userpage.Show();
                
                
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
                query = "INSERT INTO USER (Username, Password, FirstName, LastName) VALUES ( '"+username+"',md5('"+password+"'),'"+fname+"','"+lname+"'); ";
               
                try
                {
                    myConnection.Open();
                    using (commandDatabase = new MySqlCommand(query, myConnection))
                    {
                       
                        int x = commandDatabase.ExecuteNonQuery();
                        if (x > 0)
                        {
                            replyBox.Text = "You have successfully signed up! You can login now!";
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
