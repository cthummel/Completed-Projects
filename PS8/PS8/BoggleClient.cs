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

        }

        private void WordEnterBox_KeyPress(object sender, KeyEventArgs e)
        {
            //Tells Controller about a new word that was entered.

        }
    }
}
