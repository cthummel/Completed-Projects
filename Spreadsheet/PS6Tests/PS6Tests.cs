using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SS;
using System.IO;

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
        /// <summary>
        /// Checking removing Formula Errors.
        /// </summary>
        [TestMethod]
        public void ValueTest5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(A1, 10.ToString());
            sheet.SetContentsOfCell(A2, 20.ToString());
            sheet.SetContentsOfCell(A3, "=A1+A2");
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));

            sheet.SetContentsOfCell(A2, "hello");
            Assert.AreEqual(new FormulaError("One or more variables have an undefined value."), sheet.GetCellValue(A3));

            sheet.SetContentsOfCell(A2, 20.ToString());
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));
        }
        /// <summary>
        /// Checking removing Formula Errors.
        /// </summary>
        [TestMethod]
        public void ValueTest6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(A1, 10.ToString());
            sheet.SetContentsOfCell(A2, 20.ToString());
            sheet.SetContentsOfCell(A3, "=A1+A2");
            sheet.SetContentsOfCell(B1, 50.ToString());
            sheet.SetContentsOfCell(B2, 70.ToString());
            sheet.SetContentsOfCell(B3, "=B1+B2");
            sheet.SetContentsOfCell("C1", 90.ToString());
            sheet.SetContentsOfCell("C2", 210.ToString());
            sheet.SetContentsOfCell("C3", "=C1+C2");
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));

            sheet.SetContentsOfCell(A2, "hello");
            Assert.AreEqual(new FormulaError("One or more variables have an undefined value."), sheet.GetCellValue(A3));

            sheet.SetContentsOfCell(A2, 20.ToString());
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));
            TextWriter writer = File.CreateText("spreadsheettest.txt");
            sheet.Save(writer);
            writer.Close();
        }

        [TestMethod]
        public void ConstructTest1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("CAT", 10.ToString());
            sheet.SetContentsOfCell("DOG", 20.ToString());
            sheet.SetContentsOfCell("CAT AND DOG", "=CAT + DOG");
            Assert.AreEqual(30.0, sheet.GetCellValue("CAT AND DOG"));
            sheet.SetContentsOfCell("CAT AND DOG", "=DOG - CAT");
            Assert.AreEqual(10.0, sheet.GetCellValue("CAT AND DOG"));
        }

        [TestMethod]
        public void ConstructTest2()
        {
            Regex TestValid = new Regex(@"^[a-zA-Z]+[1-9]\d*$");
            AbstractSpreadsheet sheet = new Spreadsheet(TestValid);
            sheet.SetContentsOfCell(A1, 10.ToString());
            sheet.SetContentsOfCell(A2, 20.ToString());
            sheet.SetContentsOfCell(A3, "=A1+A2");
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));

            sheet.SetContentsOfCell(A2, "hello");
            Assert.AreEqual(new FormulaError("One or more variables have an undefined value."), sheet.GetCellValue(A3));

            sheet.SetContentsOfCell(A2, 20.ToString());
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void ConstructTest3()
        {
            Regex TestValid = new Regex(@"^[a-zA-Z]+[1-9]\d*$");
            AbstractSpreadsheet sheet = new Spreadsheet(TestValid);
            sheet.SetContentsOfCell("CAT", 10.ToString());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructTest4()
        {
            Regex TestValid = new Regex(@"^[a-zA-Z]+[1-9]\d*$");
            AbstractSpreadsheet sheet = new Spreadsheet(TestValid);
            sheet.SetContentsOfCell(A1, null);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void ConstructTest5()
        {
            Regex TestValid = new Regex(@"^[a-zA-Z]+[1-9]\d*$");
            AbstractSpreadsheet sheet = new Spreadsheet(TestValid);
            sheet.SetContentsOfCell(null, 10.ToString());
        }
        [TestMethod]
        public void ConstructTest6()
        {
            AbstractSpreadsheet sheet1 = new Spreadsheet();
            sheet1.SetContentsOfCell(A1, 10.ToString());
            sheet1.SetContentsOfCell(A2, 20.ToString());
            sheet1.SetContentsOfCell(A3, "=A1+A2");

            TextWriter writer = File.CreateText("spreadsheettest.txt");
            sheet1.Save(writer);
            writer.Close();

            Regex TestValid = new Regex(@"^[a-zA-Z]+[1-9]\d*$");
            TextReader source = File.OpenText(@"C:\Users\Corin Thummel\Source\Repos\spreadsheet\Spreadsheet\PS6Tests\bin\Debug\spreadsheettest.txt");
            AbstractSpreadsheet sheet = new Spreadsheet(source, TestValid);
            Assert.AreEqual(10.0, sheet.GetCellValue(A1));
            Assert.AreEqual(20.0, sheet.GetCellValue(A2));
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));
            source.Close();


        }
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetVersionException))]
        public void ConstructTest7()
        {
            AbstractSpreadsheet sheet1 = new Spreadsheet();
            sheet1.SetContentsOfCell("CAT", 10.ToString());
            sheet1.SetContentsOfCell("DOG", 20.ToString());
            sheet1.SetContentsOfCell("CAT AND DOG", "=CAT + DOG");

            TextWriter writer = File.CreateText("spreadsheettest.txt");
            sheet1.Save(writer);
            writer.Close();

            Regex TestValid = new Regex(@"^[a-zA-Z]+[1-9]\d*$");
            TextReader source = File.OpenText(@"C:\Users\Corin Thummel\Source\Repos\spreadsheet\Spreadsheet\PS6Tests\bin\Debug\spreadsheettest.txt");
            AbstractSpreadsheet sheet = new Spreadsheet(source, TestValid);
            Assert.AreEqual(10.0, sheet.GetCellValue(A1));
            Assert.AreEqual(20.0, sheet.GetCellValue(A2));
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));
            source.Close();
        }
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void ConstructTest8()
        {
            Regex TestValid = new Regex(@"^[a-zA-Z]+[1-9]\d*$");
            TextReader source = File.OpenText(@"C:\Users\Corin Thummel\Source\Repos\spreadsheet\Spreadsheet\PS6Tests\bin\Debug\spreadsheettest1.txt");
            AbstractSpreadsheet sheet = new Spreadsheet(source, TestValid);
            Assert.AreEqual(10.0, sheet.GetCellValue(A1));
            Assert.AreEqual(20.0, sheet.GetCellValue(A2));
            Assert.AreEqual(30.0, sheet.GetCellValue(A3));
            source.Close();
        }

    }
}
