using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            window.NewEvent += HandleNew;
            window.FileSaveEvent += HandleSave;
            window.FileOpenEvent += HandleOpen;
        }

        
        private void UpdateContents(string name, string contents)
        {
            var ReturnPairs = new Dictionary<string, string>();

            foreach (string cell in sheet.SetContentsOfCell(name, contents))
            {
                //Need to be careful how we handle Formula Errors here.
                string tempvalue = sheet.GetCellValue(cell).ToString();
                ReturnPairs.Add(cell, tempvalue);
            }
            window.UpdateView(ReturnPairs);
            
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



                //For now it will always close but we will remove this later.
                window.DoClose();
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
            saveFileDialog1.DefaultExt = "ss";
            saveFileDialog1.Filter = "Spreadsheet files (*.ss)|*.ss|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    TextWriter writer = File.CreateText(saveFileDialog1.FileName);
                    sheet.Save(writer);
                    //myStream.Close();
                }
            }
        }

        /// <summary>
        /// Handles a request to open a spreadsheet read from a file.
        /// </summary>
        private void HandleOpen()
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.DefaultExt = "ss";
            openFileDialog1.Filter = "Spreadsheet files (*.ss)|*.ss|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    TextReader reader = File.OpenText(openFileDialog1.FileName);
                    Regex IsValid = new Regex(@"[a-zA-Z]\d+");
                    Spreadsheet newsheet = new Spreadsheet(reader, IsValid);
                    sheet = newsheet;

                    //Now it should return all non-empty cells so that the view can update the values.
                    var ReturnPairs = new Dictionary<string, string>();
                    foreach (string s in sheet.GetNamesOfAllNonemptyCells())
                    {
                        string content = sheet.GetCellValue(s).ToString();
                        ReturnPairs.Add(s, content);
                    }
                    window.UpdateView(ReturnPairs);

                }
            }
        }
    }
}
