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
        public event Action<string, string> GameStart;



        public BoggleClient()
        {
            InitializeComponent();
            ServerNameBox.Text = "Enter default server name here.";
        }

        

        private void StartButton_Click(object sender, EventArgs e)
        {
            //Tells Controller to start a match.
            string Server = ServerNameBox.Text;
            string Player = PlayerNameBox.Text;

            //Locks down controls until cancel or game is complete.
            ServerNameBox.ReadOnly = true;
            PlayerNameBox.ReadOnly = true;
            StartButton.Enabled = false;

            //We could also consider repurposing the StartButton to turn into a cancel button after a game begins (after this click event occurs).

            if (GameStart != null)
            {
                GameStart(Server, Player);
            }
        }

        private void WordEnterBox_KeyPress(object sender, KeyEventArgs e)
        {
            //Tells Controller about a new word that was entered.

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
    }
}
