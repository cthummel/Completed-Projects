using System;
using System.Windows.Forms;
using SS;

namespace SpreadsheetGUI
{
    static class Start
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var context = FileAnalysisApplicationContext.GetContext();
            Spreadsheet sheet = new Spreadsheet();
            FileAnalysisApplicationContext.GetContext().RunNew(sheet);
            Application.Run(context);
        }
    }
}
