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
    public partial class Store : Form
    {

       
        string searchText { get; set; }
        //string query { get; set; }
        string username { get; set; }
        string list_by { get; set; }
        TextBox textBox { get; set; }
        MySqlConnection myConnection { get; set; }
        MySqlCommand commandDatabase { get; set; }
        MySqlDataReader reader { get; set;  }
        TableLayoutPanel storetable { get; set; }

        public Store()
        {
            InitializeComponent();
            string myConnString = "datasource=127.0.0.1;port=3307;username=root;password=password;database=musicstore;";
            storetable = table;
            textBox = this.searchBox;
            myConnection = new MySqlConnection(myConnString);
          
           

            executeSearch("");


            this.SortByBox.SelectedIndexChanged += new System.EventHandler(SortbyBox_SelectedIndex);

        }
        private void SortbyBox_SelectedIndex(Object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            list_by = (string)box.SelectedItem;
           
            
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(this.searchBox.Text);
            searchText = this.searchBox.Text;
            
            executeSearch(searchText);
           
        }
        public void executeSearch(string SearchText)
        {

            string query = "select  i.ItemName from item as i, category as c, category as p where(p.CategoryName like '%" + SearchText + "%'  or c.CategoryName like '%" + SearchText + "%' ) and p.CategoryID = c.ParentID and c.CategoryID = i.CategoryID ;";
            myConnection.Open();
            commandDatabase = new MySqlCommand(query, myConnection);
            commandDatabase.CommandTimeout = 60;
            reader = commandDatabase.ExecuteReader();
            int numrows = 0;
            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    numrows++;
                }
                textBox1.Text = numrows.ToString() + " items listed    Sort By:";
                table.Controls.Clear();
   
                table.RowCount = numrows;
                table.ColumnCount = 2;
                int val = 1; 
                while (reader.Read())
                {
                    for(int i=0; i < table.RowCount; i++)
                    {
                        TextBox tabletext = new TextBox();
                        tabletext.Text = val.ToString();
                        val++;
                        table.Controls.Add(tabletext, 0, i);
                        tabletext.Text = val.ToString();
                        table.Controls.Add(tabletext, 1, i);
                    }
                }
                
            }
            myConnection.Close();

        }

        /*   try
  {
      myConnection.Open();
      reader = commandDatabase.ExecuteReader();

      if (reader.HasRows)
      {
          while(reader.Read())
          {
              textBox1.Text += reader.GetString(0) + Environment.NewLine;
          }

      }
      else { Console.WriteLine("no rows found"); }
      myConnection.Close();
  }
  catch (Exception e)
  {
      MessageBox.Show(e.Message);
  }*/




    }
}
