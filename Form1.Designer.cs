namespace MusicStore
{
    partial class Store
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.SortByBox = new System.Windows.Forms.ComboBox();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // table
            // 
            this.table.AutoScroll = true;
            this.table.ColumnCount = 2;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.07975F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.92024F));
            this.table.Location = new System.Drawing.Point(61, 87);
            this.table.Name = "table";
            this.table.RowCount = 2;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table.Size = new System.Drawing.Size(652, 228);
            this.table.TabIndex = 0;
            // 
            // SortByBox
            // 
            this.SortByBox.FormattingEnabled = true;
            this.SortByBox.Items.AddRange(new object[] {
            "Popularity",
            "Price"});
            this.SortByBox.Location = new System.Drawing.Point(621, 60);
            this.SortByBox.Name = "SortByBox";
            this.SortByBox.Size = new System.Drawing.Size(92, 21);
            this.SortByBox.TabIndex = 1;
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(61, 61);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(158, 20);
            this.searchBox.TabIndex = 2;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(226, 60);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(462, 61);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(153, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "Sort By:";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Store
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 336);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.SortByBox);
            this.Controls.Add(this.table);
            this.Name = "Store";
            this.Text = "Shop";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel table;
        private System.Windows.Forms.ComboBox SortByBox;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox textBox1;
    }
}

