using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace baza.SpecialControls
{
    public partial class HeaderTextBox : UserControl
    {
        public TextBox thisText;
        public string Text;
        public HeaderTextBox()
        {
            InitializeComponent();
            thisText = this.textBox1;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            string gotText = this.textBox1.Text.Trim();
            if (gotText == "Краткое описание")
            {

                this.textBox1.Text = "";
                this.textBox1.Refresh();
            }    
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Text = this.textBox1.Text.Trim();
        }

        private void textBox1_Leave(object sender, KeyEventArgs e)
        {
            Text = this.textBox1.Text.Trim();
        }
    }
}
