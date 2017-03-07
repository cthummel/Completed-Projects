using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;

namespace SpreadsheetGUI
{
    public class Controller
    {
        // The window being controlled
        private IAnalysisView window;
        private Spreadsheet sheet;
        

        /// <summary>
        /// Begins controlling window.
        /// </summary>
        public Controller(IAnalysisView window)
        {
            this.window = window;
            this.sheet = new Spreadsheet();
            //window.FileChosenEvent += HandleFileChosen;
            window.CloseEvent += HandleClose;
            window.NewEvent += HandleNew;
            //window.FileSaveEvent += Save;
        }

        //private void Save(string dest)
        //{
        //    sheet.Save()
        //}

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            window.DoClose();
        }

        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }
    }
}
