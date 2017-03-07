using System;
using System.Windows.Forms;
using SSGui;

namespace SpreadsheetGUI
{
    /// <summary>
    /// This class handles updating the view for our spreadsheet.
    /// </summary>
    public partial class SpreadsheetView : Form
    {
        
        /// <summary>
        /// Constructor for the view
        /// </summary>
        public SpreadsheetView()
        {
            InitializeComponent();

            // This an example of registering a method so that it is notified when
            // an event happens.  The SelectionChanged event is declared with a
            // delegate that specifies that all methods that register with it must
            // take a SpreadsheetPanel as its parameter and return nothing.  So we
            // register the displaySelection method below.

            // This could also be done graphically in the designer, as has been
            // demonstrated in class.
            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(0, 0);
        }
        /// <summary>
        /// Every time the selection changes, this method is called with the
        /// Spreadsheet as its parameter.  We display the current time in the cell.
        /// </summary>
        private void displaySelection(SpreadsheetPanel ss)
        {
            int row, col;
            String contents;
            ss.GetSelection(out col, out row);

            ss.GetValue(col, row, out contents);

            //ss.SetValue(col, row, DateTime.Now.ToLocalTime().ToString("T"));

            ss.GetValue(col, row, out contents);
                //MessageBox.Show("Selection: column " + col + " row " + row + " value " + value);
            
        }

        /// <summary>
        /// Deals with the Close menu
        /// </summary>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ContentsBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
