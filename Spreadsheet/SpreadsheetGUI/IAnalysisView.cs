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


        event Action<string, string> SetContents;

        event Action FileSaveEvent;

        event Action FileOpenEvent;

        event Action FileCloseEvent;

        event Action NewEvent;

        //string ContentsOfCell { set; }

        //int RowSelection { set; }

        //int ColumnSelection { set; }

        string Title { set; }

        void UpdateView(HashSet<string>);

        void DoClose();

        void OpenNew();
    }
}
