namespace BoomWords
{
    partial class Game
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
            this.components = new System.ComponentModel.Container();
            this.GameLabel = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.LeaveButton = new System.Windows.Forms.Button();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.GamenameLabel = new System.Windows.Forms.Label();
            this.TurnLabel = new System.Windows.Forms.Label();
            this.SyllableLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TurnAttemptLabel = new System.Windows.Forms.Label();
            this.PlayersGrid = new System.Windows.Forms.DataGridView();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lives = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WordBox = new System.Windows.Forms.TextBox();
            this.BoomTimer = new System.Windows.Forms.Timer(this.components);
            this.RunDelay = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PlayersGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // GameLabel
            // 
            this.GameLabel.AutoSize = true;
            this.GameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameLabel.Location = new System.Drawing.Point(357, 11);
            this.GameLabel.Name = "GameLabel";
            this.GameLabel.Size = new System.Drawing.Size(327, 51);
            this.GameLabel.TabIndex = 0;
            this.GameLabel.Text = "BOOMWORDS";
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.ForeColor = System.Drawing.Color.Red;
            this.StartButton.Location = new System.Drawing.Point(12, 95);
            this.StartButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(195, 32);
            this.StartButton.TabIndex = 2;
            this.StartButton.Text = "START";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Visible = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // LeaveButton
            // 
            this.LeaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeaveButton.ForeColor = System.Drawing.Color.Red;
            this.LeaveButton.Location = new System.Drawing.Point(12, 498);
            this.LeaveButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LeaveButton.Name = "LeaveButton";
            this.LeaveButton.Size = new System.Drawing.Size(165, 39);
            this.LeaveButton.TabIndex = 3;
            this.LeaveButton.Text = "LEAVE";
            this.LeaveButton.UseVisualStyleBackColor = true;
            this.LeaveButton.Click += new System.EventHandler(this.LeaveButton_Click);
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.Font = new System.Drawing.Font("Rockwell", 15F);
            this.UsernameLabel.Location = new System.Drawing.Point(420, 473);
            this.UsernameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(192, 32);
            this.UsernameLabel.TabIndex = 24;
            this.UsernameLabel.Text = "User";
            this.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // GamenameLabel
            // 
            this.GamenameLabel.Font = new System.Drawing.Font("Rockwell", 15F);
            this.GamenameLabel.Location = new System.Drawing.Point(13, 60);
            this.GamenameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GamenameLabel.Name = "GamenameLabel";
            this.GamenameLabel.Size = new System.Drawing.Size(195, 32);
            this.GamenameLabel.TabIndex = 25;
            this.GamenameLabel.Text = "GameName";
            this.GamenameLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // TurnLabel
            // 
            this.TurnLabel.Font = new System.Drawing.Font("Rockwell", 15F);
            this.TurnLabel.Location = new System.Drawing.Point(335, 343);
            this.TurnLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TurnLabel.Name = "TurnLabel";
            this.TurnLabel.Size = new System.Drawing.Size(195, 32);
            this.TurnLabel.TabIndex = 26;
            this.TurnLabel.Text = "Turn";
            this.TurnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SyllableLabel
            // 
            this.SyllableLabel.Font = new System.Drawing.Font("Rockwell", 15F);
            this.SyllableLabel.Location = new System.Drawing.Point(335, 375);
            this.SyllableLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SyllableLabel.Name = "SyllableLabel";
            this.SyllableLabel.Size = new System.Drawing.Size(195, 32);
            this.SyllableLabel.TabIndex = 27;
            this.SyllableLabel.Text = "Syllable";
            this.SyllableLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Rockwell", 15F);
            this.label1.Location = new System.Drawing.Point(201, 343);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 32);
            this.label1.TabIndex = 28;
            this.label1.Text = "Turno:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Rockwell", 15F);
            this.label2.Location = new System.Drawing.Point(197, 375);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 32);
            this.label2.TabIndex = 29;
            this.label2.Text = "Syllable:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TurnAttemptLabel
            // 
            this.TurnAttemptLabel.Font = new System.Drawing.Font("Rockwell", 15F);
            this.TurnAttemptLabel.Location = new System.Drawing.Point(611, 343);
            this.TurnAttemptLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TurnAttemptLabel.Name = "TurnAttemptLabel";
            this.TurnAttemptLabel.Size = new System.Drawing.Size(195, 32);
            this.TurnAttemptLabel.TabIndex = 30;
            this.TurnAttemptLabel.Text = "Attempt";
            this.TurnAttemptLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // PlayersGrid
            // 
            this.PlayersGrid.ColumnHeadersHeight = 29;
            this.PlayersGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.User,
            this.Lives});
            this.PlayersGrid.Location = new System.Drawing.Point(829, 38);
            this.PlayersGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PlayersGrid.Name = "PlayersGrid";
            this.PlayersGrid.RowHeadersVisible = false;
            this.PlayersGrid.RowHeadersWidth = 51;
            this.PlayersGrid.RowTemplate.Height = 24;
            this.PlayersGrid.Size = new System.Drawing.Size(253, 236);
            this.PlayersGrid.TabIndex = 31;
            // 
            // User
            // 
            this.User.HeaderText = "Username";
            this.User.MinimumWidth = 6;
            this.User.Name = "User";
            this.User.Width = 125;
            // 
            // Lives
            // 
            this.Lives.HeaderText = "Lives";
            this.Lives.MinimumWidth = 6;
            this.Lives.Name = "Lives";
            this.Lives.Width = 125;
            // 
            // WordBox
            // 
            this.WordBox.Enabled = false;
            this.WordBox.Font = new System.Drawing.Font("Rockwell", 9.75F);
            this.WordBox.ForeColor = System.Drawing.Color.Black;
            this.WordBox.Location = new System.Drawing.Point(388, 510);
            this.WordBox.Margin = new System.Windows.Forms.Padding(5);
            this.WordBox.Name = "WordBox";
            this.WordBox.Size = new System.Drawing.Size(249, 27);
            this.WordBox.TabIndex = 33;
            this.WordBox.TabStop = false;
            this.WordBox.Text = "Word";
            this.WordBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.WordBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CheckWord);
            // 
            // BoomTimer
            // 
            this.BoomTimer.Interval = 20000;
            this.BoomTimer.Tick += new System.EventHandler(this.BoomTimer_Tick);
            // 
            // RunDelay
            // 
            this.RunDelay.Interval = 2000;
            this.RunDelay.Tick += new System.EventHandler(this.RunDelay_Tick);
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 565);
            this.ControlBox = false;
            this.Controls.Add(this.WordBox);
            this.Controls.Add(this.PlayersGrid);
            this.Controls.Add(this.TurnAttemptLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SyllableLabel);
            this.Controls.Add(this.TurnLabel);
            this.Controls.Add(this.GamenameLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.LeaveButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.GameLabel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Game";
            this.Text = "Game";
            this.Load += new System.EventHandler(this.Game_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PlayersGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label GameLabel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button LeaveButton;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label GamenameLabel;
        private System.Windows.Forms.Label TurnLabel;
        private System.Windows.Forms.Label SyllableLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label TurnAttemptLabel;
        private System.Windows.Forms.DataGridView PlayersGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lives;
        private System.Windows.Forms.TextBox WordBox;
        private System.Windows.Forms.Timer BoomTimer;
        private System.Windows.Forms.Timer RunDelay;
    }
}