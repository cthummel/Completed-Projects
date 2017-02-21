using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using SS;

namespace PS6Tests
{
    [TestClass]
    public class PS6Tests
    {
        private const string A1 = "A1";
        private const string A2 = "A2";
        private const string A3 = "A3";
        private const string A4 = "A4";
        private const string B1 = "B1";
        private const string B2 = "B2";
        private const string B3 = "B3";
        private const string B4 = "B4";



        [TestMethod]
        public void ValueTest1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(A1, 10.ToString());
            sheet.SetContentsOfCell(A2, 20.ToString());
            sheet.SetContentsOfCell(A3, "=A1+A2");
            sheet.SetContentsOfCell(A4, "=A1+A3");
            sheet.SetContentsOfCell("A5", "=A3+A4");
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));
            Assert.AreEqual(40.0, sheet.GetCellValue(A4));
            Assert.AreEqual(70.0, sheet.GetCellValue("A5"));

        }
        /// <summary>
        /// Replaces a formula with a double that has a different value than before. Should update properly.
        /// </summary>
        [TestMethod]
        public void ValueTest2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(A1, 10.ToString());
            sheet.SetContentsOfCell(A2, 20.ToString());
            sheet.SetContentsOfCell(A3, "=A1+A2");
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));
            sheet.SetContentsOfCell(A4, "=A1+A3");
            Assert.AreEqual(40.0, sheet.GetCellValue(A4));
            sheet.SetContentsOfCell("A5", "=A3+A4");
            Assert.AreEqual(70.0, sheet.GetCellValue("A5"));
            sheet.SetContentsOfCell(A3, "50");
            Assert.AreEqual(50.0, sheet.GetCellValue(A3));
            Assert.AreEqual(110.0, sheet.GetCellValue("A5"));

        }
        /// <summary>
        /// Spreadsheet of only formulae should have values that are all FormulaErrors.
        /// </summary>
        [TestMethod]
        public void ValueTest3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            
            sheet.SetContentsOfCell(A3, "=A1+A2");
            Assert.AreEqual(new FormulaError("One or more variables have an undefined value."), sheet.GetCellValue(A3));
            sheet.SetContentsOfCell(A4, "=A1+A3");
            Assert.AreEqual(new FormulaError("One or more variables have an undefined value."), sheet.GetCellValue(A4));
            sheet.SetContentsOfCell("A5", "=A3+A4");
            Assert.AreEqual(new FormulaError("One or more variables have an undefined value."), sheet.GetCellValue("A5"));
            Assert.AreEqual(new FormulaError("One or more variables have an undefined value."), sheet.GetCellValue(A3));
            Assert.AreEqual(new FormulaError("One or more variables have an undefined value."), sheet.GetCellValue("A5"));

        }
        /// <summary>
        /// Checking removing Formula Errors.
        /// </summary>
        [TestMethod]
        public void ValueTest4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell(A3, "=A1+A2");
            Assert.AreEqual(new FormulaError("One or more variables have an undefined value."), sheet.GetCellValue(A3));
            sheet.SetContentsOfCell(A1, 10.ToString());
            Assert.AreEqual(new FormulaError("One or more variables have an undefined value."), sheet.GetCellValue(A3));
            sheet.SetContentsOfCell(A2, 20.ToString());
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));

        }
    }
}
