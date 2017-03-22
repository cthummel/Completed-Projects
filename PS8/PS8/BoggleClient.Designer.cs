namespace PS8
{
    partial class BoggleClient
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
            this.ServerBox = new System.Windows.Forms.TextBox();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.WordEnterBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Player1ScoreLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Player1ScoreBox = new System.Windows.Forms.TextBox();
            this.Player2ScoreBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instructionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FinalWordBoxP1 = new System.Windows.Forms.RichTextBox();
            this.WordListLabelPlayer1 = new System.Windows.Forms.Label();
            this.QuitGameButton = new System.Windows.Forms.Button();
            this.Letter1 = new System.Windows.Forms.RichTextBox();
            this.Letter2 = new System.Windows.Forms.RichTextBox();
            this.Letter3 = new System.Windows.Forms.RichTextBox();
            this.Letter4 = new System.Windows.Forms.RichTextBox();
            this.Letter5 = new System.Windows.Forms.RichTextBox();
            this.Letter6 = new System.Windows.Forms.RichTextBox();
            this.Letter7 = new System.Windows.Forms.RichTextBox();
            this.Letter8 = new System.Windows.Forms.RichTextBox();
            this.Letter9 = new System.Windows.Forms.RichTextBox();
            this.Letter10 = new System.Windows.Forms.RichTextBox();
            this.Letter11 = new System.Windows.Forms.RichTextBox();
            this.Letter12 = new System.Windows.Forms.RichTextBox();
            this.Letter13 = new System.Windows.Forms.RichTextBox();
            this.Letter14 = new System.Windows.Forms.RichTextBox();
            this.Letter15 = new System.Windows.Forms.RichTextBox();
            this.Letter16 = new System.Windows.Forms.RichTextBox();
            this.FinalWordBoxP2 = new System.Windows.Forms.RichTextBox();
            this.FinalWordLabelP2 = new System.Windows.Forms.Label();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.TimeBox = new System.Windows.Forms.TextBox();
            this.TimeRemainingBox = new System.Windows.Forms.RichTextBox();
            this.RemainingLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServerBox
            // 
            this.ServerBox.Location = new System.Drawing.Point(274, 50);
            this.ServerBox.Name = "ServerBox";
            this.ServerBox.Size = new System.Drawing.Size(723, 26);
            this.ServerBox.TabIndex = 0;
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(274, 82);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(723, 26);
            this.UsernameBox.TabIndex = 1;
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(163, 50);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(105, 20);
            this.ServerLabel.TabIndex = 2;
            this.ServerLabel.Text = "Server Name:";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(181, 82);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(87, 20);
            this.UsernameLabel.TabIndex = 3;
            this.UsernameLabel.Text = "Username:";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(274, 189);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(723, 35);
            this.StartButton.TabIndex = 4;
            this.StartButton.Text = "Find Match";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // WordEnterBox
            // 
            this.WordEnterBox.Location = new System.Drawing.Point(274, 502);
            this.WordEnterBox.Name = "WordEnterBox";
            this.WordEnterBox.Size = new System.Drawing.Size(723, 26);
            this.WordEnterBox.TabIndex = 5;
            this.WordEnterBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WordEnterBox_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 502);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Enter Word:";
            // 
            // Player1ScoreLabel
            // 
            this.Player1ScoreLabel.AutoSize = true;
            this.Player1ScoreLabel.Location = new System.Drawing.Point(351, 424);
            this.Player1ScoreLabel.Name = "Player1ScoreLabel";
            this.Player1ScoreLabel.Size = new System.Drawing.Size(93, 20);
            this.Player1ScoreLabel.TabIndex = 23;
            this.Player1ScoreLabel.Text = "Your Score:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(837, 424);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 20);
            this.label5.TabIndex = 24;
            this.label5.Text = "Player 2 Score:";
            // 
            // Player1ScoreBox
            // 
            this.Player1ScoreBox.Location = new System.Drawing.Point(344, 447);
            this.Player1ScoreBox.Name = "Player1ScoreBox";
            this.Player1ScoreBox.ReadOnly = true;
            this.Player1ScoreBox.Size = new System.Drawing.Size(100, 26);
            this.Player1ScoreBox.TabIndex = 25;
            // 
            // Player2ScoreBox
            // 
            this.Player2ScoreBox.Location = new System.Drawing.Point(841, 447);
            this.Player2ScoreBox.Name = "Player2ScoreBox";
            this.Player2ScoreBox.ReadOnly = true;
            this.Player2ScoreBox.Size = new System.Drawing.Size(100, 26);
            this.Player2ScoreBox.TabIndex = 26;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1325, 33);
            this.menuStrip1.TabIndex = 28;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.instructionsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // instructionsToolStripMenuItem
            // 
            this.instructionsToolStripMenuItem.Name = "instructionsToolStripMenuItem";
            this.instructionsToolStripMenuItem.Size = new System.Drawing.Size(189, 30);
            this.instructionsToolStripMenuItem.Text = "Instructions";
            this.instructionsToolStripMenuItem.Click += new System.EventHandler(this.instructionsToolStripMenuItem_Click);
            // 
            // FinalWordBoxP1
            // 
            this.FinalWordBoxP1.Location = new System.Drawing.Point(218, 575);
            this.FinalWordBoxP1.Name = "FinalWordBoxP1";
            this.FinalWordBoxP1.ReadOnly = true;
            this.FinalWordBoxP1.Size = new System.Drawing.Size(258, 324);
            this.FinalWordBoxP1.TabIndex = 29;
            this.FinalWordBoxP1.Text = "";
            // 
            // WordListLabelPlayer1
            // 
            this.WordListLabelPlayer1.AutoSize = true;
            this.WordListLabelPlayer1.Location = new System.Drawing.Point(270, 552);
            this.WordListLabelPlayer1.Name = "WordListLabelPlayer1";
            this.WordListLabelPlayer1.Size = new System.Drawing.Size(152, 20);
            this.WordListLabelPlayer1.TabIndex = 30;
            this.WordListLabelPlayer1.Text = "Your Final Word List";
            // 
            // QuitGameButton
            // 
            this.QuitGameButton.Location = new System.Drawing.Point(1069, 502);
            this.QuitGameButton.Name = "QuitGameButton";
            this.QuitGameButton.Size = new System.Drawing.Size(163, 29);
            this.QuitGameButton.TabIndex = 31;
            this.QuitGameButton.Text = "Quit Game";
            this.QuitGameButton.UseVisualStyleBackColor = true;
            this.QuitGameButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // Letter1
            // 
            this.Letter1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter1.Location = new System.Drawing.Point(506, 250);
            this.Letter1.Name = "Letter1";
            this.Letter1.ReadOnly = true;
            this.Letter1.Size = new System.Drawing.Size(55, 54);
            this.Letter1.TabIndex = 32;
            this.Letter1.Text = "";
            // 
            // Letter2
            // 
            this.Letter2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter2.Location = new System.Drawing.Point(567, 250);
            this.Letter2.Name = "Letter2";
            this.Letter2.ReadOnly = true;
            this.Letter2.Size = new System.Drawing.Size(55, 54);
            this.Letter2.TabIndex = 33;
            this.Letter2.Text = "";
            // 
            // Letter3
            // 
            this.Letter3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter3.Location = new System.Drawing.Point(628, 250);
            this.Letter3.Name = "Letter3";
            this.Letter3.ReadOnly = true;
            this.Letter3.Size = new System.Drawing.Size(55, 54);
            this.Letter3.TabIndex = 34;
            this.Letter3.Text = "";
            // 
            // Letter4
            // 
            this.Letter4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter4.Location = new System.Drawing.Point(689, 250);
            this.Letter4.Name = "Letter4";
            this.Letter4.ReadOnly = true;
            this.Letter4.Size = new System.Drawing.Size(55, 54);
            this.Letter4.TabIndex = 35;
            this.Letter4.Text = "";
            // 
            // Letter5
            // 
            this.Letter5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter5.Location = new System.Drawing.Point(506, 310);
            this.Letter5.Name = "Letter5";
            this.Letter5.ReadOnly = true;
            this.Letter5.Size = new System.Drawing.Size(55, 54);
            this.Letter5.TabIndex = 36;
            this.Letter5.Text = "";
            // 
            // Letter6
            // 
            this.Letter6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter6.Location = new System.Drawing.Point(567, 310);
            this.Letter6.Name = "Letter6";
            this.Letter6.ReadOnly = true;
            this.Letter6.Size = new System.Drawing.Size(55, 54);
            this.Letter6.TabIndex = 37;
            this.Letter6.Text = "";
            // 
            // Letter7
            // 
            this.Letter7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter7.Location = new System.Drawing.Point(628, 310);
            this.Letter7.Name = "Letter7";
            this.Letter7.ReadOnly = true;
            this.Letter7.Size = new System.Drawing.Size(55, 54);
            this.Letter7.TabIndex = 38;
            this.Letter7.Text = "";
            // 
            // Letter8
            // 
            this.Letter8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter8.Location = new System.Drawing.Point(689, 310);
            this.Letter8.Name = "Letter8";
            this.Letter8.ReadOnly = true;
            this.Letter8.Size = new System.Drawing.Size(55, 54);
            this.Letter8.TabIndex = 39;
            this.Letter8.Text = "";
            // 
            // Letter9
            // 
            this.Letter9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter9.Location = new System.Drawing.Point(506, 370);
            this.Letter9.Name = "Letter9";
            this.Letter9.ReadOnly = true;
            this.Letter9.Size = new System.Drawing.Size(55, 54);
            this.Letter9.TabIndex = 40;
            this.Letter9.Text = "";
            // 
            // Letter10
            // 
            this.Letter10.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter10.Location = new System.Drawing.Point(567, 370);
            this.Letter10.Name = "Letter10";
            this.Letter10.ReadOnly = true;
            this.Letter10.Size = new System.Drawing.Size(55, 54);
            this.Letter10.TabIndex = 41;
            this.Letter10.Text = "";
            // 
            // Letter11
            // 
            this.Letter11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter11.Location = new System.Drawing.Point(628, 370);
            this.Letter11.Name = "Letter11";
            this.Letter11.ReadOnly = true;
            this.Letter11.Size = new System.Drawing.Size(55, 54);
            this.Letter11.TabIndex = 42;
            this.Letter11.Text = "";
            // 
            // Letter12
            // 
            this.Letter12.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter12.Location = new System.Drawing.Point(689, 370);
            this.Letter12.Name = "Letter12";
            this.Letter12.ReadOnly = true;
            this.Letter12.Size = new System.Drawing.Size(55, 54);
            this.Letter12.TabIndex = 43;
            this.Letter12.Text = "";
            // 
            // Letter13
            // 
            this.Letter13.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter13.Location = new System.Drawing.Point(506, 430);
            this.Letter13.Name = "Letter13";
            this.Letter13.ReadOnly = true;
            this.Letter13.Size = new System.Drawing.Size(55, 54);
            this.Letter13.TabIndex = 44;
            this.Letter13.Text = "";
            // 
            // Letter14
            // 
            this.Letter14.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter14.Location = new System.Drawing.Point(567, 430);
            this.Letter14.Name = "Letter14";
            this.Letter14.ReadOnly = true;
            this.Letter14.Size = new System.Drawing.Size(55, 54);
            this.Letter14.TabIndex = 45;
            this.Letter14.Text = "";
            // 
            // Letter15
            // 
            this.Letter15.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter15.Location = new System.Drawing.Point(628, 430);
            this.Letter15.Name = "Letter15";
            this.Letter15.ReadOnly = true;
            this.Letter15.Size = new System.Drawing.Size(55, 54);
            this.Letter15.TabIndex = 46;
            this.Letter15.Text = "";
            // 
            // Letter16
            // 
            this.Letter16.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter16.Location = new System.Drawing.Point(689, 430);
            this.Letter16.Name = "Letter16";
            this.Letter16.ReadOnly = true;
            this.Letter16.Size = new System.Drawing.Size(55, 54);
            this.Letter16.TabIndex = 47;
            this.Letter16.Text = "";
            // 
            // FinalWordBoxP2
            // 
            this.FinalWordBoxP2.Location = new System.Drawing.Point(793, 575);
            this.FinalWordBoxP2.Name = "FinalWordBoxP2";
            this.FinalWordBoxP2.ReadOnly = true;
            this.FinalWordBoxP2.Size = new System.Drawing.Size(258, 324);
            this.FinalWordBoxP2.TabIndex = 48;
            this.FinalWordBoxP2.Text = "";
            // 
            // FinalWordLabelP2
            // 
            this.FinalWordLabelP2.AutoSize = true;
            this.FinalWordLabelP2.Location = new System.Drawing.Point(837, 552);
            this.FinalWordLabelP2.Name = "FinalWordLabelP2";
            this.FinalWordLabelP2.Size = new System.Drawing.Size(185, 20);
            this.FinalWordLabelP2.TabIndex = 49;
            this.FinalWordLabelP2.Text = "Player 2\'s Final Word List";
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(274, 114);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(723, 37);
            this.RegisterButton.TabIndex = 50;
            this.RegisterButton.Text = "Register User";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterBox_Click);
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Location = new System.Drawing.Point(184, 157);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(84, 20);
            this.TimeLabel.TabIndex = 51;
            this.TimeLabel.Text = "Time Limit:";
            // 
            // TimeBox
            // 
            this.TimeBox.Location = new System.Drawing.Point(274, 157);
            this.TimeBox.Name = "TimeBox";
            this.TimeBox.Size = new System.Drawing.Size(723, 26);
            this.TimeBox.TabIndex = 52;
            // 
            // TimeRemainingBox
            // 
            this.TimeRemainingBox.Location = new System.Drawing.Point(557, 657);
            this.TimeRemainingBox.Name = "TimeRemainingBox";
            this.TimeRemainingBox.ReadOnly = true;
            this.TimeRemainingBox.Size = new System.Drawing.Size(161, 96);
            this.TimeRemainingBox.TabIndex = 53;
            this.TimeRemainingBox.Text = "";
            // 
            // RemainingLabel
            // 
            this.RemainingLabel.AutoSize = true;
            this.RemainingLabel.Location = new System.Drawing.Point(576, 634);
            this.RemainingLabel.Name = "RemainingLabel";
            this.RemainingLabel.Size = new System.Drawing.Size(127, 20);
            this.RemainingLabel.TabIndex = 54;
            this.RemainingLabel.Text = "Time Remaining:";
            // 
            // BoggleClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1325, 938);
            this.Controls.Add(this.RemainingLabel);
            this.Controls.Add(this.TimeRemainingBox);
            this.Controls.Add(this.TimeBox);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.FinalWordLabelP2);
            this.Controls.Add(this.FinalWordBoxP2);
            this.Controls.Add(this.Letter16);
            this.Controls.Add(this.Letter15);
            this.Controls.Add(this.Letter14);
            this.Controls.Add(this.Letter13);
            this.Controls.Add(this.Letter12);
            this.Controls.Add(this.Letter11);
            this.Controls.Add(this.Letter10);
            this.Controls.Add(this.Letter9);
            this.Controls.Add(this.Letter8);
            this.Controls.Add(this.Letter7);
            this.Controls.Add(this.Letter6);
            this.Controls.Add(this.Letter5);
            this.Controls.Add(this.Letter4);
            this.Controls.Add(this.Letter3);
            this.Controls.Add(this.Letter2);
            this.Controls.Add(this.Letter1);
            this.Controls.Add(this.QuitGameButton);
            this.Controls.Add(this.WordListLabelPlayer1);
            this.Controls.Add(this.FinalWordBoxP1);
            this.Controls.Add(this.Player2ScoreBox);
            this.Controls.Add(this.Player1ScoreBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Player1ScoreLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.WordEnterBox);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.ServerLabel);
            this.Controls.Add(this.UsernameBox);
            this.Controls.Add(this.ServerBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BoggleClient";
            this.Text = "Boggle Client";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ServerBox;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox WordEnterBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Player1ScoreLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Player1ScoreBox;
        private System.Windows.Forms.TextBox Player2ScoreBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem instructionsToolStripMenuItem;
        private System.Windows.Forms.RichTextBox FinalWordBoxP1;
        private System.Windows.Forms.Label WordListLabelPlayer1;
        private System.Windows.Forms.Button QuitGameButton;
        private System.Windows.Forms.RichTextBox Letter1;
        private System.Windows.Forms.RichTextBox Letter2;
        private System.Windows.Forms.RichTextBox Letter3;
        private System.Windows.Forms.RichTextBox Letter4;
        private System.Windows.Forms.RichTextBox Letter5;
        private System.Windows.Forms.RichTextBox Letter6;
        private System.Windows.Forms.RichTextBox Letter7;
        private System.Windows.Forms.RichTextBox Letter8;
        private System.Windows.Forms.RichTextBox Letter9;
        private System.Windows.Forms.RichTextBox Letter10;
        private System.Windows.Forms.RichTextBox Letter11;
        private System.Windows.Forms.RichTextBox Letter12;
        private System.Windows.Forms.RichTextBox Letter13;
        private System.Windows.Forms.RichTextBox Letter14;
        private System.Windows.Forms.RichTextBox Letter15;
        private System.Windows.Forms.RichTextBox Letter16;
        private System.Windows.Forms.RichTextBox FinalWordBoxP2;
        private System.Windows.Forms.Label FinalWordLabelP2;
        private System.Windows.Forms.Button RegisterButton;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.TextBox TimeBox;
        private System.Windows.Forms.RichTextBox TimeRemainingBox;
        private System.Windows.Forms.Label RemainingLabel;
    }
}

