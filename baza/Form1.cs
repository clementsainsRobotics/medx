using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace baza

{
    public partial class Form1 : Form
    {
        DALCPatients mDALCPatients = new DALCPatients();
        public Form1()
        {
            InitializeComponent();
        }

        private void bdConnectionButton_Click(object sender, EventArgs e)
        {
            mDALCPatients.RefreshData();
            dbNameTextBox.Text = mDALCPatients.Patients.Count.ToString();

            dataGridView1.Columns.Add("Id", "Id");
            dataGridView1.Columns.Add("Name", "Name");
            foreach (Patient patientIt in mDALCPatients.Patients)
            {
                dataGridView1.Rows.Add(
                    new object[2] { patientIt.Id.ToString(), patientIt.Name });
            }
            //
        }

        private void StartupOnClickEvent(object sender, EventArgs e)
        {
            StartupForm FormStartup = new StartupForm();
            FormStartup.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dbNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
