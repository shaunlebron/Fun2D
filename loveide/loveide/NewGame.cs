using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace loveide
{
    public partial class NewGame : Form
    {
        public NewGame()
        {
            InitializeComponent();
            designForm();

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        public string GameName { get; set; }

        public string NameField
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }

        public List<string> UsedGameNames { get; set; }

        private void designForm()
        {
            this.BackColor = Color.FromArgb(233, 242, 248);
            lblDesc.ForeColor = Color.FromArgb(0x6ba1c5);
            lblDesc.Text = "Name your Game:";
            lblDesc.Font = new Font("Arial", 24, FontStyle.Bold, GraphicsUnit.Pixel);
            txtName.Font = new Font("Arial", 18, FontStyle.Regular, GraphicsUnit.Pixel);
            lblDesc.Left = 10;
            txtName.Left = txtName.Left;
            txtName.Top = lblDesc.Bottom + 10;
            txtName.Width = lblDesc.Width;
            btnOk.Height = btnCancel.Height = txtName.Height;
            btnOk.Top = txtName.Top;
            btnOk.Left = txtName.Right + 10;
            btnCancel.Left = btnOk.Left;
            btnCancel.Top = btnOk.Bottom + 10;
        }

        string removeBadChar(string filename)
        {
            // Replace invalid characters with "_" char.
            return Regex.Replace(filename, @"[^\w\.-]", "_");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            GameName = removeBadChar(NameField);
            foreach (var s in UsedGameNames)
                if (s.ToLower() == GameName.ToLower())
                {
                    MessageBox.Show("Name is already taken.  Please choose another!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
