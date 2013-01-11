namespace baza.Forms
{
    partial class FormDrugTreatment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDrugTreatment));
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.dateTimePicker14 = new System.Windows.Forms.DateTimePicker();
            this.label67 = new System.Windows.Forms.Label();
            this.comboBoxDrugDose = new System.Windows.Forms.ComboBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxDrugs = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxDrugGroupList = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.searchPatientBox1 = new baza.SearchPatientBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(333, 173);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(100, 20);
            this.textBox11.TabIndex = 72;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(254, 177);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(60, 13);
            this.label28.TabIndex = 71;
            this.label28.Text = "Кратность";
            // 
            // dateTimePicker14
            // 
            this.dateTimePicker14.Location = new System.Drawing.Point(114, 226);
            this.dateTimePicker14.Name = "dateTimePicker14";
            this.dateTimePicker14.Size = new System.Drawing.Size(154, 20);
            this.dateTimePicker14.TabIndex = 70;
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(9, 232);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(89, 13);
            this.label67.TabIndex = 69;
            this.label67.Text = "Дата окончания";
            // 
            // comboBoxDrugDose
            // 
            this.comboBoxDrugDose.FormattingEnabled = true;
            this.comboBoxDrugDose.Location = new System.Drawing.Point(169, 173);
            this.comboBoxDrugDose.Name = "comboBoxDrugDose";
            this.comboBoxDrugDose.Size = new System.Drawing.Size(51, 21);
            this.comboBoxDrugDose.TabIndex = 68;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(72, 174);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(43, 20);
            this.textBox4.TabIndex = 65;
            this.textBox4.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 64;
            this.label5.Text = "Доза";
            // 
            // comboBoxDrugs
            // 
            this.comboBoxDrugs.FormattingEnabled = true;
            this.comboBoxDrugs.Location = new System.Drawing.Point(72, 147);
            this.comboBoxDrugs.Name = "comboBoxDrugs";
            this.comboBoxDrugs.Size = new System.Drawing.Size(459, 21);
            this.comboBoxDrugs.TabIndex = 63;
            this.comboBoxDrugs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxDrugs_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 62;
            this.label4.Text = "Препарат";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(376, 222);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(154, 23);
            this.button4.TabIndex = 61;
            this.button4.Text = "Подтвердить";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(72, 65);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(459, 21);
            this.comboBox3.TabIndex = 60;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(114, 200);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(154, 20);
            this.dateTimePicker2.TabIndex = 59;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(10, 68);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(51, 13);
            this.label30.TabIndex = 58;
            this.label30.Text = "Диагноз";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(9, 206);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(71, 13);
            this.label31.TabIndex = 56;
            this.label31.Text = "Дата начала";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(557, 25);
            this.toolStrip1.TabIndex = 76;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(61, 22);
            this.toolStripButton1.Text = "Записать";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(57, 22);
            this.toolStripButton2.Text = "Закрыть";
            this.toolStripButton2.ToolTipText = "CloseButton";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 77;
            this.label1.Text = "ед изм";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 78;
            this.label2.Text = "Группа";
            // 
            // comboBoxDrugGroupList
            // 
            this.comboBoxDrugGroupList.FormattingEnabled = true;
            this.comboBoxDrugGroupList.Location = new System.Drawing.Point(72, 92);
            this.comboBoxDrugGroupList.Name = "comboBoxDrugGroupList";
            this.comboBoxDrugGroupList.Size = new System.Drawing.Size(459, 21);
            this.comboBoxDrugGroupList.TabIndex = 79;
            this.comboBoxDrugGroupList.SelectedIndexChanged += new System.EventHandler(this.comboBoxDrugGroupList_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(154, 120);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(376, 20);
            this.textBox1.TabIndex = 80;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 81;
            this.label3.Text = "Поиск по названию";
            // 
            // searchPatientBox1
            // 
            this.searchPatientBox1.AccessibleDescription = "поиск пациентов";
            this.searchPatientBox1.AccessibleName = "SePaBox";
            this.searchPatientBox1.Location = new System.Drawing.Point(72, 37);
            this.searchPatientBox1.Name = "searchPatientBox1";
            this.searchPatientBox1.Size = new System.Drawing.Size(473, 22);
            this.searchPatientBox1.TabIndex = 75;
            this.searchPatientBox1.setPatientSelected += new baza.SearchPatientBox.setNewPatientSelected(this.searchPatientBox1_setPatientSelected);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 282);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(557, 22);
            this.statusStrip1.TabIndex = 82;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(93, 17);
            this.toolStripStatusLabel1.Text = "введите данные";
            // 
            // FormDrugTreatment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 304);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBoxDrugGroupList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.searchPatientBox1);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.dateTimePicker14);
            this.Controls.Add(this.label67);
            this.Controls.Add(this.comboBoxDrugDose);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxDrugs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label31);
            this.Name = "FormDrugTreatment";
            this.Text = "Лекарственное лечение";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.DateTimePicker dateTimePicker14;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.ComboBox comboBoxDrugDose;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxDrugs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private SearchPatientBox searchPatientBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxDrugGroupList;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}