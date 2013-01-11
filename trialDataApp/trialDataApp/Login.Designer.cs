namespace trialDataApp
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.DbServer = new System.Windows.Forms.ComboBox();
            this.DbLogin = new System.Windows.Forms.ComboBox();
            this.DbPass = new System.Windows.Forms.MaskedTextBox();
            this.Server = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DbOpenButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // DbServer
            // 
            this.DbServer.AccessibleDescription = "Выберите сервер для подключения";
            this.DbServer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.DbServer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.DbServer.FormattingEnabled = true;
            this.DbServer.Location = new System.Drawing.Point(90, 45);
            this.DbServer.Name = "DbServer";
            this.DbServer.Size = new System.Drawing.Size(331, 21);
            this.DbServer.TabIndex = 0;
            // 
            // DbLogin
            // 
            this.DbLogin.AccessibleDescription = "Введите имя пользователя";
            this.DbLogin.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.DbLogin.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.DbLogin.FormattingEnabled = true;
            this.DbLogin.Location = new System.Drawing.Point(90, 90);
            this.DbLogin.Name = "DbLogin";
            this.DbLogin.Size = new System.Drawing.Size(197, 21);
            this.DbLogin.TabIndex = 1;
            // 
            // DbPass
            // 
            this.DbPass.AccessibleDescription = "Введите пароль";
            this.DbPass.Location = new System.Drawing.Point(90, 135);
            this.DbPass.Name = "DbPass";
            this.DbPass.PasswordChar = '*';
            this.DbPass.Size = new System.Drawing.Size(116, 20);
            this.DbPass.TabIndex = 2;
            this.DbPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DbPass_KeyDown);
            // 
            // Server
            // 
            this.Server.AutoSize = true;
            this.Server.Location = new System.Drawing.Point(24, 48);
            this.Server.Name = "Server";
            this.Server.Size = new System.Drawing.Size(44, 13);
            this.Server.TabIndex = 3;
            this.Server.Text = "Сервер";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Пароль";
            // 
            // DbOpenButton
            // 
            this.DbOpenButton.Location = new System.Drawing.Point(27, 198);
            this.DbOpenButton.Name = "DbOpenButton";
            this.DbOpenButton.Size = new System.Drawing.Size(197, 23);
            this.DbOpenButton.TabIndex = 7;
            this.DbOpenButton.Text = "Войти";
            this.DbOpenButton.UseVisualStyleBackColor = true;
            this.DbOpenButton.Click += new System.EventHandler(this.DbOpen);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(243, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(178, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Закрыть";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = global::trialDataApp.Properties.Resources.shar;
            this.pictureBox1.Location = new System.Drawing.Point(357, 90);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 65);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 245);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DbOpenButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Server);
            this.Controls.Add(this.DbPass);
            this.Controls.Add(this.DbLogin);
            this.Controls.Add(this.DbServer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.Text = "Соединение с сервером Мед Экс v ";
            this.Text +=  DBExchange.Inst.versionNumber;
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox DbServer;
        private System.Windows.Forms.ComboBox DbLogin;
        private System.Windows.Forms.MaskedTextBox DbPass;
        private System.Windows.Forms.Label Server;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DbOpenButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}