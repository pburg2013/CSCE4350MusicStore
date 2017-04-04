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
    public partial class SellPage : Form
    {
        List<string> qualityNames = new List<string>();//will store all the qualityNames
        List<int> qualityIDs = new List<int>(); // will store all the qualityIDs
        List<string> qualityDescriptions = new List<string>();//will store all the quality descriptions
        List<string> ParentCategories = new List<string>();//will store all the parent category names
        List<string> subCategories = new List<string>(); //will hold the sub category names
        List<int> categoryIDs = new List<int>();//will store all the category IDs
        string query { get; set; }
        string username { get; set; }
        int userID { get; set; }
        string prodName { get; set; }
        string prodQuality { get; set; }
        string prodDescription { get; set; }
        string prodCat { get; set; }
        string prodSubCat { get; set; }
        double prodPrice { get; set; }
        int prodQuantity { get; set; }
        MySqlConnection myConnection { get; set; }
        MySqlCommand commandDatabase { get; set; }
        MySqlDataReader reader { get; set; }
        public SellPage(string name, int userid)
        {
            InitializeComponent();
            string myConnString = "datasource=127.0.0.1;port=3307;username=root;password=password;database=musicstore;";
            myConnection = new MySqlConnection(myConnString);
            //call a funciton to populate the comboboxes
            username = name;
            userID = userid;
            populateList();
            populateBoxes();






        }
        public void populateList()
        {
            //get the quality names and IDs set up
            qualityNames.Clear();
            qualityIDs.Clear();
            qualityDescriptions.Clear();
            ParentCategories.Clear();
            subCategories.Clear();
            categoryIDs.Clear();
            int num_parent = 0;
            int num_sub = 0;
            query = "select QualityName, QualityID,Description from quality;";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    qualityNames.Add(reader.GetString(0));
                    qualityIDs.Add(reader.GetInt32(1));
                    qualityDescriptions.Add(reader.GetString(2));

                }
            }
            myConnection.Close();

            //get all parent ids
            query = "select CategoryName, CategoryID from category where isnull(ParentID);";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ParentCategories.Add(reader.GetString(0));
                    categoryIDs.Add(reader.GetInt32(1));
                    num_parent++;
                }
            }
            myConnection.Close();

            query = "select CategoryName, CategoryID from category where ParentID >=0;";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    subCategories.Add(reader.GetString(0));
                    categoryIDs.Add(reader.GetInt32(1));
                    num_sub++;
                }
            }
            myConnection.Close();


        }
        public void populateBoxes()
        {
            //run through the list of qualities and add them to the quality box
            foreach (string q in qualityNames)
            {
                QualityBox.Items.Add(q);
            }

            //run through the list of parent categories and add them to the quality box
            foreach (string s in ParentCategories)
            {
                CategoryBox.Items.Add(s);

            }

            //add the list of subcategories
            foreach(string s in subCategories)
            {
                SubCategoryBox.Items.Add(s);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //user has added a new product to sell in the store

            //getting the strings and saving them 

            prodName = NameBox.Text;
            prodPrice = Convert.ToDouble(PriceBox.Text);
            prodQuantity = Convert.ToInt32(QuantityBox.Text);
            prodDescription = DescBox.Text;
            int CatID = get_categoryID();
            int QID = get_qualityID();
            //add item to the items table
            int itemID = add_item(CatID, QID);


            //add the item in the inventory table

            query = "INSERT INTO TABLE inventory (ItemID, Quantity) VALUES (@itemID, @prodQuantity); ";
            try
            {
                myConnection.Open();

                using (commandDatabase = new MySqlCommand(query, myConnection))
                {
                    commandDatabase.Parameters.Add("@itemID", MySqlDbType.Int32).Value = itemID;
                    commandDatabase.Parameters.Add("@prodQuantity", MySqlDbType.Int32).Value = prodQuantity;
                 
                    int x = commandDatabase.ExecuteNonQuery();
                    if (x > 0)
                    {
                        textBox2.Text = "Product added to store inventory!";
                    }
                    else
                    {
                        textBox2.Text = "Product unable to be added to inventory";
                    }


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex.Message);
            }


            myConnection.Close();

        }

        private void QualityBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox qbox = (ComboBox)sender;
            prodQuality = (string) qbox.SelectedItem;
            int index = 0; 
            for(int i =0; i < qualityNames.Count(); i++){
                if (qualityNames[i] == prodQuality)
                    index = i;
            }
            QualityTipText.Text = prodQuality + ": " + qualityDescriptions[index];
        }

        private void CategoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbox = (ComboBox)sender;
            prodCat = (string)cbox.SelectedItem;

        }

        private void SubCategoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbox = (ComboBox)sender;
            prodSubCat = (string)cbox.SelectedItem;
        }
        public int get_categoryID()
        {
            query = "select i.CategoryID from category as i, category as g where i.CategoryName like '%" + prodSubCat + "%' and g.CategoryName like '%" + prodCat + "%' and i.ParentID = g.CategoryID";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();
            int catID = -1;

            //if the reader has rows then save and return the id
            if (reader.HasRows)
            {
                reader.Read();
                catID = reader.GetInt32(0);

            }
            myConnection.Close();
            return catID;



        }
        public int add_item(int CatID, int QualID)
        {
            query = "INSERT INTO item (ItemName, Description, SellerID, CategoryID,QualityID,Price) VALUES ( @prodName,@prodDescription,@userID,@CatID, @QualID,@prodPrice );";
            try
            {
                myConnection.Open();
                using (commandDatabase = new MySqlCommand(query, myConnection))
                {
                    commandDatabase.Parameters.Add("@prodName", MySqlDbType.VarChar).Value = prodName;
                    commandDatabase.Parameters.Add("@prodDescription", MySqlDbType.VarChar).Value = prodDescription;
                    commandDatabase.Parameters.Add("@userID", MySqlDbType.Int32).Value = userID;
                    commandDatabase.Parameters.Add("@CatID", MySqlDbType.Int32).Value = CatID;
                    commandDatabase.Parameters.Add("@QualID", MySqlDbType.Int32).Value = QualID;
                    commandDatabase.Parameters.Add("@prodPrice", MySqlDbType.Decimal).Value = prodPrice;
                    int x = commandDatabase.ExecuteNonQuery();
                    if (x > 0)
                    {
                        MessageBox.Show( "Product created! Now adding to Inventory");
                    }
                    else
                    {
                        textBox2.Text = "Product creation unsuccessful..";
                    }


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex.Message);
            }



            myConnection.Close();
            //get and return the new items ID
            query = "select ItemID from item where ItemName like '%" + prodName + "%' and SellerID = " + userID + " ;";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();
            int itemID = -1;

            //if the reader has rows then save and return the id
            if (reader.HasRows)
            {
                reader.Read();
                itemID = reader.GetInt32(0);

            }
            myConnection.Close();
            return itemID;

        }
   
        public int get_qualityID()
        {
            query = "select qualityID from quality where qualityName = '%" + prodQuality + "%'";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();
            int QID = -1;

            //if the reader has rows then save and return the id
            if (reader.HasRows)
            {
                reader.Read();
                QID = reader.GetInt32(0);

            }
            myConnection.Close();
            return QID;
        }
    }
   
}
