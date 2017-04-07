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
    public partial class Checkout : Form
    {
        string username { get; set; }
        string query { get; set; }
        int userID { get; set; }
        List<float> itemPrices = new List<float>();

        List<int> itemIDsChecked = new List<int>(); //hold the itemid for every id that is checked
        List<int> itemQuantities = new List<int>();
        List<int> itemCheckedQuantities = new List<int>();
        List<float> prices = new List<float>();
        List<string> itemNames = new List<string>();
        MySqlConnection myConnection { get; set; }
        MySqlCommand commandDatabase { get; set; }
        MySqlDataReader reader { get; set; }
        public Checkout(string user, List<int> itemChecked)
        {
            string myConnString = "datasource=127.0.0.1;port=3307;username=root;password=password;database=musicstore;";
            myConnection = new MySqlConnection(myConnString);
            itemIDsChecked.Clear();
            itemIDsChecked = itemChecked;
            InitializeComponent();
            list_items();
            get_total();
            username = user;
        }
        public void list_items()
        {
            
            int numrows = 0;
           
            //creating the table and adding items to it
            //will display the item, drop down box to choose quantity and price for that
            tableLayoutPanel1.SuspendLayout();
            itemNames.Clear();
            foreach (int x in itemIDsChecked)
            {
                //get all the item IDs and their quanities in the inventory
                query = "select i.ItemName, m.Quantity, i.Price from item as i, inventory as m where i.ItemID = m.ItemID and m.ItemID = " + x + ";";
                myConnection.Open();
                commandDatabase = new MySqlCommand(query, myConnection);
                commandDatabase.CommandTimeout = 60;
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    numrows++;
                    reader.Read();
                    itemNames.Add(reader.GetString(0));
                    itemQuantities.Add(reader.GetInt32(1));
                    itemPrices.Add(reader.GetFloat(2));
                }
                else
                {
                    itemNames.Add("Out of stock item!");
                    itemQuantities.Add(0);
                    itemPrices.Add((float) 0.00);
                 
                }
                myConnection.Close();
            }
            //add the information to the table layout
            textBox1.Text += numrows.ToString();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowCount = numrows+1;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.AutoSize = true;

            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            //add the labels

            Label NameLabel = new Label();
            NameLabel.AutoSize = true;
            NameLabel.Text = "Item Name";
            tableLayoutPanel1.Controls.Add(NameLabel, 0, 0);

            Label DesLable = new Label();
            DesLable.AutoSize = true;
            DesLable.Text = "Quantity";
            tableLayoutPanel1.Controls.Add(DesLable, 1, 0);

            Label PriceLabel = new Label();
            PriceLabel.AutoSize = true;
            PriceLabel.Text = "Price";
            tableLayoutPanel1.Controls.Add(PriceLabel, 2, 0);

            //add the information to the table

            for(int i=1; i< tableLayoutPanel1.RowCount; i++)
            {
                TextBox name = new TextBox();
                name.Text = itemNames[i - 1];
                name.ReadOnly = true;
                name.TabStop = false;
                Size size = TextRenderer.MeasureText(name.Text, name.Font);
                name.Width = size.Width;
                name.BackColor = Color.LightGray;
                name.BorderStyle = BorderStyle.None;
                name.Height = size.Height;
                // name.Multiline = true;
                tableLayoutPanel1.Controls.Add(name, 0, i);

                NumericUpDown quantity = new NumericUpDown();
                if (itemQuantities[i - 1] == 0)
                    quantity.Value = 0;
                else
                    quantity.Value = 1;
                itemCheckedQuantities.Add((int)quantity.Value);
                quantity.Name = itemNames[i - 1];
                quantity.Minimum = 0;
                quantity.Maximum = itemQuantities[i - 1];
                quantity.AutoSize = true;
                quantity.ValueChanged += new EventHandler(value_changed);
                tableLayoutPanel1.Controls.Add(quantity, 1, i);


                TextBox desc = new TextBox();
                desc.ReadOnly = true;
                desc.TabStop = false;
                desc.Name = itemNames[i - 1] + "Text";
                desc.Text = ((int)quantity.Value * itemPrices[i - 1]).ToString();
                size = TextRenderer.MeasureText(desc.Text, desc.Font);
                desc.BackColor = Color.LightGray;
                desc.BorderStyle = BorderStyle.None;
                desc.Width = size.Width;
                desc.Height = size.Height;
                //  desc.Multiline = true;
                tableLayoutPanel1.Controls.Add(desc, 2, i);

               



            }
            tableLayoutPanel1.ResumeLayout();


        }private void value_changed(Object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            float price; 
            for(int i =0;i < itemNames.Count(); i++)
            {
                if(itemNames[i] == num.Name)
                {
                    price = itemPrices[i] * (int)num.Value;
                    itemCheckedQuantities[i] = (int)num.Value;
                    foreach(Control c in tableLayoutPanel1.Controls)
                    {
                        if(c.Name ==( itemNames[i] + "Text"))
                        {
                            c.Text = price.ToString();
                        }
                    }
                }
            }
            get_total();
            
        }
        public void get_total()
        {
            
            //add up all the prices 
            for(int i =1; i < tableLayoutPanel1.RowCount; i++)
            {
                Control c = tableLayoutPanel1.GetControlFromPosition(2, i);
                prices.Add(float.Parse(c.Text));


            }
            float total = 0;
            foreach (float x in prices)
                total += x;
            PriceBox.Text =  total.ToString();
            TotalBox.Text =  (total + 5.00).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //create a new order in the database
            //first get the user ID given the username
            string address = textBox3.Text;
            float total = float.Parse(TotalBox.Text);
             userID=-1;
            string date = DateTime.Now.ToString("yyyy-MM-dd") ;

            query = " select UserID from user where Username = '" + username + "';";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                userID = reader.GetInt32(0);
            }
            else
            {
                MessageBox.Show("Invalid user name");
            }

            myConnection.Close();

            //create the order
            query = "insert into order_master (PurchaserID, ShippingAddress, OrderDate, Total,Payment) values ("+userID+", '"+address+"', '"+date+"' , "+total+" ,md5('" + textBox4.Text+"') ) ;";

            myConnection.Open();
            using (commandDatabase = new MySqlCommand(query, myConnection))
            {

                int x = commandDatabase.ExecuteNonQuery();
                if (x > 0)
                {
                    MessageBox.Show("Order has been placed! ");
                }
                else
                {
                    MessageBox.Show("Error with order");
                }


            }
            myConnection.Close();

            //add the order detail for all items
            string message = ""; 
            int orderID=1;
            query = "select orderID from order_master where Total = " + total + " and PurchaserID = " + userID + "; ";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
               orderID = reader.GetInt32(0);
            }
            else
            {
                MessageBox.Show("Unable to find order");
            }
            myConnection.Close();
            
            //for all items in the list, create an row in the order detail
            for(int i=0; i < itemIDsChecked.Count(); i++)
            {
                if(orderID  > 0)
                 query = "insert into order_detail (OrderID, ItemID, OrderStatusID, Quantity, Total) values (" + orderID + ", " + itemIDsChecked[i] + ", 1, " + itemCheckedQuantities[i] + ", " + total + ");";
                myConnection.Open();
                using (commandDatabase = new MySqlCommand(query, myConnection))
                {

                    int x = commandDatabase.ExecuteNonQuery();
                    if (x > 0)
                    {
                        message += "Item: " + itemNames[i] + " has been ordered!" + Environment.NewLine;
                    }
                    else
                    {
                        MessageBox.Show("Error with order");
                    }


                }
                myConnection.Close();

            }
            //after checkout go to account page with nothing in your cart
            MessageBox.Show(message);
            Account page = new Account(username, userID, new List<int>());
            page.FormClosed += new FormClosedEventHandler(form_closed);
            page.Show();
            this.Hide();

        }
        private void form_closed(Object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
