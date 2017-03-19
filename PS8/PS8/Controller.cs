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

        private void StartMatch(string ServerName, string PlayerName)
        {

        }


    }
}
