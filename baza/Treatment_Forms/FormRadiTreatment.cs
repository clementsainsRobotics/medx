using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace baza.Forms
{
    public partial class FormRadioTreatment : Form
    {
        public FormRadioTreatment(int _patId)
        {
            InitializeComponent();
            if (_patId > 0)
            {
                searchPatientBox1.searchById(_patId);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormRadioTreatment.ActiveForm.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            writeThis();
        }

        private void writeThis()
        {
            Processing.Treatment.RadiologyTreatmentData rtd = new Processing.Treatment.RadiologyTreatmentData();
            rtd.PatId = searchPatientBox1.pIdN;
            rtd.RayStart = dateTimePicker7.Value;
            rtd.RayStop = dateTimePicker15.Value;
            rtd.Sod = Convert.ToDecimal( textBox14.Text.ToString());
            rtd.Rod = Convert.ToDecimal( textBox6.Text.ToString());
            Processing.Treatment.RadiologyTreatmentProcessData rtpd = new Processing.Treatment.RadiologyTreatmentProcessData();
            rtpd.writeData(rtd);

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            writeThis();
        }
    }
}
