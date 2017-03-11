using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using SS;
using Formulas;
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
        public Controller(IAnalysisView window, Spreadsheet ss)
        {
            this.window = window;
            this.sheet = ss;
            var setvalues = new Dictionary<string, string>();
            foreach (string s in ss.GetNamesOfAllNonemptyCells())
            {
                setvalues.Add(s, ss.GetCellValue(s).ToString());
            }
            window.UpdateView(setvalues);
            //window.FileChosenEvent += HandleFileChosen;

            window.SetContents += UpdateContents;
            window.GetContents += PassBackContents;
            window.FileCloseEvent += HandleClose;
            window.NewEvent += HandleNew;
            window.FileSaveEvent += HandleSave;
            window.FileOpenEvent += HandleOpen;
        }

        
        private void UpdateContents(string name, string contents)
        {
            var ReturnPairs = new Dictionary<string, string>();
            try
            {
                foreach (string cell in sheet.SetContentsOfCell(name, contents))
                {
                    //Need to be careful how we handle Formula Errors here.
                    if (sheet.GetCellValue(cell).GetType() == typeof(FormulaError))
                    {
                        ReturnPairs.Add(cell, "Formula Error");
                    }
                    else
                    {
                        string tempvalue = sheet.GetCellValue(cell).ToString();
                        ReturnPairs.Add(cell, tempvalue);
                    }
                    
                }
                window.UpdateView(ReturnPairs);
            }
            catch (CircularException ex)
            {
                //MessageBox goes here saying that circular exceptions are bad.
                window.Message = ex.Message;
            }
            catch
            {
                //Would be nice to output the error messages that ive baked into each exception in spreadsheet but not sure how to do it.
                window.Message = "An Error has occured.";
            }            
        }

        private void PassBackContents(string name)
        {
            if (sheet.GetCellContents(name).GetType() == typeof(Formula))
            {
                window.ContentsOfCell = "=" + sheet.GetCellContents(name).ToString();
            }
            else
            {
                window.ContentsOfCell = sheet.GetCellContents(name).ToString();
            }
        }

        /// <summary>
        /// Handles a request to close the window
        /// </summary>
        private void HandleClose()
        {
            //Check for unsaved progress before closing the window.
            if (sheet.Changed == true)
            {
                window.SaveWarning();
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
            Spreadsheet sheet = new Spreadsheet();
            window.OpenNew(sheet);
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
                    window.Title = saveFileDialog1.FileName;
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

                    window.OpenNew(newsheet);
                    //// Now it should return all non-empty cells so that the view can update the values.
                    //var ReturnPairs = new Dictionary<string, string>();
                    //foreach (string s in newsheet.GetNamesOfAllNonemptyCells())
                    //{
                    //    string content = newsheet.GetCellValue(s).ToString();
                    //    ReturnPairs.Add(s, content);
                    //}
                    //window.Title = openFileDialog1.FileName;
                    
                    //window.UpdateView(ReturnPairs);
                }
            }
        }
    }
}
