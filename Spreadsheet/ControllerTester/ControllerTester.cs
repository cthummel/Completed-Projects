﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using SS;
using SSGui;
using SpreadsheetGUI;
using System.Windows.Forms;

namespace ControllerTester
{
    [TestClass]
    public class ControllerTester
    {
        [TestMethod]
        public void TestMethod1()
        {
            SpreadsheetView view = new SpreadsheetView();
            Controller controller = new Controller(view);

            //view.SetContents("A1", "10");

        }
    }
}
