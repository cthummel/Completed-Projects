using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controllable interface of AnalysisWindow
    /// </summary>
    public interface IAnalysisView
    {
        

        event Action<string> SetContents;

        event Action FileSaveEvent;

        event Action FileOpenEvent;

        event Action FileCloseEvent;

        event Action FileNewEvent;

        int CharCount { set; }

        int LineCount { set; }

        string SearchString { set; }

        int SubstringCount { set; }

        int WordCount { set; }

        string Title { set; }

        string Message { set; }

        void DoClose();

        void OpenNew();
    }
}
