namespace baza.Forms
{
    partial class FormSetDiagnosis
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
            this.comboBoxDiagSubGroup = new System.Windows.Forms.ComboBox();
            this.label54 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBoxDiagGroup = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBoxDiagDescr = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox8 = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.searchPatientBox1 = new baza.SearchPatientBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxDiagSubGroup
            // 
            this.comboBoxDiagSubGroup.FormattingEnabled = true;
            this.comboBoxDiagSubGroup.Location = new System.Drawing.Point(79, 96);
            this.comboBoxDiagSubGroup.Name = "comboBoxDiagSubGroup";
            this.comboBoxDiagSubGroup.Size = new System.Drawing.Size(607, 21);
            this.comboBoxDiagSubGroup.TabIndex = 55;
            this.comboBoxDiagSubGroup.SelectedIndexChanged += new System.EventHandler(this.comboBoxDiagSubGroup_SelectedIndexChanged);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(9, 99);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(42, 13);
            this.label54.TabIndex = 54;
            this.label54.Text = "Группа";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(208, 344);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(76, 17);
            this.checkBox1.TabIndex = 52;
            this.checkBox1.Text = "Основной";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(555, 69);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(131, 20);
            this.dateTimePicker1.TabIndex = 51;
            // 
            // comboBoxDiagGroup
            // 
            this.comboBoxDiagGroup.FormattingEnabled = true;
            this.comboBoxDiagGroup.Location = new System.Drawing.Point(79, 69);
            this.comboBoxDiagGroup.Name = "comboBoxDiagGroup";
            this.comboBoxDiagGroup.Size = new System.Drawing.Size(352, 21);
            this.comboBoxDiagGroup.TabIndex = 50;
            this.comboBoxDiagGroup.SelectedIndexChanged += new System.EventHandler(this.comboBoxDiagGroup_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 49;
            this.label8.Text = "Код МКБ10";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(290, 340);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(145, 23);
            this.button2.TabIndex = 48;
            this.button2.Text = "Утвердить диагноз";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(468, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Дата диагноза";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(618, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "Id Диагноза";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(471, 42);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(216, 21);
            this.comboBox1.TabIndex = 43;
            // 
            // comboBoxDiagDescr
            // 
            this.comboBoxDiagDescr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxDiagDescr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxDiagDescr.FormattingEnabled = true;
            this.comboBoxDiagDescr.Location = new System.Drawing.Point(79, 123);
            this.comboBoxDiagDescr.Name = "comboBoxDiagDescr";
            this.comboBoxDiagDescr.Size = new System.Drawing.Size(607, 21);
            this.comboBoxDiagDescr.TabIndex = 42;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 41;
            this.label9.Text = "Диагноз";
            // 
            // comboBox8
            // 
            this.comboBox8.FormattingEnabled = true;
            this.comboBox8.Items.AddRange(new object[] {
            "Диагноз направившего учреждения",
            "Диагноз при поступлении/обращении",
            "Клинический диагноз",
            "Уточненный клинический диагноз",
            "Патологоанатомический диагноз",
            "Сопутствующий диагноз",
            "Причина смерти"});
            this.comboBox8.Location = new System.Drawing.Point(188, 42);
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Size = new System.Drawing.Size(243, 21);
            this.comboBox8.TabIndex = 40;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 45);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(157, 13);
            this.label17.TabIndex = 39;
            this.label17.Text = "Текущая категория диагноза";
            // 
            // searchPatientBox1
            // 
            this.searchPatientBox1.AccessibleDescription = "поиск пациентов";
            this.searchPatientBox1.AccessibleName = "SePaBox";
            this.searchPatientBox1.Location = new System.Drawing.Point(79, 14);
            this.searchPatientBox1.Name = "searchPatientBox1";
            this.searchPatientBox1.Size = new System.Drawing.Size(428, 22);
            this.searchPatientBox1.TabIndex = 56;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(578, 340);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 23);
            this.button1.TabIndex = 57;
            this.button1.Text = "Закрыть форму";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 150);
            this.textBox1.MaxLength = 512;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(674, 184);
            this.textBox1.TabIndex = 45;
            this.textBox1.Text = "Описание";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 340);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 23);
            this.button3.TabIndex = 58;
            this.button3.Text = "Удалить";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormSetDiagnosis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 375);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.searchPatientBox1);
            this.Controls.Add(this.comboBoxDiagSubGroup);
            this.Controls.Add(this.label54);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.comboBoxDiagGroup);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.comboBoxDiagDescr);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBox8);
            this.Controls.Add(this.label17);
            this.Name = "FormSetDiagnosis";
            this.Text = "Диагноз пациента";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxDiagSubGroup;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comboBoxDiagGroup;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBoxDiagDescr;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox8;
        private System.Windows.Forms.Label label17;
        private SearchPatientBox searchPatientBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
    }
}