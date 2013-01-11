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
    public partial class FormEtap : Form
    {
        public FormEtap(int _patId)
        {
            InitializeComponent();
              if (_patId > 0)
            
              {
                
                  this.searchPatientBox1.searchById(_patId);
              
              }
        }

        public void writeStep()
        {
            try
            {
                Processing.Treatment.TreatmentStep ts = new Processing.Treatment.TreatmentStep();
                ts.Doctor = DBExchange.Inst.dbUsrId;
                ts.EffectStart = dateTimePicker12.Value;
                ts.EffectStop = dateTimePicker13.Value;
                ts.LineStart = dateTimePicker10.Value;
                ts.LineStop = dateTimePicker11.Value;
                ts.LineName = textBox8.Text;
                ts.ShortComment = textBox10.Text;
                ts.LongComment = richTextBox3.Text;
                ts.Patient = searchPatientBox1.pIdN;

                Processing.Treatment.TreatmentStepDataProcessing tsdp = new Processing.Treatment.TreatmentStepDataProcessing();
                tsdp.writeTreatmentStep(ts);
            }

            catch (Exception exception)
            {
                Warnings.WarnLog log = new Warnings.WarnLog();
                log.writeLog(MethodBase.GetCurrentMethod().Name, exception.Message.ToString(), exception.StackTrace.ToString());
            }


        }

        private void button15_Click(object sender, EventArgs e)
        {
            writeStep();
            
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void записатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            writeStep();
            this.Close();
        }

    }
}
