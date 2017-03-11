using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SSGui;
using SS;

namespace SpreadsheetGUI
{
    /// <summary>
    /// This class handles updating the view for our spreadsheet.
    /// </summary>
    public partial class SpreadsheetView : Form, IAnalysisView
    {
        private int row, col;
        private String contents;


        public event Action<string, string> SetContents;

        public event Action<string> GetContents;

        public event Action FileSaveEvent;

        public event Action FileOpenEvent;

        public event Action FileCloseEvent;

        public event Action NewEvent;

        /// <summary>
        /// Constructor for the view
        /// </summary>
        public SpreadsheetView()
        {
            InitializeComponent();
            
            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(0, 0);
            CellBox.Text = "A1";
        }


        

        public string ContentsOfCell
        {
            set { ContentsBox.Text = value; }
        }

        public string Message
        {
            set { MessageBox.Show(value); }
        }

        /// <summary>
        /// Every time the selection changes, this method is called with the
        /// Spreadsheet as its parameter.
        /// </summary>
        private void displaySelection(SpreadsheetPanel ss)
        {
            ss.GetSelection(out col, out row);
            string column = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(col, 1);
            string number = (row + 1).ToString();
            string name = column + number;
            CellBox.Text = name;

            if (GetContents != null)
            {
                GetContents(name);
            }
        }

        /// <summary>
        /// Deals with changing the contents text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentsBox_KeyPress(object sender, KeyEventArgs e)
        {
            //If this passes probably need an event so that we send the contents to the controller to update the model.
            if (e.KeyCode == Keys.Enter)
            {
                if (SetContents != null)
                {
                    string column = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(col, 1);
                    string number = (row + 1).ToString();
                    string name = column + number;
                    contents = ContentsBox.Text;
                    SetContents(name, contents);
                }
            }
        }

        /// <summary>
        /// Creates a new spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Spreadsheet sheet = new Spreadsheet();
            OpenNew(sheet);
        }

        /// <summary>
        /// Opens a new spreadsheet that was saved on the harddrive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileOpenEvent != null)
            {
                FileOpenEvent();
            }
        }

        /// <summary>
        /// Deals with saving the spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileSaveEvent != null)
            {
                FileSaveEvent();
            }
           
        }

        /// <summary>
        /// Deals with the Close menu item.
        /// </summary>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileCloseEvent != null)
            {
                FileCloseEvent();
            }
        }

        /// <summary>
        /// Deals with the Help menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. To select a cell, just click on it." + Environment.NewLine + 
                "2. To change the contents of a cell please enter the desired contents in the textbox at the top. Prepend all formulae with '='. Then press enter to confirm." + 
                Environment.NewLine + "3. Other standard menu functions can be selected by clicking file in the upper left hand corner.");
        }

        /// <summary>
        /// Sets the title in the UI
        /// </summary>
        public string Title
        {
            set { Text = value; }
        }

        /// <summary>
        /// Opens a new Analysis window.
        /// </summary>
        public void OpenNew(Spreadsheet sheet)
        {
            //Spreadsheet sheet = new Spreadsheet();
            FileAnalysisApplicationContext.GetContext().RunNew(sheet);
        }

        /// <summary>
        /// Closes this window.
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        public void UpdateView(Dictionary<string, string> values)
        {
            foreach (string cellName in values.Keys)
            {
                int tempRow, tempCol;
                tempCol = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(cellName[0]);
                tempRow = Int32.Parse(cellName.Substring(1)) - 1;
                spreadsheetPanel1.SetValue(tempCol, tempRow, values[cellName]);
            }
        }

        public void SaveWarning()
        {
            DialogResult result = MessageBox.Show("Would you like to save your document before closing?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                if (FileSaveEvent != null)
                {
                    FileSaveEvent();
                }
                DoClose();
            }
            else if (result == DialogResult.No)
            {
                DoClose();
            }
            else if (result == DialogResult.Cancel)
            {
                //Dont save or close the form.
            }
        }

        
    }
}
