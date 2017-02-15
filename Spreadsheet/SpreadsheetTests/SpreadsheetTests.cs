using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using SS;
using Formulas;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        private const string A1 = "A1";
        private const string A2 = "A2";
        private const string A3 = "A3";
        private const string A4 = "A4";
        private const string A5 = "A5";

        private const string B1 = "B1";
        private const string B2 = "B2";
        private const string B3 = "B3";
        private const string B4 = "B4";
        private const string B5 = "B5";

        /// <summary>
        /// Tests GetNamesOfAllNonemptyCells on an empty sheet.
        /// </summary>
        [TestMethod]
        public void EmptySheet1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            var emptylist = new List<string>();
            var sheetlist = new List<string>();
            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                sheetlist.Add(s);
            }
            Assert.AreEqual(emptylist.Count, sheetlist.Count);

        }

        /// <summary>
        /// Tests GetCellContents on empty sheet.
        /// </summary>
        [TestMethod]
        public void EmptySheet2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            string A1cont = sheet.GetCellContents(A1).ToString();
            string A2cont = sheet.GetCellContents(A2).ToString();
            string A3cont = sheet.GetCellContents(A3).ToString();
            string A4cont = sheet.GetCellContents(A4).ToString();
            string A5cont = sheet.GetCellContents(A5).ToString();

            Assert.AreEqual(string.Empty, A1cont);
            Assert.AreEqual(string.Empty, A2cont);
            Assert.AreEqual(string.Empty, A3cont);
            Assert.AreEqual(string.Empty, A4cont);
            Assert.AreEqual(string.Empty, A5cont);

        }

        /// <summary>
        /// Tests SetCellContents with double on empty sheet.
        /// </summary>
        [TestMethod]
        public void EmptySheet3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula A3formula = new Formula("A1 * 2");

            sheet.SetCellContents(A1, 10);
            sheet.SetCellContents(A2, "Hey Guys!");
            sheet.SetCellContents(A3, A3formula);

            double A1cont = (double)sheet.GetCellContents(A1);
            string A2cont = (string)sheet.GetCellContents(A2);
            Formula A3cont = (Formula)sheet.GetCellContents(A3);
            //double A4cont = (double)sheet.GetCellContents(A4);
            //double A5cont = (double)sheet.GetCellContents(A5);

            Assert.AreEqual(10, A1cont);
            Assert.AreEqual("Hey Guys!", A2cont);
            Assert.AreEqual(A3formula, A3cont);
            //Assert.AreEqual(string.Empty, A4cont);
            //Assert.AreEqual(string.Empty, A5cont);

        }
        /// <summary>
        /// Tests multiple SetCellContents using only doubles
        /// </summary>
        [TestMethod]
        public void BasicSheet1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            double A1cont;


            sheet.SetCellContents(A1, 10);
            A1cont = (double)sheet.GetCellContents(A1);
            Assert.AreEqual(10, A1cont);

            sheet.SetCellContents(A1, 20);
            A1cont = (double)sheet.GetCellContents(A1);
            Assert.AreEqual(20, A1cont);

            sheet.SetCellContents(A1, 30);
            A1cont = (double)sheet.GetCellContents(A1);
            Assert.AreEqual(30, A1cont);

            sheet.SetCellContents(A1, 40);
            A1cont = (double)sheet.GetCellContents(A1);
            Assert.AreEqual(40, A1cont);

        }
    }
}
