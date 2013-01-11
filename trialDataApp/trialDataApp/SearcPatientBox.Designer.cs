namespace trialDataApp
{
    partial class SearchPatientBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.searchPatientComboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // searchPatientComboBox1
            // 
            this.searchPatientComboBox1.AccessibleName = "sePaCBox";
            this.searchPatientComboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.searchPatientComboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.searchPatientComboBox1.FormattingEnabled = true;
            this.searchPatientComboBox1.Location = new System.Drawing.Point(0, 0);
            this.searchPatientComboBox1.Name = "searchPatientComboBox1";
            this.searchPatientComboBox1.Size = new System.Drawing.Size(370, 21);
            this.searchPatientComboBox1.TabIndex = 0;
            this.searchPatientComboBox1.SelectedIndexChanged += new System.EventHandler(this.searchPatientComboBox1_SelectedIndexChanged);
            this.searchPatientComboBox1.Enter += new System.EventHandler(this.searchPatientComboBox1_Enter);
            this.searchPatientComboBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchPatientComboBox1_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(376, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Возраст";
            // 
            // SearchPatientBox
            // 
            this.AccessibleDescription = "поиск пациентов";
            this.AccessibleName = "SePaBox";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchPatientComboBox1);
            this.Name = "SearchPatientBox";
            this.Size = new System.Drawing.Size(428, 22);
            this.DoubleClick += new System.EventHandler(this.SearchPatientBox_DoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox searchPatientComboBox1;
        private System.Windows.Forms.Label label1;


    }
}
