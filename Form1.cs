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

        List<string> itemNames = new List<string>(); //hold all the items name
        List<string> itemDecriptions = new List<string>();//hold all the item descriptions
        List<string> itemPrices = new List<string>(); //hold all the items prices
        List<string> itemQuality = new List<string>(); //hold all the items qualities
        string searchText { get; set; }
        //string query { get; set; }
        string username { get; set; }
        string list_by { get; set; }
        TextBox textBox { get; set; }
        MySqlConnection myConnection { get; set; }
        MySqlCommand commandDatabase { get; set; }
        MySqlDataReader reader { get; set;  }
      

        public Store()
        {
            InitializeComponent();
            string myConnString = "datasource=127.0.0.1;port=3307;username=root;password=password;database=musicstore;";
        
            textBox = this.searchBox;
            myConnection = new MySqlConnection(myConnString);
            this.DoubleBuffered = true;
            

            searchText = "";
            list_by = "";

            executeSearch(searchText, list_by);


            this.SortByBox.SelectedIndexChanged += new System.EventHandler(SortbyBox_SelectedIndex);

        }
        private void SortbyBox_SelectedIndex(Object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            list_by = (string)box.SelectedItem;
            executeSearch(searchText, list_by);
           
            
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(this.searchBox.Text);
            searchText = this.searchBox.Text;
            
            executeSearch(searchText,list_by);
           
        }
        public void executeSearch(string SearchText, string orderBy)
        {
            string query;
            if (orderBy == "Price")
                query = "select  i.ItemName, i.Description, i.Price, q.QualityName from item as i, category as c, category as p, quality as q  where(p.CategoryName like '%" + SearchText + "%'  or c.CategoryName like '%" + SearchText + "%' ) and p.CategoryID = c.ParentID and c.CategoryID = i.CategoryID and q.QualityID = i.QualityID order by Price;";
            else if (orderBy == "Popularity")
                query = "select i.ItemName, i.Description, i.Price, q.QualityName from item as i, category as c, category as p, quality as q,  order_detail as w where(p.CategoryName like '%" + SearchText + "%'  or c.CategoryName like '%" + SearchText + "%' ) and (p.CategoryID = c.ParentID and c.CategoryID = i.CategoryID) and w.ItemID = i.ItemID and q.QualityID = i.QualityID order by w.Quantity desc;";
            else
                query = "select  i.ItemName, i.Description, i.Price, q.QualityName from item as i, category as c, category as p, quality as q where(p.CategoryName like '%" + SearchText + "%'  or c.CategoryName like '%" + SearchText + "%' ) and p.CategoryID = c.ParentID and c.CategoryID = i.CategoryID and q.QualityID = i.QualityID ;";


        
            table.SuspendLayout();
            itemNames.Clear();
            itemDecriptions.Clear();
            itemPrices.Clear();
            itemQuality.Clear();
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
                    //saving the reader data into appropriate list
                    itemNames.Add(reader.GetString(0));
                    itemDecriptions.Add(reader.GetString(1));
                    itemPrices.Add(reader.GetString(2));
                    itemQuality.Add(reader.GetString(3));

                }
            }
            
            textBox1.Text = numrows.ToString() + " items listed    Sort By:";
         
            table.Controls.Clear();
            table.ColumnStyles.Clear();
            table.RowCount = numrows;
            table.ColumnCount = 5;
             
            table.AutoSize = true;
                

              

            for (int i = 0; i < table.RowCount; i++)
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            for (int i = 0; i < table.ColumnCount; i++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            //adding the product lables to the table of products
             
            Label NameLabel = new Label();
            NameLabel.AutoSize = true;
            NameLabel.Text = "Item Name";
            table.Controls.Add(NameLabel, 0, 0);
            Label DesLable = new Label();
            DesLable.AutoSize = true;
            DesLable.Text = "Description";
            table.Controls.Add(DesLable, 1, 0);
            Label PriceLabel = new Label();
            PriceLabel.AutoSize = true;
            PriceLabel.Text = "Price";
            table.Controls.Add(PriceLabel, 2, 0);
            Label QLabel = new Label();
            QLabel.AutoSize = true;
            QLabel.Text = "Quality";
            table.Controls.Add(QLabel, 3, 0);
            Label BuyLabel = new Label();
            BuyLabel.AutoSize = true;
            BuyLabel.Text = "Add to Cart";
            table.Controls.Add(BuyLabel, 4, 0);

            for (int i = 1; i < table.RowCount; i++)
            {
                //all Name, descriptions, prices per item are located in the row index
                TextBox name = new TextBox();
                name.Text = itemNames[i - 1];
                Size size = TextRenderer.MeasureText(name.Text, name.Font);
                name.Width = size.Width;
                name.Height = size.Height;
                // name.Multiline = true;
                table.Controls.Add(name, 0, i);
                TextBox desc = new TextBox();
                desc.Text = itemDecriptions[i - 1];
                size = TextRenderer.MeasureText(desc.Text, desc.Font);
                desc.Width = size.Width;
                desc.Height = size.Height;
                //  desc.Multiline = true;
                table.Controls.Add(desc, 1, i);
                TextBox price = new TextBox();
                price.Text = itemPrices[i - 1];
                size = TextRenderer.MeasureText(price.Text, price.Font);
                price.Width = size.Width;
                price.Height = size.Height;
                table.Controls.Add(price, 2, i);
                TextBox quality = new TextBox();
                quality.Text = itemQuality[i - 1];
                size = TextRenderer.MeasureText(quality.Text, quality.Font);
                quality.Width = size.Width;
                quality.Height = size.Height;
                table.Controls.Add(quality, 3, i);
                CheckBox checkbox = new CheckBox();
                checkbox.Name = (i - 1).ToString();
                table.Controls.Add(checkbox, 4, i);


               
            }
            table.AutoScroll = true;
            table.ResumeLayout();
            table.Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);
                // int val = 1;
               

            
             /*   while (reader.Read())
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
                
            }*/
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
