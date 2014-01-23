using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace loveide
{
    public partial class RenameForm : Form
    {
        char[] invalidChars;

        public RenameForm()
        {
            InitializeComponent();
            invalidChars = Path.GetInvalidFileNameChars();
        }

        public string NewName
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        bool valid()
        {
            foreach (char c in invalidChars)
            {
                if (NewName.Contains(c))
                {
                    MessageBox.Show("Please do not use \"" + c + "\" in the name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (valid())
                this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
