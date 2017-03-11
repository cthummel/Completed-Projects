﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SSGui;

namespace SpreadsheetGUI
{
    /// <summary>
    /// This class handles updating the view for our spreadsheet.
    /// </summary>
    public partial class SpreadsheetView : Form, IAnalysisView
    {
        private int row, col;
        private String contents;

        /// <summary>
        /// Constructor for the view
        /// </summary>
        public SpreadsheetView()
        {
            InitializeComponent();
            
            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(0, 0);
        }


        public event Action<string, string> SetContents;

        public event Action<string> GetContents;

        public event Action FileSaveEvent;

        public event Action FileOpenEvent;

        public event Action FileCloseEvent;

        public event Action NewEvent;

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
            if (GetContents != null)
            {
                GetContents(name);
            }

            //ss.GetValue(col, row, out contents);
            //ContentsBox.Text = contents;

            //ss.SetValue(col, row, contents);
            //ss.SetValue(col, row, DateTime.Now.ToLocalTime().ToString("T"));
            //MessageBox.Show("Selection: column " + col + " row " + row + " value " + value);
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
            OpenNew();
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
        public void OpenNew()
        {
            FileAnalysisApplicationContext.GetContext().RunNew();
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