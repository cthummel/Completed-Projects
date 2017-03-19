using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS8
{
    public class Controller
    {
        private IAnalysisView window;

        public Controller(IAnalysisView window)
        {
            this.window = window;


            window.GameStart += StartMatch;


        }

        private void StartMatch(string server, string player)
        {

        }


        private void ReturnLetters()
        {
            //Asks server for letters and parses them.
            //Saves them into a returnletters list before sending to the view to update.
            var returnletters = new List<string>();




            window.SetLetters(returnletters);
        }


    }
}
