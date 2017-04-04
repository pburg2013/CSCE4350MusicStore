using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class Account : Form
    {
        string uname { get; set; }
        int userId { get; set; }
        public Account(string username, int id)
        {
            InitializeComponent();
            uname = username;
            userId = id;
            textBox1.Text += username;
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
    }
}
