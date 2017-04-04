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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ShopButton_Click(object sender, EventArgs e)
        {
            Store newStore = new Store();
            newStore.StartPosition = FormStartPosition.WindowsDefaultBounds;
            newStore.FormClosed += new FormClosedEventHandler(form_closed);
            newStore.Show();
            this.Hide();
        }

        private void SellButton_Click(object sender, EventArgs e)
        {
            LoginForm loginform = new LoginForm();
            loginform.FormClosed += new FormClosedEventHandler(form_closed);
            loginform.Show();
            this.Hide();
        }
        private void form_closed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
