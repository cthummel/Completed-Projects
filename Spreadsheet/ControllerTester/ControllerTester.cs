using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using SS;
using SSGui;
using SpreadsheetGUI;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ControllerTester
{
    [TestClass]
    public class ControllerTester
    {
        [TestMethod]
        public void TestGetValue1()
        {
            Spreadsheet ss = new Spreadsheet();
            SpreadsheetView view = new SpreadsheetView();
            Controller controller = new Controller(view, ss);

            ss.SetContentsOfCell("a1", "42");
            ss.SetContentsOfCell("a2", "24");
            ss.SetContentsOfCell("a3", "=a1 + a2");
            Assert.AreEqual(ss.GetCellValue("a3"), 66.0);
        }

        [TestMethod]
        public void TestGetValue2()
        {
            Spreadsheet ss = new Spreadsheet();
            SpreadsheetView view = new SpreadsheetView();
            Controller controller = new Controller(view, ss);

            ss.SetContentsOfCell("a1", "15003");
            ss.SetContentsOfCell("a2", "299");
            ss.SetContentsOfCell("a3", "=a1 + a2");
            Assert.AreEqual(ss.GetCellValue("a3"), 15302.0);

        }

        [TestMethod]
        public void TestGetContents1()
        {
            Spreadsheet ss = new Spreadsheet();
            SpreadsheetView view = new SpreadsheetView();
            Controller controller = new Controller(view, ss);

            ss.SetContentsOfCell("a1", "42");
            ss.SetContentsOfCell("a2", "24");
            ss.SetContentsOfCell("a3", "=a1 + a2");
            Assert.AreEqual(ss.GetCellContents("a3").ToString(), "A1 + A2");
        }

        [TestMethod]
        public void TestGetContents2()
        {
            Spreadsheet ss = new Spreadsheet();
            SpreadsheetView view = new SpreadsheetView();
            Controller controller = new Controller(view, ss);

            ss.SetContentsOfCell("a1", "15003");
            ss.SetContentsOfCell("a2", "299");
            ss.SetContentsOfCell("a3", "=a1 + a2");
            Assert.AreEqual(ss.GetCellContents("a3").ToString(), "A1 + A2");
        }

        [TestMethod]
        public void TestGetContents3()
        {
            Spreadsheet ss = new Spreadsheet();
            SpreadsheetView view = new SpreadsheetView();
            Controller controller = new Controller(view, ss);

            ss.SetContentsOfCell("a1", "15003");
            ss.SetContentsOfCell("a1", "1777");
            Assert.AreEqual(ss.GetCellContents("a1"), 1777.0);
        }

        [TestMethod]
        public void TestChanged1()
        {
            Spreadsheet ss = new Spreadsheet();
            SpreadsheetView view = new SpreadsheetView();
            Controller controller = new Controller(view, ss);

            Assert.IsFalse(ss.Changed);
            ss.SetContentsOfCell("a1", "15003");
            ss.SetContentsOfCell("a1", "1777");
            Assert.AreEqual(ss.GetCellContents("a1"), 1777.0);
            Assert.IsTrue(ss.Changed);
        }

        [TestMethod]
        public void TestGetNames1()
        {
            Spreadsheet ss = new Spreadsheet();
            SpreadsheetView view = new SpreadsheetView();
            Controller controller = new Controller(view, ss);

            ss.SetContentsOfCell("a1", "15003");
            ss.SetContentsOfCell("a1", "1777");
            ss.SetContentsOfCell("a2", "24");
            HashSet<string> comparison = new HashSet<string>();
            comparison.Add("A1");
            comparison.Add("A2");

            IEnumerable<string> en = ss.GetNamesOfAllNonemptyCells();
            foreach (string s in en)
            {
                Assert.IsTrue(comparison.Contains(s));
            }
        }

        [TestMethod]
        public void TestGetNames2()
        {
            Spreadsheet ss = new Spreadsheet();
            SpreadsheetView view = new SpreadsheetView();
            Controller controller = new Controller(view, ss);

            ss.SetContentsOfCell("a1", "15003");
            ss.SetContentsOfCell("a1", "1777");
            ss.SetContentsOfCell("a2", "24");
            HashSet<string> comparison = new HashSet<string>();
            comparison.Add("A3");
            comparison.Add("A4");

            IEnumerable<string> en = ss.GetNamesOfAllNonemptyCells();
            foreach (string s in en)
            {
                Assert.IsFalse(comparison.Contains(s));
            }
        }

    }
}