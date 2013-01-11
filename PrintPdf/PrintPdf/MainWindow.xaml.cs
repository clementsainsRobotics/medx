using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System.Web;
using System.IO;


namespace PrintPdf
{
////////////////////////////////   

    public partial class MainWindow
    {

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            thisRead("pk.pdf");

        }

        public void thisRead(string formFile)
        {
            string newFile = "new_print.pdf";
           // FontFactory.Register("c:\\windows\\fonts\\arial.ttf");
            PdfReader reader = new PdfReader(formFile);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(newFile, FileMode.Create));
            AcroFields fields = stamper.AcroFields;

            fields.SetField("NumericField1", "121");
            fields.SetField("TextField2", "there");
            stamper.FormFlattening = false;
            stamper.Close();
            reader.Close();
        }

        private void ListFieldNames()
        {

            // create a new PDF reader based on the PDF template document

            PdfReader pdfReader = new PdfReader("po.pdf");
            // create and populate a string builder with each of the 
            // field names available in the subject PDF

            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry de in pdfReader.AcroFields.Fields)
            {
                sb.Append(de.Key.ToString() + Environment.NewLine);
            }
            // Write the string builder's content to the form's textbox

            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = 0;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ListFieldNames();
        }

    }



public class newReadPdf 
{




}

////////////////////////////////
}

