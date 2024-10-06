namespace WindowsFormsApplication1
{
    partial class Launcher
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.UserSign = new System.Windows.Forms.Button();
            this.LogIn_Button = new System.Windows.Forms.Button();
            this.Password_Label = new System.Windows.Forms.Label();
            this.Password_Box = new System.Windows.Forms.TextBox();
            this.User_Label = new System.Windows.Forms.Label();
            this.User_Box = new System.Windows.Forms.TextBox();
            this.LogOut_Button = new System.Windows.Forms.Button();
            this.LeaderBoard_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(25, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 31);
            this.button1.TabIndex = 4;
            this.button1.Text = "conectar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(25, 332);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(147, 53);
            this.button3.TabIndex = 10;
            this.button3.Text = "desconectar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // UserSign
            // 
            this.UserSign.Location = new System.Drawing.Point(534, 96);
            this.UserSign.Name = "UserSign";
            this.UserSign.Size = new System.Drawing.Size(104, 47);
            this.UserSign.TabIndex = 16;
            this.UserSign.Text = "Sign In";
            this.UserSign.UseVisualStyleBackColor = true;
            this.UserSign.Click += new System.EventHandler(this.UserSign_Click);
            // 
            // LogIn_Button
            // 
            this.LogIn_Button.Location = new System.Drawing.Point(534, 35);
            this.LogIn_Button.Name = "LogIn_Button";
            this.LogIn_Button.Size = new System.Drawing.Size(104, 47);
            this.LogIn_Button.TabIndex = 15;
            this.LogIn_Button.Text = "Log In";
            this.LogIn_Button.UseVisualStyleBackColor = true;
            this.LogIn_Button.Click += new System.EventHandler(this.LogIn_Button_Click);
            // 
            // Password_Label
            // 
            this.Password_Label.AutoSize = true;
            this.Password_Label.Location = new System.Drawing.Point(359, 96);
            this.Password_Label.Name = "Password_Label";
            this.Password_Label.Size = new System.Drawing.Size(53, 13);
            this.Password_Label.TabIndex = 14;
            this.Password_Label.Text = "Password";
            // 
            // Password_Box
            // 
            this.Password_Box.Location = new System.Drawing.Point(362, 119);
            this.Password_Box.Name = "Password_Box";
            this.Password_Box.Size = new System.Drawing.Size(142, 20);
            this.Password_Box.TabIndex = 13;
            // 
            // User_Label
            // 
            this.User_Label.AutoSize = true;
            this.User_Label.Location = new System.Drawing.Point(359, 39);
            this.User_Label.Name = "User_Label";
            this.User_Label.Size = new System.Drawing.Size(29, 13);
            this.User_Label.TabIndex = 12;
            this.User_Label.Text = "User";
            // 
            // User_Box
            // 
            this.User_Box.Location = new System.Drawing.Point(362, 62);
            this.User_Box.Name = "User_Box";
            this.User_Box.Size = new System.Drawing.Size(142, 20);
            this.User_Box.TabIndex = 11;
            // 
            // LogOut_Button
            // 
            this.LogOut_Button.Location = new System.Drawing.Point(534, 35);
            this.LogOut_Button.Name = "LogOut_Button";
            this.LogOut_Button.Size = new System.Drawing.Size(104, 47);
            this.LogOut_Button.TabIndex = 17;
            this.LogOut_Button.Text = "Log Out";
            this.LogOut_Button.UseVisualStyleBackColor = true;
            this.LogOut_Button.Visible = false;
            this.LogOut_Button.Click += new System.EventHandler(this.LogOut_Button_Click);
            // 
            // LeaderBoard_Button
            // 
            this.LeaderBoard_Button.Location = new System.Drawing.Point(362, 204);
            this.LeaderBoard_Button.Name = "LeaderBoard_Button";
            this.LeaderBoard_Button.Size = new System.Drawing.Size(104, 47);
            this.LeaderBoard_Button.TabIndex = 18;
            this.LeaderBoard_Button.Text = "LeaderBoard";
            this.LeaderBoard_Button.UseVisualStyleBackColor = true;
            this.LeaderBoard_Button.Click += new System.EventHandler(this.LeaderBoard_Button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 562);
            this.Controls.Add(this.LeaderBoard_Button);
            this.Controls.Add(this.LogOut_Button);
            this.Controls.Add(this.UserSign);
            this.Controls.Add(this.LogIn_Button);
            this.Controls.Add(this.Password_Label);
            this.Controls.Add(this.Password_Box);
            this.Controls.Add(this.User_Label);
            this.Controls.Add(this.User_Box);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button UserSign;
        private System.Windows.Forms.Button LogIn_Button;
        private System.Windows.Forms.Label Password_Label;
        private System.Windows.Forms.TextBox Password_Box;
        private System.Windows.Forms.Label User_Label;
        private System.Windows.Forms.TextBox User_Box;
        private System.Windows.Forms.Button LogOut_Button;
        private System.Windows.Forms.Button LeaderBoard_Button;
    }
}

