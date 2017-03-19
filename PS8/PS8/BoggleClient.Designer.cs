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
            this.ServerNameBox = new System.Windows.Forms.TextBox();
            this.PlayerNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.WordEnterBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Letter1 = new System.Windows.Forms.TextBox();
            this.Letter2 = new System.Windows.Forms.TextBox();
            this.Letter3 = new System.Windows.Forms.TextBox();
            this.Letter4 = new System.Windows.Forms.TextBox();
            this.Letter5 = new System.Windows.Forms.TextBox();
            this.Letter6 = new System.Windows.Forms.TextBox();
            this.Letter7 = new System.Windows.Forms.TextBox();
            this.Letter8 = new System.Windows.Forms.TextBox();
            this.Letter9 = new System.Windows.Forms.TextBox();
            this.Letter11 = new System.Windows.Forms.TextBox();
            this.Letter10 = new System.Windows.Forms.TextBox();
            this.Letter12 = new System.Windows.Forms.TextBox();
            this.Letter13 = new System.Windows.Forms.TextBox();
            this.Letter14 = new System.Windows.Forms.TextBox();
            this.Letter15 = new System.Windows.Forms.TextBox();
            this.Letter16 = new System.Windows.Forms.TextBox();
            this.Player1ScoreLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Player1ScoreBox = new System.Windows.Forms.TextBox();
            this.Player2ScoreBox = new System.Windows.Forms.TextBox();
            this.LetterLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ServerNameBox
            // 
            this.ServerNameBox.Location = new System.Drawing.Point(274, 71);
            this.ServerNameBox.Name = "ServerNameBox";
            this.ServerNameBox.Size = new System.Drawing.Size(723, 26);
            this.ServerNameBox.TabIndex = 0;
            // 
            // PlayerNameBox
            // 
            this.PlayerNameBox.Location = new System.Drawing.Point(274, 141);
            this.PlayerNameBox.Name = "PlayerNameBox";
            this.PlayerNameBox.Size = new System.Drawing.Size(723, 26);
            this.PlayerNameBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Player Name:";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(1003, 71);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(163, 96);
            this.StartButton.TabIndex = 4;
            this.StartButton.Text = "Find Match";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // WordEnterBox
            // 
            this.WordEnterBox.Location = new System.Drawing.Point(274, 796);
            this.WordEnterBox.Name = "WordEnterBox";
            this.WordEnterBox.Size = new System.Drawing.Size(723, 26);
            this.WordEnterBox.TabIndex = 5;
            this.WordEnterBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WordEnterBox_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 796);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Enter Word:";
            // 
            // Letter1
            // 
            this.Letter1.Location = new System.Drawing.Point(329, 344);
            this.Letter1.Name = "Letter1";
            this.Letter1.ReadOnly = true;
            this.Letter1.Size = new System.Drawing.Size(100, 26);
            this.Letter1.TabIndex = 7;
            this.Letter1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter2
            // 
            this.Letter2.Location = new System.Drawing.Point(506, 344);
            this.Letter2.Name = "Letter2";
            this.Letter2.ReadOnly = true;
            this.Letter2.Size = new System.Drawing.Size(100, 26);
            this.Letter2.TabIndex = 8;
            this.Letter2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter3
            // 
            this.Letter3.Location = new System.Drawing.Point(674, 344);
            this.Letter3.Name = "Letter3";
            this.Letter3.ReadOnly = true;
            this.Letter3.Size = new System.Drawing.Size(100, 26);
            this.Letter3.TabIndex = 9;
            this.Letter3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter4
            // 
            this.Letter4.Location = new System.Drawing.Point(854, 344);
            this.Letter4.Name = "Letter4";
            this.Letter4.ReadOnly = true;
            this.Letter4.Size = new System.Drawing.Size(100, 26);
            this.Letter4.TabIndex = 10;
            this.Letter4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter5
            // 
            this.Letter5.Location = new System.Drawing.Point(329, 433);
            this.Letter5.Name = "Letter5";
            this.Letter5.ReadOnly = true;
            this.Letter5.Size = new System.Drawing.Size(100, 26);
            this.Letter5.TabIndex = 11;
            this.Letter5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter6
            // 
            this.Letter6.Location = new System.Drawing.Point(506, 433);
            this.Letter6.Name = "Letter6";
            this.Letter6.ReadOnly = true;
            this.Letter6.Size = new System.Drawing.Size(100, 26);
            this.Letter6.TabIndex = 12;
            this.Letter6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter7
            // 
            this.Letter7.Location = new System.Drawing.Point(674, 433);
            this.Letter7.Name = "Letter7";
            this.Letter7.ReadOnly = true;
            this.Letter7.Size = new System.Drawing.Size(100, 26);
            this.Letter7.TabIndex = 13;
            this.Letter7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter8
            // 
            this.Letter8.Location = new System.Drawing.Point(854, 433);
            this.Letter8.Name = "Letter8";
            this.Letter8.ReadOnly = true;
            this.Letter8.Size = new System.Drawing.Size(100, 26);
            this.Letter8.TabIndex = 14;
            this.Letter8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter9
            // 
            this.Letter9.Location = new System.Drawing.Point(329, 530);
            this.Letter9.Name = "Letter9";
            this.Letter9.ReadOnly = true;
            this.Letter9.Size = new System.Drawing.Size(100, 26);
            this.Letter9.TabIndex = 15;
            this.Letter9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter11
            // 
            this.Letter11.Location = new System.Drawing.Point(674, 530);
            this.Letter11.Name = "Letter11";
            this.Letter11.ReadOnly = true;
            this.Letter11.Size = new System.Drawing.Size(100, 26);
            this.Letter11.TabIndex = 16;
            this.Letter11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter10
            // 
            this.Letter10.Location = new System.Drawing.Point(506, 530);
            this.Letter10.Name = "Letter10";
            this.Letter10.ReadOnly = true;
            this.Letter10.Size = new System.Drawing.Size(100, 26);
            this.Letter10.TabIndex = 17;
            this.Letter10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter12
            // 
            this.Letter12.Location = new System.Drawing.Point(854, 530);
            this.Letter12.Name = "Letter12";
            this.Letter12.ReadOnly = true;
            this.Letter12.Size = new System.Drawing.Size(100, 26);
            this.Letter12.TabIndex = 18;
            this.Letter12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter13
            // 
            this.Letter13.Location = new System.Drawing.Point(329, 628);
            this.Letter13.Name = "Letter13";
            this.Letter13.ReadOnly = true;
            this.Letter13.Size = new System.Drawing.Size(100, 26);
            this.Letter13.TabIndex = 19;
            this.Letter13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter14
            // 
            this.Letter14.Location = new System.Drawing.Point(506, 628);
            this.Letter14.Name = "Letter14";
            this.Letter14.ReadOnly = true;
            this.Letter14.Size = new System.Drawing.Size(100, 26);
            this.Letter14.TabIndex = 20;
            this.Letter14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter15
            // 
            this.Letter15.Location = new System.Drawing.Point(674, 628);
            this.Letter15.Name = "Letter15";
            this.Letter15.ReadOnly = true;
            this.Letter15.Size = new System.Drawing.Size(100, 26);
            this.Letter15.TabIndex = 21;
            this.Letter15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Letter16
            // 
            this.Letter16.Location = new System.Drawing.Point(854, 628);
            this.Letter16.Name = "Letter16";
            this.Letter16.ReadOnly = true;
            this.Letter16.Size = new System.Drawing.Size(100, 26);
            this.Letter16.TabIndex = 22;
            this.Letter16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Player1ScoreLabel
            // 
            this.Player1ScoreLabel.AutoSize = true;
            this.Player1ScoreLabel.Location = new System.Drawing.Point(82, 344);
            this.Player1ScoreLabel.Name = "Player1ScoreLabel";
            this.Player1ScoreLabel.Size = new System.Drawing.Size(93, 20);
            this.Player1ScoreLabel.TabIndex = 23;
            this.Player1ScoreLabel.Text = "Your Score:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1136, 343);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 20);
            this.label5.TabIndex = 24;
            this.label5.Text = "Player 2 Score:";
            // 
            // Player1ScoreBox
            // 
            this.Player1ScoreBox.Location = new System.Drawing.Point(75, 367);
            this.Player1ScoreBox.Name = "Player1ScoreBox";
            this.Player1ScoreBox.ReadOnly = true;
            this.Player1ScoreBox.Size = new System.Drawing.Size(100, 26);
            this.Player1ScoreBox.TabIndex = 25;
            // 
            // Player2ScoreBox
            // 
            this.Player2ScoreBox.Location = new System.Drawing.Point(1140, 366);
            this.Player2ScoreBox.Name = "Player2ScoreBox";
            this.Player2ScoreBox.ReadOnly = true;
            this.Player2ScoreBox.Size = new System.Drawing.Size(100, 26);
            this.Player2ScoreBox.TabIndex = 26;
            // 
            // LetterLabel
            // 
            this.LetterLabel.AutoSize = true;
            this.LetterLabel.Location = new System.Drawing.Point(567, 302);
            this.LetterLabel.Name = "LetterLabel";
            this.LetterLabel.Size = new System.Drawing.Size(150, 20);
            this.LetterLabel.TabIndex = 27;
            this.LetterLabel.Text = "Collection of Letters";
            // 
            // BoggleClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 938);
            this.Controls.Add(this.LetterLabel);
            this.Controls.Add(this.Player2ScoreBox);
            this.Controls.Add(this.Player1ScoreBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Player1ScoreLabel);
            this.Controls.Add(this.Letter16);
            this.Controls.Add(this.Letter15);
            this.Controls.Add(this.Letter14);
            this.Controls.Add(this.Letter13);
            this.Controls.Add(this.Letter12);
            this.Controls.Add(this.Letter10);
            this.Controls.Add(this.Letter11);
            this.Controls.Add(this.Letter9);
            this.Controls.Add(this.Letter8);
            this.Controls.Add(this.Letter7);
            this.Controls.Add(this.Letter6);
            this.Controls.Add(this.Letter5);
            this.Controls.Add(this.Letter4);
            this.Controls.Add(this.Letter3);
            this.Controls.Add(this.Letter2);
            this.Controls.Add(this.Letter1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.WordEnterBox);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PlayerNameBox);
            this.Controls.Add(this.ServerNameBox);
            this.Name = "BoggleClient";
            this.Text = "Boggle Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ServerNameBox;
        private System.Windows.Forms.TextBox PlayerNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox WordEnterBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Letter1;
        private System.Windows.Forms.TextBox Letter2;
        private System.Windows.Forms.TextBox Letter3;
        private System.Windows.Forms.TextBox Letter4;
        private System.Windows.Forms.TextBox Letter5;
        private System.Windows.Forms.TextBox Letter6;
        private System.Windows.Forms.TextBox Letter7;
        private System.Windows.Forms.TextBox Letter8;
        private System.Windows.Forms.TextBox Letter9;
        private System.Windows.Forms.TextBox Letter11;
        private System.Windows.Forms.TextBox Letter10;
        private System.Windows.Forms.TextBox Letter12;
        private System.Windows.Forms.TextBox Letter13;
        private System.Windows.Forms.TextBox Letter14;
        private System.Windows.Forms.TextBox Letter15;
        private System.Windows.Forms.TextBox Letter16;
        private System.Windows.Forms.Label Player1ScoreLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Player1ScoreBox;
        private System.Windows.Forms.TextBox Player2ScoreBox;
        private System.Windows.Forms.Label LetterLabel;
    }
}

