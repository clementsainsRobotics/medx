using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Reflection;
using System.Collections;

namespace baza.Forms
{
    public partial class FormTreatmentBase : Form
    {
        private Processing.Treatment.TreatmentStepDataProcessing tsdp;
        private Processing.Treatment.TreatmentStepsList tsList;
        private Processing.DrugsClass.PatientDrugTreatment patientDrugTreatmentList;

        private int _patId;

        public FormTreatmentBase(int _thisPatient)
        {
            
            InitializeComponent();

            if (_thisPatient > 0)
            {
                _patId = _thisPatient;
                this.searchPatientBox1.searchById(_patId);
                loadTreatmStepList();
                loadDrugTreatmentData();
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
           
        }

        private void loadTreatmStepList()
        {
            listBox2.Items.Clear();
            tsList = new Processing.Treatment.TreatmentStepsList();
            tsList.getTreatmentStepByPatientId(_patId);
            foreach (Processing.Treatment.TreatmentStep ts in tsList)
            {
                this.listBox2.Items.Add(ts.LineName);
            }
            listBox2.Refresh();
        }

        private void resizeFirstTab()
        {
            //if (richTextBox1.Height > 120)
            //{
            //    splitContainer2.SplitterDistance = splitContainer2.SplitterDistance + richTextBox1.Height - 100;
            //}
            //else
            //{
            //    splitContainer2.SplitterDistance = splitContainer1.Height / 2;
            //}
            //this.tabControl2.Refresh();
        }

        private void vScrollBar1_MouseHover(object sender, EventArgs e)
        {
            resizeFirstTab();
        }



        private void button1_Click(object sender, EventArgs e)
        {
          //  writeStep();

            loadTreatmStepList();
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadTreatmentStep(listBox2.SelectedIndex);
        }

        private void loadTreatmentStep(int _thisStep)
        {

        }

        private void loadSurgeryTreatmentList()
        {


        }

        private void loadDrugTreatmentData()
        {
            listBox3.Items.Clear();
            patientDrugTreatmentList = new Processing.DrugsClass.PatientDrugTreatment();
            patientDrugTreatmentList.getPatientDrugs(searchPatientBox1.pIdN);

            foreach (Processing.DrugsClass.DrugTreatment i in patientDrugTreatmentList)
            {
                listBox3.Items.Add(i.WrittenDate + " " + i.drugName + " " + i.drugDose + " " + i.drugDoseName + " " + i.doctorName);

            }
            listBox3.Refresh();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            FormDrugTreatment fdt = new FormDrugTreatment(_patId);
            fdt.ShowDialog();
            loadDrugTreatmentData();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FormEtap fe = new FormEtap(_patId);
            fe.ShowDialog();

        }


    }
}
