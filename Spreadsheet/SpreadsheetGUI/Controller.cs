using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SS;
using System.Windows.Forms;

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
            window.SetContents += UpdateContents;
            window.FileCloseEvent += HandleClose;
            window.FileNewEvent += HandleNew;
            window.FileSaveEvent += HandleSave;
            window.FileOpenEvent += HandleOpen;
        }

        
        private void UpdateContents(string contents)
        {
            var ReturnSet = new HashSet<string>();


            
        }


        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            //Check for unsaved progress before closing the window.
            if (sheet.Changed == true)
            {
                //SHould check if the user wants to save. Maybe use a message box with "Yes", "No", "Cancel" options.


                //Save(this, e);
            }
            else
            {
                //Just closes the window since nothing needed to be saved.
                window.DoClose();
            }
            
        }

        /// <summary>
        /// Handles a request to open a new window.
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }

        private void HandleSave()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Spreadsheet files (*.ss)|*.ss|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    TextWriter writer = File.CreateText(saveFileDialog1.InitialDirectory);
                    sheet.Save(writer);
                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// Handles a request to open a spreadsheet read from a file.
        /// </summary>
        private void HandleOpen()
        {

        }
    }
}
