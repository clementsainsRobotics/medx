namespace baza
{
    partial class RegisterForm
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
            this.f_name = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.u_name = new System.Windows.Forms.TextBox();
            this.r_pass = new System.Windows.Forms.MaskedTextBox();
            this.r_pass_2 = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.otdel = new System.Windows.Forms.ComboBox();
            this.status = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.RegisterUser = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.s_name = new System.Windows.Forms.TextBox();
            this.r_user_mail = new System.Windows.Forms.MaskedTextBox();
            this.label_sn = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // f_name
            // 
            this.f_name.Location = new System.Drawing.Point(12, 12);
            this.f_name.Name = "f_name";
            this.f_name.Size = new System.Drawing.Size(197, 20);
            this.f_name.TabIndex = 0;
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(12, 38);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(197, 20);
            this.name.TabIndex = 1;
            // 
            // u_name
            // 
            this.u_name.Location = new System.Drawing.Point(12, 112);
            this.u_name.Name = "u_name";
            this.u_name.Size = new System.Drawing.Size(143, 20);
            this.u_name.TabIndex = 3;
            // 
            // r_pass
            // 
            this.r_pass.Location = new System.Drawing.Point(12, 138);
            this.r_pass.Name = "r_pass";
            this.r_pass.PasswordChar = '*';
            this.r_pass.Size = new System.Drawing.Size(143, 20);
            this.r_pass.TabIndex = 4;
            this.r_pass.UseSystemPasswordChar = true;
            // 
            // r_pass_2
            // 
            this.r_pass_2.Location = new System.Drawing.Point(12, 164);
            this.r_pass_2.Name = "r_pass_2";
            this.r_pass_2.PasswordChar = '*';
            this.r_pass_2.Size = new System.Drawing.Size(143, 20);
            this.r_pass_2.TabIndex = 5;
            this.r_pass_2.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(161, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Пароль не менее 5 знаков";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Повторить пароль";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Логин (только английские 4)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(215, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Фамилия";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(215, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Имя ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(161, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Дата рождения";
            // 
            // otdel
            // 
            this.otdel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.otdel.FormattingEnabled = true;
            this.otdel.Location = new System.Drawing.Point(12, 233);
            this.otdel.Name = "otdel";
            this.otdel.Size = new System.Drawing.Size(350, 21);
            this.otdel.TabIndex = 7;
            // 
            // status
            // 
            this.status.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.status.FormattingEnabled = true;
            this.status.Location = new System.Drawing.Point(12, 260);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(279, 21);
            this.status.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 217);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Отделение";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(297, 263);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Профессия";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(215, 317);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Адрес эл. почты";
            // 
            // RegisterUser
            // 
            this.RegisterUser.Location = new System.Drawing.Point(12, 376);
            this.RegisterUser.Name = "RegisterUser";
            this.RegisterUser.Size = new System.Drawing.Size(117, 23);
            this.RegisterUser.TabIndex = 18;
            this.RegisterUser.Text = "Регистрировать";
            this.RegisterUser.UseVisualStyleBackColor = true;
            this.RegisterUser.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(198, 376);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(164, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "Закрыть окно регистрации";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // s_name
            // 
            this.s_name.Location = new System.Drawing.Point(12, 64);
            this.s_name.Name = "s_name";
            this.s_name.Size = new System.Drawing.Size(197, 20);
            this.s_name.TabIndex = 2;
            // 
            // r_user_mail
            // 
            this.r_user_mail.Location = new System.Drawing.Point(12, 314);
            this.r_user_mail.Name = "r_user_mail";
            this.r_user_mail.Size = new System.Drawing.Size(198, 20);
            this.r_user_mail.TabIndex = 9;
            // 
            // label_sn
            // 
            this.label_sn.AutoSize = true;
            this.label_sn.Location = new System.Drawing.Point(215, 67);
            this.label_sn.Name = "label_sn";
            this.label_sn.Size = new System.Drawing.Size(54, 13);
            this.label_sn.TabIndex = 20;
            this.label_sn.Text = "Отчество";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 191);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(143, 20);
            this.dateTimePicker1.TabIndex = 24;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Врач",
            "Наблюдатель",
            "Администратор"});
            this.comboBox1.Location = new System.Drawing.Point(13, 288);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(196, 21);
            this.comboBox1.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(216, 291);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "Статус";
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 442);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label_sn);
            this.Controls.Add(this.r_user_mail);
            this.Controls.Add(this.s_name);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.RegisterUser);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.status);
            this.Controls.Add(this.otdel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.r_pass_2);
            this.Controls.Add(this.r_pass);
            this.Controls.Add(this.u_name);
            this.Controls.Add(this.name);
            this.Controls.Add(this.f_name);
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Форма регистрации пользователя";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox f_name;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox u_name;
        private System.Windows.Forms.MaskedTextBox r_pass;
        private System.Windows.Forms.MaskedTextBox r_pass_2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox otdel;
        private System.Windows.Forms.ComboBox status;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button RegisterUser;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox s_name;
        private System.Windows.Forms.MaskedTextBox r_user_mail;
        private System.Windows.Forms.Label label_sn;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label10;
    }
}