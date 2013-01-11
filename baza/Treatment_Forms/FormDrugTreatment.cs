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
    public partial class FormDrugTreatment : Form
    {

        Processing.Diagnosis.DiagList patientDiags;
        Processing.DrugsClass.DrugGroupList dgroupList;
        Processing.DrugsClass.DrugList drugList;
        Processing.DrugsClass.DrugDoseList drDoseList;

        public FormDrugTreatment(int _patId)
        {
            InitializeComponent();
            if (_patId > 0)
            {
                this.searchPatientBox1.searchById(_patId);                
            }
            getDrugGroupList();
            getMeasureList();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormDrugTreatment.ActiveForm.Close();
        }

        private void getSchemeData()
        {



        }



        private void getEtapDataForDiag()
        {
        }
        private void getDiagsForPatient(int _patientId)
        {
            patientDiags = new Processing.Diagnosis.DiagList();
            patientDiags.GetDiagListForPatient(_patientId);

            this.comboBox3.Items.Clear();
            foreach (Processing.Diagnosis.Diag i in patientDiags)
            {
                this.comboBox3.Items.Add(i.DiagName + " "+i.DiagDate.ToShortDateString());
            }

        
        }
        private void getCycles() { }

        private void getDrugGroupList()
        {
            dgroupList = new Processing.DrugsClass.DrugGroupList();
            dgroupList.getDrugGroupList();
            foreach (Processing.DrugsClass.DrugGroup i in dgroupList)
            {
                comboBoxDrugGroupList.Items.Add(i.groupDescription);
            }

        }

        private void getDrug() 
        {

            comboBoxDrugs.Items.Clear();
            comboBoxDrugs.Text = "";
            foreach (Processing.DrugsClass.Drug i in drugList)
            {
                comboBoxDrugs.Items.Add(i.drugName);
            }
            if (comboBoxDrugs.Items.Count > 0)
            {
                comboBoxDrugs.SelectedIndex = 0;
            }

        }

        private void getMeasureList()
        {
            drDoseList = new Processing.DrugsClass.DrugDoseList();
            drDoseList.getDoseTypes();
            comboBoxDrugDose.Items.Clear();
            foreach (Processing.DrugsClass.DrugDose i in drDoseList)
            {
                comboBoxDrugDose.Items.Add(i.DoseName);
            }
            comboBoxDrugDose.SelectedIndex = 0;
        }


        private void writeDrugTreatment() 
        {
            decimal _dose = Convert.ToDecimal(textBox4.Text);
            decimal _unit = drDoseList[this.comboBoxDrugDose.SelectedIndex].DoseId;
            string _krat = textBox11.Text.Trim();
            int _drug = drugList[comboBoxDrugs.SelectedIndex].drugId;
            int _doc = DBExchange.Inst.dbUsrId;
            int _pat = this.searchPatientBox1.pIdN;
            if (textBox4.Text.Trim().Length > 0)
            {

                Processing.DrugsClass.DrugDataProcessing ddp = new Processing.DrugsClass.DrugDataProcessing();
                try
                {
                    ddp.writeDrugTreatment(_dose, _unit, _krat, dateTimePicker2.Value, this.dateTimePicker14.Value, _pat, _drug, _doc);
                    this.textBox4.Text = "";
                    this.toolStripStatusLabel1.Text = "Записано лекарство " + drDoseList[this.comboBoxDrugDose.SelectedIndex].DoseName + " для пациента " + this.searchPatientBox1.patName;
                }
                catch (Exception exception)
                {
                    Warnings.WarnLog log = new Warnings.WarnLog();
                    log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
                }
            }
           
        }



        private void searchPatientBox1_setPatientSelected(int _selectedPatient)
        {
            getDiagsForPatient(_selectedPatient);
        }

        private void comboBoxDrugGroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            drugList = new Processing.DrugsClass.DrugList();
            drugList.getDrugsByGroupId(dgroupList[comboBoxDrugGroupList.SelectedIndex].groupId );
            getDrug();
        }

        private void comboBoxDrugs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                drugList = new Processing.DrugsClass.DrugList();
                drugList.getDrugsByName(this.comboBoxDrugs.Text.ToLower().Trim());
                getDrug();
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                drugList = new Processing.DrugsClass.DrugList();
                drugList.getDrugsByName(this.textBox1.Text.ToLower().Trim());
                getDrug();
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                writeDrugTreatment();
            }
            finally
            {
                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                writeDrugTreatment();
            }
            finally
            {
                this.Close();
            }
        }



    }
}
