using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace baza.Forms
{
    public partial class FormSimpleInputForm : Form
    {

        public event EventHandler<FormParentEventArgs> ParentText;

        public FormSimpleInputForm()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeNewClass(this.richTextBox1.Text.Trim());
            ActiveForm.Close();
        }
        private void writeNewClass(string _name)
        {
            _name = _name.Replace("\n", "<br>");
            EventHandler<FormParentEventArgs> handler = this.ParentText;

            if (handler != null)
            {
                handler(this, new FormParentEventArgs(_name));
            }

        }



    }
    public class FormParentEventArgs : EventArgs
    {

        private readonly string _className = "";
        


        public FormParentEventArgs(string ClassName)
        {
            _className = ClassName;
            
        }



        public string ClassName
        {

            get
            {
                return _className;
            }

        }



    }
}

