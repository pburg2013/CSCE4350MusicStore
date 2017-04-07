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
    public partial class Account : Form
    {
        string uname { get; set; }
        int userId { get; set; }
        List<int> itemIDs = new List<int>();
        List<int> orderIDs = new List<int>();
        List<string> items = new List<string>();
        List<int> itemsChecked = new List<int>();
        string query { get; set; }
        TextBox list { get; set; }
        MySqlConnection myConnection { get; set; }
        MySqlCommand commandDatabase { get; set; }
        MySqlDataReader reader { get; set; }
       

        public Account(string username, int id, List<int> BuyingItems )
        {
            InitializeComponent();
            uname = username;
            userId = id;
            textBox1.Text += username + "!" ;
            label6.Text += BuyingItems.Count();
            itemsChecked = BuyingItems;
            list = textBox4;
            string myConnString = "datasource=127.0.0.1;port=3307;username=root;password=password;database=musicstore;";
            myConnection = new MySqlConnection(myConnString);
            getItemsOrdered();
            getPendingItems();
        }

        private void sell_button_Click(object sender, EventArgs e)
        {
            //creating a new sell page
            SellPage sellpage = new SellPage(uname, userId);
            sellpage.FormClosed += new FormClosedEventHandler(form_closed);
            sellpage.Show();
            this.Hide();


        }
        private void form_closed(Object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
        public void getItemsOrdered()
        {
            list.Text = "";
            //get all the items listed
            query = "select i.ItemName, m.StatusName from item as i, order_detail as d, order_master as w, order_status as m where w.PurchaserID = " + userId + " and d.OrderStatusID = m.OrderStatusID and d.ItemID = i.ItemID and d.OrderID = w.OrderID; ";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Text += reader.GetString(0) + ":    " + reader.GetString(1) + Environment.NewLine;
                }
            }
            myConnection.Close();
        }
        public void getPendingItems()
        {

            itemIDs.Clear();
            items.Clear();
            orderIDs.Clear();
            checkedListBox1.Items.Clear();
            
            //getting all items that need to be shipped
            query = " select i.ItemName,p.FirstName, p.LastName,  m.ShippingAddress, d.Quantity, i.ItemID, d.OrderID from item as i,user as p, order_detail as d, order_master as m where i.SellerID = " + userId + " and d.OrderID = m.OrderID and d.OrderStatusID = 1 and i.ItemID = d.ItemID and m.PurchaserID = p.UserID;";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //in the flow layout panel create a check box that will indicate the item, quantity and address. 
                    //If the checkbox is clicked that means that the item was shipped
                    checkedListBox1.Items.Add(reader.GetString(0) + ": " + reader.GetString(1)+ " "+ reader.GetString(2) + " : " + reader.GetString(3) + " : " + reader.GetInt32(4));
                    itemIDs.Add(reader.GetInt32(5));
                    orderIDs.Add(reader.GetInt32(6));

                    items.Add(reader.GetString(0) + ": " + reader.GetString(1) + " " + reader.GetString(2) + " : " + reader.GetString(3) + " : " + reader.GetInt32(4));

                }
            }
            myConnection.Close();
            

        }
       
       

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // MessageBox.Show("Event changed");
            CheckedListBox box = (CheckedListBox)sender;
            string prod = (string)box.SelectedItem;
            int index = 0;
            for (int i = 0; i < items.Count(); i++)
            {
                if (items[i] == prod)
                {
                    index = i;
                }
            }
            //update the database, set that order detail to "shipped" status
            query = "set SQL_SAFE_UPDATES = 0; update order_detail set OrderStatusID = 2 where itemID = " + itemIDs[index] + " and OrderID = " + orderIDs[index] + " ; SET SQL_SAFE_UPDATES = 1; ";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                if (commandDatabase.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Item " + itemIDs[index] + " for order number : " + orderIDs[index] + " has been updated!");
                } 
            }catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //create a shop form giving the initials and list as an argument
            Store newstore = new Store(uname, itemsChecked);
            newstore.FormClosed += new FormClosedEventHandler(form_closed);
            newstore.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //create a Main form
            MainForm main = new MainForm();
            main.FormClosed += new FormClosedEventHandler(form_closed);
            main.Show();
            this.Hide();
        }

        private void checkout_button_Click(object sender, EventArgs e)
        {
            //create checkout form
            Checkout checkoutpage = new Checkout(uname, itemsChecked);
            checkoutpage.FormClosed += new FormClosedEventHandler(form_closed);
            checkoutpage.Show();
            this.Hide();
        }
    }
}
