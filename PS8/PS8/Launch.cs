using System;
using System.Windows.Forms;

namespace PS8
{
    static class Launch
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BoggleClient window = new BoggleClient();
            new Controller(window);
            Application.Run(window);
        }
    }
}
