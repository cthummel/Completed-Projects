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
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySheet4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, 200);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySheet5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("900", 200);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySheet6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, "Hello");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySheet7()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("900", "World");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySheet8()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form = new Formula("A10");
            sheet.SetCellContents(null, form);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void EmptySheet9()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form = new Formula("A10");
            sheet.SetCellContents("900", form);
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

            var contentslist = new List<string>();
            contentslist.Add(A1);
            var sheetlist = new List<string>();
            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                sheetlist.Add(s);
            }
            Assert.AreEqual(contentslist.ElementAt(0), sheetlist.ElementAt(0));

        }
        [TestMethod]
        public void BasicSheet2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            string A1cont;


            sheet.SetCellContents(A1, "A");
            A1cont = (string)sheet.GetCellContents(A1);
            Assert.AreEqual("A", A1cont);

            sheet.SetCellContents(A1, "B");
            A1cont = (string)sheet.GetCellContents(A1);
            Assert.AreEqual("B", A1cont);

            sheet.SetCellContents(A1, "C");
            A1cont = (string)sheet.GetCellContents(A1);
            Assert.AreEqual("C", A1cont);

            sheet.SetCellContents(A1, "D");
            A1cont = (string)sheet.GetCellContents(A1);
            Assert.AreEqual("D", A1cont);

            var contentslist = new List<string>();
            contentslist.Add(A1);
            var sheetlist = new List<string>();
            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                sheetlist.Add(s);
            }
            Assert.AreEqual(contentslist.ElementAt(0), sheetlist.ElementAt(0));

        }
        [TestMethod]
        public void BasicSheet3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            string A1cont;
            var contentslist = new List<string>();
            var sheetlist = new List<string>();

            sheet.SetCellContents(A1, "A");
            A1cont = (string)sheet.GetCellContents(A1);
            Assert.AreEqual("A", A1cont);

            sheet.SetCellContents(A1, "B");
            A1cont = (string)sheet.GetCellContents(A1);
            Assert.AreEqual("B", A1cont);

            sheet.SetCellContents(A1, "");
            A1cont = (string)sheet.GetCellContents(A1);
            Assert.AreEqual(string.Empty, A1cont);


            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                sheetlist.Add(s);
            }
            Assert.AreEqual(contentslist.Count, sheetlist.Count);

            sheet.SetCellContents(A1, "D");
            A1cont = (string)sheet.GetCellContents(A1);
            Assert.AreEqual("D", A1cont);

            contentslist = new List<string>();
            contentslist.Add(A1);
            sheetlist = new List<string>();
            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                sheetlist.Add(s);
            }
            Assert.AreEqual(contentslist.ElementAt(0), sheetlist.ElementAt(0));

        } 


        // Tried changing to set contents of cell
        [TestMethod]
        public void BasicSheet4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            double A1cont;
            double A2cont;
            Formula A3cont;
            string A3content;

            var contentslist = new List<string>();
            var sheetlist = new List<string>();

            sheet.SetContentsOfCell(A1, "10");
            A1cont = (double)sheet.GetCellContents(A1);
            Assert.AreEqual(10, A1cont);
            contentslist.Add(A1);

            sheet.SetContentsOfCell(A2, "20");
            A2cont = (double)sheet.GetCellContents(A2);
            Assert.AreEqual(20, A2cont);
            contentslist.Add(A2);

            Formula form = new Formula("A1 + A2");
            sheet.SetContentsOfCell(A3, "form");
            A3cont = (Formula)(sheet.GetCellContents(A3));
            contentslist.Add(A3);

            sheet.SetContentsOfCell(A3, string.Empty);
            A3content = (string)sheet.GetCellContents(A3);
            Assert.AreEqual(string.Empty, A3content);
            contentslist.Remove(A3);

            //contentslist.Add(A1);
            sheetlist = new List<string>();
            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                sheetlist.Add(s);
            }
            for (int i = 0; i < sheetlist.Count; i++)
            {
                Assert.AreEqual(contentslist.ElementAt(i), sheetlist.ElementAt(i));
            }
        }

        

        /// <summary>
        /// Replacing formula cells with new formula cells. Mostly looking internally at graph.
        /// </summary>
        [TestMethod]
        public void FormulaSheet1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();


            Formula form1 = new Formula("A2 / A3");
            Formula form2 = new Formula("A5 + A3");
            Formula form3 = new Formula("A4 * A4");
            Formula form4 = new Formula("A5 * A5");

            sheet.SetCellContents(A1, form1);
            sheet.SetCellContents(A2, form2);
            sheet.SetCellContents(A3, form3);
            sheet.SetCellContents(A4, form4);


            Assert.IsFalse(false);

        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void FormulaSheet2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form1 = new Formula("A1 / A3");
            sheet.SetCellContents(A1, form1);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void FormulaSheet3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form1 = new Formula("A2 / A3");
            Formula form2 = new Formula("A1 / A3");
            sheet.SetCellContents(A1, form1);
            sheet.SetCellContents(A1, form2);
        }
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void FormulaSheet4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form1 = new Formula("A2 / A3");
            Formula form3 = new Formula("A1 / A4");
            sheet.SetCellContents(A1, form1);
            sheet.SetCellContents(A3, form3);

        }
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void FormulaSheet5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form1 = new Formula("A1");
            Formula form2 = new Formula("A2");
            Formula form3 = new Formula("A3");

            sheet.SetCellContents(A1, form2);
            sheet.SetCellContents(A2, form3);
            sheet.SetCellContents(A3, form1);

        }
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void FormulaSheet6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form1 = new Formula("A1");
            Formula form2 = new Formula("A2");
            Formula form3 = new Formula("A3");
            Formula form4 = new Formula("A4");

            sheet.SetCellContents(A1, form2);
            sheet.SetCellContents(A2, form3);
            sheet.SetCellContents(A3, form4);
            sheet.SetCellContents(A3, form1);

        }
        [TestMethod]

        public void FormulaSheet7()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form1 = new Formula("A1");
            Formula form2 = new Formula("A2");
            Formula form3 = new Formula("A3");
            Formula form4 = new Formula("A4");

            sheet.SetCellContents(A1, form2);
            sheet.SetCellContents(A1, form3);
            sheet.SetCellContents(A1, form4);

            Formula check = (Formula)sheet.GetCellContents(A1);
            string checkstring = check.ToString();

            Assert.AreEqual(checkstring, "A4");
        }

        [TestMethod]
        public void FormulaSheet8()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();


            Formula form1 = new Formula("B3 + B1");
            Formula form2 = new Formula("A1 + B2");
            Formula form3 = new Formula("A3 * A4");
            //Formula form4 = new Formula("A5 * A5");

            sheet.SetCellContents(A1, form1);
            sheet.SetCellContents(A2, form2);
            sheet.SetCellContents(B3, form3);
            sheet.SetCellContents(B3, 10);
            sheet.SetCellContents(B3, form3);
            sheet.SetCellContents(B3, "Nicely removed");


            Assert.IsFalse(false);

        }

        [TestMethod]
        public void MixedTest1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula form1 = new Formula("A1");
            Formula form2 = new Formula("A2");
            Formula form3 = new Formula("A3");
            Formula form4 = new Formula("A4");

            sheet.SetCellContents(A1, 10);
            sheet.SetCellContents(A1, "Hello!");
            sheet.SetCellContents(A1, 20);
            sheet.SetCellContents(A1, "There");
            sheet.SetCellContents(A1, form2);
            sheet.SetCellContents(A1, form3);
            sheet.SetCellContents(A1, 10);
            sheet.SetCellContents(A1, form4);

            Formula check = (Formula)sheet.GetCellContents(A1);
            string checkstring = check.ToString();

            Assert.AreEqual(checkstring, "A4");
        }

        [TestMethod]
        public void MixedTest2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            var Set1 = new HashSet<string>();

            Formula form1 = new Formula("A1");
            Formula form2 = new Formula("A2");
            Formula form3 = new Formula("A3");
            Formula form4 = new Formula("A4");
            Formula formb1 = new Formula("B1 + A1");
            Formula formb2 = new Formula("B2 + A2");
            Formula formb3 = new Formula("B3 + A3");
            Formula formb4 = new Formula("B4 * A4");

            foreach (string s in sheet.SetCellContents(A1, 10))
            {
                Set1.Add(s);
            }
            Assert.AreEqual(Set1.ElementAt(0), A1);
            Set1 = new HashSet<string>();

            foreach (string s in sheet.SetCellContents(A1, "moose"))
            {
                Set1.Add(s);
            }
            Assert.AreEqual(Set1.ElementAt(0), A1);
            Set1 = new HashSet<string>();

            foreach (string s in sheet.SetCellContents(A1, form2))
            {
                Set1.Add(s);
            }
            Assert.AreEqual(Set1.ElementAt(0), A1);
            Set1 = new HashSet<string>();

            foreach (string s in sheet.SetCellContents(A1, 1))
            {
                Set1.Add(s);
            }
            Assert.AreEqual(Set1.ElementAt(0), A1);
            Set1 = new HashSet<string>();

            sheet.SetCellContents(A2, "Hello!");
            sheet.SetCellContents(A2, "There");
            sheet.SetCellContents(A2, form3);
            sheet.SetCellContents(A2, formb4);

            sheet.SetCellContents(B1, 100);
            sheet.SetCellContents(B1, "cats");
            sheet.SetCellContents(B1, 10);

            sheet.SetCellContents(B2, form4);
            sheet.SetCellContents(B2, formb4);
            sheet.SetCellContents(B2, formb1);

            double check1 = (double)sheet.GetCellContents(A1);
            Formula check2 = (Formula)sheet.GetCellContents(A2);
            double check3 = (double)sheet.GetCellContents(B1);
            Formula check4 = (Formula)sheet.GetCellContents(B2);

            string checkstring2 = check2.ToString();
            string checkstring4 = check4.ToString();

            Assert.AreEqual(check1, 1);
            Assert.AreEqual(checkstring2, "B4 * A4");
            Assert.AreEqual(check3, 10);
            Assert.AreEqual(checkstring4, "B1 + A1");
        }

    

        [TestMethod]
        public void MixedTest4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            Formula formb1 = new Formula("B1 + A1");
            Formula formb2 = new Formula("B2 + A2");
            Formula formb3 = new Formula("B3 + A3");
            Formula formb4 = new Formula("B4 * A4");

            sheet.SetContentsOfCell(B1, "100");
            sheet.SetContentsOfCell(B1, "cats");
            sheet.SetContentsOfCell(B1, "10");
            sheet.SetContentsOfCell(B1, "formb2");
            sheet.SetContentsOfCell(B1, "20");
            sheet.SetContentsOfCell(B1, "formb3");
            sheet.SetContentsOfCell(B1, "10");
        }

        
    } 
}
