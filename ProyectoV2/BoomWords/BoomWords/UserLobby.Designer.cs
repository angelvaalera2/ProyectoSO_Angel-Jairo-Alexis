namespace BoomWords
{
    partial class UserLobby
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
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.LogOutButton = new System.Windows.Forms.Button();
            this.BoomWordsLabel = new System.Windows.Forms.Label();
            this.JoinButton = new System.Windows.Forms.Button();
            this.CreateButton = new System.Windows.Forms.Button();
            this.JoinGamenameBox = new System.Windows.Forms.MaskedTextBox();
            this.JoinPasswordBox = new System.Windows.Forms.MaskedTextBox();
            this.CreateGamenameBox = new System.Windows.Forms.MaskedTextBox();
            this.CreatePasswordBox = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.Font = new System.Drawing.Font("Rockwell", 15F);
            this.UsernameLabel.Location = new System.Drawing.Point(16, 11);
            this.UsernameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(192, 64);
            this.UsernameLabel.TabIndex = 23;
            this.UsernameLabel.Text = "User";
            // 
            // LogOutButton
            // 
            this.LogOutButton.Font = new System.Drawing.Font("Rockwell", 12F);
            this.LogOutButton.Location = new System.Drawing.Point(864, 481);
            this.LogOutButton.Margin = new System.Windows.Forms.Padding(4);
            this.LogOutButton.Name = "LogOutButton";
            this.LogOutButton.Size = new System.Drawing.Size(187, 58);
            this.LogOutButton.TabIndex = 29;
            this.LogOutButton.Text = "Log Out";
            this.LogOutButton.UseVisualStyleBackColor = true;
            this.LogOutButton.Click += new System.EventHandler(this.LogOutButton_Click);
            // 
            // BoomWordsLabel
            // 
            this.BoomWordsLabel.AutoSize = true;
            this.BoomWordsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoomWordsLabel.Location = new System.Drawing.Point(316, 60);
            this.BoomWordsLabel.Name = "BoomWordsLabel";
            this.BoomWordsLabel.Size = new System.Drawing.Size(453, 69);
            this.BoomWordsLabel.TabIndex = 32;
            this.BoomWordsLabel.Text = "BOOMWORDS";
            // 
            // JoinButton
            // 
            this.JoinButton.Location = new System.Drawing.Point(617, 392);
            this.JoinButton.Name = "JoinButton";
            this.JoinButton.Size = new System.Drawing.Size(217, 81);
            this.JoinButton.TabIndex = 40;
            this.JoinButton.Text = "Join";
            this.JoinButton.UseVisualStyleBackColor = true;
            this.JoinButton.Click += new System.EventHandler(this.JoinButton_Click);
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(184, 396);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(250, 77);
            this.CreateButton.TabIndex = 41;
            this.CreateButton.Text = "Create a Party";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // JoinGamenameBox
            // 
            this.JoinGamenameBox.Location = new System.Drawing.Point(617, 251);
            this.JoinGamenameBox.Name = "JoinGamenameBox";
            this.JoinGamenameBox.Size = new System.Drawing.Size(217, 22);
            this.JoinGamenameBox.TabIndex = 42;
            // 
            // JoinPasswordBox
            // 
            this.JoinPasswordBox.Location = new System.Drawing.Point(617, 311);
            this.JoinPasswordBox.Name = "JoinPasswordBox";
            this.JoinPasswordBox.Size = new System.Drawing.Size(217, 22);
            this.JoinPasswordBox.TabIndex = 43;
            // 
            // CreateGamenameBox
            // 
            this.CreateGamenameBox.Location = new System.Drawing.Point(195, 251);
            this.CreateGamenameBox.Name = "CreateGamenameBox";
            this.CreateGamenameBox.Size = new System.Drawing.Size(239, 22);
            this.CreateGamenameBox.TabIndex = 44;
            // 
            // CreatePasswordBox
            // 
            this.CreatePasswordBox.Location = new System.Drawing.Point(195, 322);
            this.CreatePasswordBox.Name = "CreatePasswordBox";
            this.CreatePasswordBox.Size = new System.Drawing.Size(239, 22);
            this.CreatePasswordBox.TabIndex = 45;
            // 
            // UserLobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.ControlBox = false;
            this.Controls.Add(this.CreatePasswordBox);
            this.Controls.Add(this.CreateGamenameBox);
            this.Controls.Add(this.JoinPasswordBox);
            this.Controls.Add(this.JoinGamenameBox);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.JoinButton);
            this.Controls.Add(this.BoomWordsLabel);
            this.Controls.Add(this.LogOutButton);
            this.Controls.Add(this.UsernameLabel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "UserLobby";
            this.Text = "UserLobby";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserLobby_Closed);
            this.Load += new System.EventHandler(this.UserLobby_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Button LogOutButton;
        private System.Windows.Forms.Label BoomWordsLabel;
        private System.Windows.Forms.Button JoinButton;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.MaskedTextBox JoinGamenameBox;
        private System.Windows.Forms.MaskedTextBox JoinPasswordBox;
        private System.Windows.Forms.MaskedTextBox CreateGamenameBox;
        private System.Windows.Forms.MaskedTextBox CreatePasswordBox;
    }
}