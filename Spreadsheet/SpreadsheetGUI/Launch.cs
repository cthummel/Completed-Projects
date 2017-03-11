﻿using System;
using System.Windows.Forms;
using SSGui;

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
            FileAnalysisApplicationContext.GetContext().RunNew();
            Application.Run(context);
        }
    }
}