using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace baza
{
    public partial class printFormGist : Form
    {
        public printFormGist(string webPr, bool printData, bool landS)
        {
            InitializeComponent();
            
            this.webBrowser1.DocumentText = webPr;
            this.webBrowser1.Refresh();
            if (printData == true && landS == true)
            {
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(printLandS);
            }
            else if (printData == true && landS == false)
            {
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(PrintNow);
               
            }
            else 
            {}

            
        }

        public void PrintNow(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            ((WebBrowser)sender).Print();
            ((WebBrowser)sender).Dispose();

        }

        public void printLandS(object sender, WebBrowserDocumentCompletedEventArgs e)
        {


        ((WebBrowser)sender).ShowPageSetupDialog() ;
        ((WebBrowser)sender).ShowPrintPreviewDialog ();
        ((WebBrowser)sender).Dispose();

        }


        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
