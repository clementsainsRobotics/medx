namespace baza.Forms
{
    partial class FormTicket
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
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.comboBox11 = new System.Windows.Forms.ComboBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.comboBox9 = new System.Windows.Forms.ComboBox();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.button7 = new System.Windows.Forms.Button();
            this.listBoxMyPatientTickets = new System.Windows.Forms.ListBox();
            this.searchPatientBox1 = new baza.SearchPatientBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(513, 44);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(34, 17);
            this.checkBox4.TabIndex = 15;
            this.checkBox4.Text = "Л";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // comboBox11
            // 
            this.comboBox11.FormattingEnabled = true;
            this.comboBox11.Location = new System.Drawing.Point(12, 67);
            this.comboBox11.Name = "comboBox11";
            this.comboBox11.Size = new System.Drawing.Size(44, 21);
            this.comboBox11.TabIndex = 14;
            this.comboBox11.Text = "Гр";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(473, 44);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(33, 17);
            this.checkBox3.TabIndex = 13;
            this.checkBox3.Text = "К";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(434, 44);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(33, 17);
            this.checkBox2.TabIndex = 12;
            this.checkBox2.Text = "В";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // comboBox9
            // 
            this.comboBox9.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox9.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox9.FormattingEnabled = true;
            this.comboBox9.Location = new System.Drawing.Point(62, 67);
            this.comboBox9.Name = "comboBox9";
            this.comboBox9.Size = new System.Drawing.Size(644, 21);
            this.comboBox9.TabIndex = 11;
            this.comboBox9.Text = "Услуга";
            // 
            // comboBox7
            // 
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Location = new System.Drawing.Point(12, 40);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(416, 21);
            this.comboBox7.TabIndex = 10;
            this.comboBox7.Text = "Отделение";
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.Location = new System.Drawing.Point(556, 41);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.Size = new System.Drawing.Size(150, 20);
            this.dateTimePicker4.TabIndex = 9;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(473, 10);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(113, 23);
            this.button7.TabIndex = 8;
            this.button7.Text = "Назначить";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // listBoxMyPatientTickets
            // 
            this.listBoxMyPatientTickets.FormattingEnabled = true;
            this.listBoxMyPatientTickets.Location = new System.Drawing.Point(13, 95);
            this.listBoxMyPatientTickets.Name = "listBoxMyPatientTickets";
            this.listBoxMyPatientTickets.Size = new System.Drawing.Size(693, 199);
            this.listBoxMyPatientTickets.TabIndex = 17;
            // 
            // searchPatientBox1
            // 
            this.searchPatientBox1.AccessibleDescription = "поиск пациентов";
            this.searchPatientBox1.AccessibleName = "SePaBox";
            this.searchPatientBox1.Location = new System.Drawing.Point(12, 12);
            this.searchPatientBox1.Name = "searchPatientBox1";
            this.searchPatientBox1.Size = new System.Drawing.Size(428, 22);
            this.searchPatientBox1.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(604, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "Закрыть";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 300);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBoxMyPatientTickets);
            this.Controls.Add(this.searchPatientBox1);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.comboBox11);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.comboBox9);
            this.Controls.Add(this.comboBox7);
            this.Controls.Add(this.dateTimePicker4);
            this.Controls.Add(this.button7);
            this.Name = "FormTicket";
            this.Text = "Назначение";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.ComboBox comboBox11;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ComboBox comboBox9;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
        private System.Windows.Forms.Button button7;
        private SearchPatientBox searchPatientBox1;
        private System.Windows.Forms.ListBox listBoxMyPatientTickets;
        private System.Windows.Forms.Button button1;
    }
}