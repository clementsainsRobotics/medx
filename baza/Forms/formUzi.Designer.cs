namespace baza
{
    partial class formUzi
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.закрытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.просмотретьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.распечататьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.записатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxTemplate = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.searchPatientBox1 = new baza.SearchPatientBox();
            this.comboBoxZone = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.searchReestrBox1 = new baza.Forms.SearchReestrBox();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Номер";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(88, 54);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(131, 20);
            this.textBox1.TabIndex = 24;
            // 
            // закрытьToolStripMenuItem
            // 
            this.закрытьToolStripMenuItem.Name = "закрытьToolStripMenuItem";
            this.закрытьToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.закрытьToolStripMenuItem.Text = "Закрыть";
            this.закрытьToolStripMenuItem.Click += new System.EventHandler(this.закрытьToolStripMenuItem_Click);
            // 
            // просмотретьToolStripMenuItem
            // 
            this.просмотретьToolStripMenuItem.Name = "просмотретьToolStripMenuItem";
            this.просмотретьToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.просмотретьToolStripMenuItem.Text = "Просмотреть";
            this.просмотретьToolStripMenuItem.Click += new System.EventHandler(this.просмотретьToolStripMenuItem_Click);
            // 
            // распечататьToolStripMenuItem
            // 
            this.распечататьToolStripMenuItem.Name = "распечататьToolStripMenuItem";
            this.распечататьToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.распечататьToolStripMenuItem.Text = "Распечатать";
            this.распечататьToolStripMenuItem.Click += new System.EventHandler(this.распечататьToolStripMenuItem_Click);
            // 
            // записатьToolStripMenuItem
            // 
            this.записатьToolStripMenuItem.Name = "записатьToolStripMenuItem";
            this.записатьToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.записатьToolStripMenuItem.Text = "Записать";
            this.записатьToolStripMenuItem.Click += new System.EventHandler(this.записатьToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.записатьToolStripMenuItem,
            this.просмотретьToolStripMenuItem,
            this.распечататьToolStripMenuItem,
            this.закрытьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(559, 24);
            this.menuStrip1.TabIndex = 28;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // textBox2
            // 
            this.textBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
            this.textBox2.Location = new System.Drawing.Point(1, 164);
            this.textBox2.MaxLength = 100;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(541, 20);
            this.textBox2.TabIndex = 31;
            this.textBox2.Text = "Краткое описание";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Location = new System.Drawing.Point(0, 190);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(559, 251);
            this.richTextBox1.TabIndex = 29;
            this.richTextBox1.Text = "Полное описание";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Шаблон";
            // 
            // comboBoxTemplate
            // 
            this.comboBoxTemplate.FormattingEnabled = true;
            this.comboBoxTemplate.Location = new System.Drawing.Point(88, 108);
            this.comboBoxTemplate.Name = "comboBoxTemplate";
            this.comboBoxTemplate.Size = new System.Drawing.Size(366, 21);
            this.comboBoxTemplate.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Пациент";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(241, 54);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(128, 20);
            this.dateTimePicker1.TabIndex = 38;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(402, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 39);
            this.button1.TabIndex = 39;
            this.button1.Text = "Добавить текст шаблона";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // searchPatientBox1
            // 
            this.searchPatientBox1.AccessibleDescription = "поиск пациентов";
            this.searchPatientBox1.AccessibleName = "SePaBox";
            this.searchPatientBox1.Location = new System.Drawing.Point(88, 27);
            this.searchPatientBox1.Name = "searchPatientBox1";
            this.searchPatientBox1.Size = new System.Drawing.Size(471, 21);
            this.searchPatientBox1.TabIndex = 22;
            // 
            // comboBoxZone
            // 
            this.comboBoxZone.FormattingEnabled = true;
            this.comboBoxZone.Location = new System.Drawing.Point(88, 81);
            this.comboBoxZone.Name = "comboBoxZone";
            this.comboBoxZone.Size = new System.Drawing.Size(281, 21);
            this.comboBoxZone.TabIndex = 40;
            this.comboBoxZone.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "Зона";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(461, 112);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(51, 17);
            this.checkBox1.TabIndex = 42;
            this.checkBox1.Text = "Свои";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // searchReestrBox1
            // 
            this.searchReestrBox1.Location = new System.Drawing.Point(88, 136);
            this.searchReestrBox1.Name = "searchReestrBox1";
            this.searchReestrBox1.Size = new System.Drawing.Size(443, 22);
            this.searchReestrBox1.TabIndex = 43;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "Реестр услуг";
            // 
            // formUzi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 441);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.searchReestrBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxZone);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.comboBoxTemplate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchPatientBox1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "formUzi";
            this.Text = "УЗИ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private SearchPatientBox searchPatientBox1;
        private System.Windows.Forms.ToolStripMenuItem закрытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem просмотретьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem распечататьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem записатьToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxTemplate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBoxZone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
        private Forms.SearchReestrBox searchReestrBox1;
        private System.Windows.Forms.Label label4;
    }
}