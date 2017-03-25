using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PS8
{
    public partial class BoggleClient : Form, IAnalysisView
    {
        public event Action<string, int> GameStart;
        public event Action<string,string> Register;
        public event Action CancelGame;
        public event Action<string> WordEntered;
        public event Action RegCancel;
        public event Action JoinCancel;

        public BoggleClient()
        {
            InitializeComponent();
            
            ServerBox.Text = "http://cs3500-boggle-s17.azurewebsites.net/BoggleService.svc/";
            UsernameBox.Text = "Player1";
            TimeBox.Text = "90";
            QuitGameButton.Enabled = false;
            StartButton.Enabled = false;
        }

        /// <summary>
        /// Fires when the user clicks the Join Match button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            //Tells Controller to start a match.
            //string server = ServerBox.Text;
            string player = UsernameBox.Text;
            int time = Int32.Parse(TimeBox.Text);

            //Empty out any boxes that have left over text in them.
            FinalWordBoxP1.Text = string.Empty;
            FinalWordBoxP2.Text = string.Empty;
            TimeRemainingBox.Text = string.Empty;
            Player1ScoreBox.Text = string.Empty;
            Player2ScoreBox.Text = string.Empty;
            foreach (Control c in Controls)
            {
                if (c.Name.StartsWith("Letter"))
                {
                    c.Text = string.Empty;
                }
            }

            //Locks down controls until cancel or game is complete.
            ServerBox.ReadOnly = true;
            UsernameBox.ReadOnly = true;
            TimeBox.ReadOnly = true;
            StartButton.Enabled = false;
            RegisterButton.Enabled = false;
            QuitGameButton.Enabled = true;

            //We could also consider repurposing the StartButton to turn into a cancel button after a game begins (after this click event occurs).

            if (GameStart != null)
            {
                GameStart(player, time);
            }
        }

        /// <summary>
        /// Fires when user enters a new word.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordEnterBox_KeyPress(object sender, KeyEventArgs e)
        {
            // Tells Controller about a new word that was entered.
            if (e.KeyCode == Keys.Enter)
            {
                WordEntered?.Invoke(WordEnterBox.Text);
                WordEnterBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// Sets the game board.
        /// </summary>
        /// <param name="LetterList"></param>
        public void SetLetters(string letters)
        {
            foreach (Control c in Controls)
            {
                int index;
                if (c.Name.StartsWith("Letter") && int.TryParse(c.Name.Substring(6), out index))
                {
                    c.Text = letters[index-1].ToString();

                    if (c.Text == "Q")
                    {
                        c.Text = "QU";
                    }
                }
            }
        }

        public void Update (string[] parameters)
        {
            TimeRemainingBox.Text = parameters[0];
            Player1ScoreBox.Text = parameters[1];
            Player2ScoreBox.Text = parameters[2];

            if (parameters.Length > 3)
            {
                Player1ScoreLabel.Text = parameters[3];
                Player2ScoreLabel.Text = parameters[4];
            }
            //FinalWordBoxP1.Text = parameters[3];
        }

        public void FinalWords(Dictionary<string, List<string>> FinalWords, string Player1Nickname, string Player2Nickname)
        {
            var TempList = new List<string>();
            
            //Set final words box for player 1.
            if (FinalWords.TryGetValue(Player1Nickname, out TempList))
            {
                foreach(string s in TempList)
                {
                    if (FinalWordBoxP1.Text == string.Empty)
                    {
                        FinalWordBoxP1.AppendText(s);
                    }
                    else
                    {
                        FinalWordBoxP1.AppendText(Environment.NewLine + s);
                    }   
                }
            }

            //Set final words box for player 2.
            if (FinalWords.TryGetValue(Player2Nickname, out TempList))
            {
                foreach (string s in TempList)
                {
                    if (FinalWordBoxP2.Text == string.Empty)
                    {
                        FinalWordBoxP2.AppendText(s);
                    }
                    else
                    {
                        FinalWordBoxP2.AppendText(Environment.NewLine + s);
                    }
                }
            }
        }

        public void GameOver()
        {
            RegisterButton.Enabled = true;
            StartButton.Enabled = true;
            QuitGameButton.Enabled = false;
            TimeBox.ReadOnly = false;
            ServerBox.ReadOnly = false;
            UsernameBox.ReadOnly = false;
            TimeBox.Focus();
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter a Boggle server name and username and then click Register to regester your name with the server." + Environment.NewLine +
                "Then enter a time limit for the game between 5 and 250 seconds. Next click find match to find an opponent." + Environment.NewLine +
                "Type words below and press enter to confirm. When the game is complete you will see the words and corresponding scores for both players.");
        }

        /// <summary>
        /// Quits the currently running game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (CancelGame != null)
            {
                CancelGame();
            }
        }

        /// <summary>
        /// Fires when user wants to register a new user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterBox_Click(object sender, EventArgs e)
        {
            Register?.Invoke(UsernameBox.Text, ServerBox.Text);
            StartButton.Enabled = true;
        }
    }
}
