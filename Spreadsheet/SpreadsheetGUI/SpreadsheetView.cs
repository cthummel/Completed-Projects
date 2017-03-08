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
        private int row, col;
        private String contents;

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
        /// Spreadsheet as its parameter.
        /// </summary>
        private void displaySelection(SpreadsheetPanel ss)
        {
            ss.GetSelection(out col, out row);
            ss.SetValue(col, row, contents);

            //ss.SetValue(col, row, DateTime.Now.ToLocalTime().ToString("T"));

            ss.GetValue(col, row, out contents);
            //MessageBox.Show("Selection: column " + col + " row " + row + " value " + value);
            
        }

       
        /// <summary>
        /// Deals with changing the contents text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentsBox_KeyDown(object sender, KeyEventArgs e)
        {
            //If this passes probably need an event so that we send the contents to the controller to update the model.
            if (e.KeyCode == Keys.Enter)
            {
                contents = ContentsBox.Text;
                
            }

        }
        /// <summary>
        /// Creates a new spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Opens a new spreadsheet that was saved on the harddrive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Deals with saving the spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
        /// <summary>
        /// Deals with the Close menu
        /// </summary>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Deals with the Help menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
