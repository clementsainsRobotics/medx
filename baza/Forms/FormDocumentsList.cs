using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using System.Reflection;


namespace baza.Forms
{
    public partial class FormDocumentsList : Form
    {
        private Documentation.DocumentsList documentList;

        public event EventHandler<FormParentListEventArgs> ParentDocumentsList;

        public FormDocumentsList(bool _patient, int _patientId, DateTime _dateFrom, DateTime _dateTo)
        {
            InitializeComponent();
            if (_patient == true)
            {
                loadDataForPatient(_patientId, _dateFrom, _dateTo);
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }

        public void loadDataForPatient(int _pid, DateTime _dateFrom, DateTime _dateTo)
        {
            documentList = new Documentation.DocumentsList();
            documentList.GetPatientDocumentsByDate(_pid, _dateFrom, _dateTo);
            foreach (Documentation.Document i in documentList)
            {
                checkedListBox1.Items.Add(i.Serial+" "+ i.DocumentShortName +" "+ i.DocumentHeader + " " + i.Date);
            }

        }

        private void createListSelected()
        {

            try
            {
                Documentation.DocumentsList dList = new Documentation.DocumentsList();
            if (checkedListBox1.SelectedItems.Count > 0)
                {
                               
                foreach (int ic in this.checkedListBox1.CheckedIndices)
                    {
                         dList.Add(documentList[ic] );
                //    checkedListBox1.SetItemCheckState(ic, CheckState.Unchecked);
                    }
                                
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Выберите документы");
                }

            EventHandler<FormParentListEventArgs> handler = this.ParentDocumentsList;

            if (handler != null)
            {
                handler(this, new FormParentListEventArgs (dList));
            }
                   
            }
                    
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
            }     
            

    }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                createListSelected();
                ActiveForm.Close();
            }
            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
            }        
        }


    }





    public class FormParentListEventArgs : EventArgs
    {

        private readonly Documentation.DocumentsList _docList = null;
        public FormParentListEventArgs(Documentation.DocumentsList DocList)
        {
            _docList = DocList;
        }

        public Documentation.DocumentsList DocList
        {
            get
            {
                return _docList;
            }
        }
        

    }

}
