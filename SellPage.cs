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
        MySqlConnection myConnection { get; set; }
        MySqlCommand commandDatabase { get; set; }
        MySqlDataReader reader { get; set; }
        public SellPage(string name, int id)
        {
            InitializeComponent();
            string myConnString = "datasource=127.0.0.1;port=3307;username=root;password=password;database=musicstore;";
            myConnection = new MySqlConnection(myConnString);
            //call a funciton to populate the comboboxes
            username = name;
            userID = id;
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


    }
   
}
