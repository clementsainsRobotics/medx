using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace baza.SpecialControls
{
    public partial class BodyRichTextBox : UserControl
    {
        public RichTextBox thisText;
        public string Text;
        public BodyRichTextBox()
        {
            InitializeComponent();
            thisText = this.richTextBox1;
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            string gotText = this.richTextBox1.Text.Trim();
            if (gotText == "Полное описание")
            {

                this.richTextBox1.Text = "";
                this.richTextBox1.Refresh();
            }    
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            Text = this.richTextBox1.Text.Trim();
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            Text = this.richTextBox1.Text.Trim();
        }
    }
}
