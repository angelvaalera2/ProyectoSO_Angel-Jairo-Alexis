namespace BoomWords
{
    partial class Launcher
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
            this.SignInButton = new System.Windows.Forms.Button();
            this.LogInButton = new System.Windows.Forms.Button();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.ConnectionLabel = new System.Windows.Forms.Label();
            this.LeaderBoardGrid = new System.Windows.Forms.DataGridView();
            this.Username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Score = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.LeaderBoardGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // SignInButton
            // 
            this.SignInButton.Font = new System.Drawing.Font("Rockwell", 12F);
            this.SignInButton.Location = new System.Drawing.Point(300, 131);
            this.SignInButton.Margin = new System.Windows.Forms.Padding(4);
            this.SignInButton.Name = "SignInButton";
            this.SignInButton.Size = new System.Drawing.Size(139, 58);
            this.SignInButton.TabIndex = 26;
            this.SignInButton.Text = "Sign In";
            this.SignInButton.UseVisualStyleBackColor = true;
            this.SignInButton.Click += new System.EventHandler(this.SignInButton_Click);
            // 
            // LogInButton
            // 
            this.LogInButton.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogInButton.Location = new System.Drawing.Point(300, 56);
            this.LogInButton.Margin = new System.Windows.Forms.Padding(4);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(139, 60);
            this.LogInButton.TabIndex = 25;
            this.LogInButton.Text = "Log In";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogInButton_Click);
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.Location = new System.Drawing.Point(70, 131);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(189, 25);
            this.PasswordLabel.TabIndex = 24;
            this.PasswordLabel.Text = "Password";
            this.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Font = new System.Drawing.Font("Rockwell", 9.75F);
            this.PasswordBox.Location = new System.Drawing.Point(70, 159);
            this.PasswordBox.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(188, 27);
            this.PasswordBox.TabIndex = 23;
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.Font = new System.Drawing.Font("Rockwell", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameLabel.Location = new System.Drawing.Point(70, 61);
            this.UsernameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(189, 25);
            this.UsernameLabel.TabIndex = 22;
            this.UsernameLabel.Text = "User";
            this.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // UsernameBox
            // 
            this.UsernameBox.Font = new System.Drawing.Font("Rockwell", 9.75F);
            this.UsernameBox.Location = new System.Drawing.Point(70, 89);
            this.UsernameBox.Margin = new System.Windows.Forms.Padding(4);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(188, 27);
            this.UsernameBox.TabIndex = 21;
            this.UsernameBox.TabStop = false;
            // 
            // ConnectionLabel
            // 
            this.ConnectionLabel.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.ConnectionLabel.Location = new System.Drawing.Point(544, 226);
            this.ConnectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ConnectionLabel.Name = "ConnectionLabel";
            this.ConnectionLabel.Size = new System.Drawing.Size(195, 32);
            this.ConnectionLabel.TabIndex = 29;
            this.ConnectionLabel.Text = "Connection Parameters";
            this.ConnectionLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // LeaderBoardGrid
            // 
            this.LeaderBoardGrid.ColumnHeadersHeight = 29;
            this.LeaderBoardGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Username,
            this.Score});
            this.LeaderBoardGrid.Location = new System.Drawing.Point(486, 26);
            this.LeaderBoardGrid.Name = "LeaderBoardGrid";
            this.LeaderBoardGrid.ReadOnly = true;
            this.LeaderBoardGrid.RowHeadersVisible = false;
            this.LeaderBoardGrid.RowHeadersWidth = 51;
            this.LeaderBoardGrid.RowTemplate.Height = 24;
            this.LeaderBoardGrid.Size = new System.Drawing.Size(253, 174);
            this.LeaderBoardGrid.TabIndex = 30;
            // 
            // Username
            // 
            this.Username.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Username.HeaderText = "Username";
            this.Username.MinimumWidth = 125;
            this.Username.Name = "Username";
            this.Username.ReadOnly = true;
            this.Username.Width = 125;
            // 
            // Score
            // 
            this.Score.HeaderText = "Score";
            this.Score.MinimumWidth = 125;
            this.Score.Name = "Score";
            this.Score.ReadOnly = true;
            this.Score.Width = 125;
            // 
            // Launcher
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(752, 267);
            this.Controls.Add(this.LeaderBoardGrid);
            this.Controls.Add(this.ConnectionLabel);
            this.Controls.Add(this.SignInButton);
            this.Controls.Add(this.LogInButton);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.UsernameBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Launcher";
            this.Text = "--Launcher--";
            this.Activated += new System.EventHandler(this.Launcher_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Launcher_Closed);
            this.Load += new System.EventHandler(this.Launcher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LeaderBoardGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button SignInButton;
        private System.Windows.Forms.Button LogInButton;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.Label ConnectionLabel;
        private System.Windows.Forms.DataGridView LeaderBoardGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Username;
        private System.Windows.Forms.DataGridViewTextBoxColumn Score;
    }
}

