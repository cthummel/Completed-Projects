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

        //int CharCount { set; }

        

        //void DoClose();

        //void OpenNew();
    }
}
