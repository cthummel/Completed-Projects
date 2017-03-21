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

        public event Action CancelGame;

        public event Action<string> WordEntered;


        public BoggleClient()
        {
            InitializeComponent();
            
            ServerBox.Text = "Player1";
            UsernameBox.Text = "90";
            CancelButton.Enabled = false;
        }

        

        private void StartButton_Click(object sender, EventArgs e)
        {
            //Tells Controller to start a match.
            string player = ServerBox.Text;
            int time = Int32.Parse(UsernameBox.Text);

            //Locks down controls until cancel or game is complete.
            ServerBox.ReadOnly = true;
            UsernameBox.ReadOnly = true;
            UsernameBox.ReadOnly = true;
            StartButton.Enabled = false;
            CancelButton.Enabled = true;

            //We could also consider repurposing the StartButton to turn into a cancel button after a game begins (after this click event occurs).

            if (GameStart != null)
            {
                GameStart(player, time);
            }
        }

        private void WordEnterBox_KeyPress(object sender, KeyEventArgs e)
        {
            //Tells Controller about a new word that was entered.
            string word = WordEnterBox.Text;

        }

        public void SetLetters(List<string> LetterList)
        {
            foreach (Control c in Controls)
            {
                int index;
                if (c.Name.StartsWith("Letter") && int.TryParse(c.Name.Substring(6), out index))
                {
                    c.Text = LetterList.ElementAt(index - 1);
                }
            }
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (CancelGame != null)
            {
                CancelGame();
            }
        }

        
    }
}
