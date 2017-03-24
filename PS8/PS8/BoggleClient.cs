using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS8
{
    public partial class BoggleClient : Form, IAnalysisView
    {
        public event Action<string, int> GameStart;

        public event Action<string,string> Register;

        public event Action CancelGame;

        public event Action<string> WordEntered;


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
        /// Fires when the user clicks the Find Match button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            //Tells Controller to start a match.
            //string server = ServerBox.Text;
            string player = UsernameBox.Text;
            int time = Int32.Parse(TimeBox.Text);

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
            //Tells Controller about a new word that was entered.
            string word = WordEnterBox.Text;
            WordEntered?.Invoke(word);
            
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
            //FinalWordBoxP1.Text = parameters[3];
        }


        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("");
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
