//Written by Corin Thummel, February 2017.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Formulas;
using Dependencies;
using System.IO;

namespace SS
{
    /// <summary>
    /// Spreadsheet class that extends the abstract methods of AbstractSpreadsheet.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private const string validpattern = @"^[a-zA-Z]+[1-9]\d*$";
        private List<Cell> CellList;
        private DependencyGraph Graph;

        /// <summary>
        /// Constructs a new Spreadsheet.
        /// </summary>
        public Spreadsheet()
        {
            CellList = new List<Cell>();
            Graph = new DependencyGraph();
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                throw new NotImplementedException();
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the IsValid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (Cell cell in CellList)
            {
                if (cell.Name != null)
                {
                    yield return cell.Name;
                }
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetCellContents(string name)
        {
            if (name == null || !Regex.IsMatch(name, validpattern))
            {
                throw new InvalidNameException();
            }

            foreach(Cell cell in CellList)
            {
                if (cell.Name == name)
                {
                    return cell.Contents;
                }
            }

            //This triggers when the name isnt in the list.
            return string.Empty;
            
        }


        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            var ReturnSet = new HashSet<string>();
            var OldDependents = new List<string>();
            bool found = false;

            if (name == null || !Regex.IsMatch(name, validpattern))
            {
                throw new InvalidNameException();
            }

            //Looks through CellList for the cell called name.
            foreach (Cell cell in CellList)
            {
                if (cell.Name == name)
                {
                    foreach (string DependName in GetCellsToRecalculate(name))
                    {
                        ReturnSet.Add(DependName);
                    }
                    

                    //Since we found that the cell already existed, replace the contents and replace any pre-exisiting dependents.
                    cell.Contents = number;


                    foreach (string dependent in Graph.GetDependents(name))
                    {
                        OldDependents.Add(dependent);
                    }
                    foreach(string t in OldDependents)
                    {
                        Graph.RemoveDependency(name, t);
                    }
                    
                    found = true;
                    break;
                }
            }

            //If the name wasnt in the list already we can add it as a new cell.
            //Fix the value in the constructor later.
            if (found == false)
            {
                Cell newcell = new Cell(name, number, number);
                CellList.Add(newcell);
                foreach (string var in GetCellsToRecalculate(name))
                {
                    ReturnSet.Add(var);
                }
            }

            return ReturnSet;

        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            var ReturnSet = new HashSet<string>();
            var OldDependents = new List<string>();
            bool found = false;

            if (text == null)
            {
                throw new ArgumentNullException();
            }

            if (name == null || !Regex.IsMatch(name, validpattern))
            {
                throw new InvalidNameException();
            }
            
            //Looks through CellList for the cell called name.
            foreach (Cell cell in CellList)
            {
                if (cell.Name == name)
                {
                    //If the replacement text is an empty string we are emptying the cell, otherwise we are changing a cell to be used.
                    if (text != string.Empty)
                    {
                        foreach (string DependName in GetCellsToRecalculate(name))
                        {
                            ReturnSet.Add(DependName);
                        }

                        

                        //Since we found that the cell already existed, replace the contents and replace any pre-exisiting dependents.
                        cell.Contents = text;
                        foreach (string dependent in Graph.GetDependents(name))
                        {
                            OldDependents.Add(dependent);
                        }
                        foreach (string t in OldDependents)
                        {
                            Graph.RemoveDependency(name, t);
                        }
                        found = true;
                        break;
                    }
                    else
                    {
                        //Exxample.
                        //A1 contains 3
                        //A2 contains 5
                        //A3 contains A1 + A2
                        //A4 contains A3*2
                        //Replacing A3 with an empty text should remove from the graph that it has dependees A1 and A2, but do nothing about A4.
                        

                        foreach (string DependName in GetCellsToRecalculate(name))
                        {
                            ReturnSet.Add(DependName);
                        }

                        CellList.Remove(cell);
                        foreach (string dep in Graph.GetDependees(cell.Name))
                        {
                            Graph.RemoveDependency(cell.Name, dep);
                        }

                        found = true;
                        break;

                    }
                    
                }
            }

            //If the name wasnt in the list already we can add it as a new cell.
            //Fix the value in the constructor later.
            if (found == false)
            {
                if (text != string.Empty)
                {
                    Cell newcell = new Cell(name, text, text);
                    CellList.Add(newcell);
                    foreach (string var in GetCellsToRecalculate(name))
                    {
                        ReturnSet.Add(var);
                    }
                }
                
            }

            return ReturnSet;
        }



        /// <summary>
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            var ReturnSet = new HashSet<string>();
            //ReturnSet.Add(name);
            var NewDependents = new List<string>();
            var OldDependees = new List<string>();
            bool found = false;
            bool InGraph = false;

            if (name == null || !Regex.IsMatch(name, validpattern))
            {
                throw new InvalidNameException();
            }

            //Looks through CellList for the cell called name.
            foreach (Cell cell in CellList)
            {
                if (cell.Name == name)
                {
                    foreach (string var in formula.GetVariables())
                    {
                        NewDependents.Add(var);
                    }

                    foreach (string DependName in GetCellsToRecalculate(name))
                    {
                        if (NewDependents.Contains(DependName))
                        {
                            throw new CircularException();
                        }

                        ReturnSet.Add(DependName);
                    }

                    foreach (string s in Graph.GetDependents(name))
                    {
                        InGraph = true;
                    }

                    if (InGraph != true)
                    {
                        foreach (string t in NewDependents)
                        {
                            Graph.AddDependency(name, t);
                        }
                    }
                    else
                    {
                        Graph.ReplaceDependents(name, NewDependents);
                    }
                    
                    //Since we found that the cell already existed, replace the contents and replace any pre-exisiting dependents.
                    cell.Contents = formula;

                    found = true;
                    break;
                }
            }

            //If the name wasnt in the list already we can add it as a new cell.
            //Fix the value in the constructor later.
            if (found == false)
            {
                Cell newcell = new Cell(name, formula, null);
                CellList.Add(newcell);
                foreach (string var in formula.GetVariables())
                {
                    if (var == name)
                    {
                        throw new CircularException();
                    }
                    else
                    {
                        Graph.AddDependency(name, var);
                    }
                }

                foreach (string var in GetCellsToRecalculate(name))
                {
                    ReturnSet.Add(var);
                }
            }
            return ReturnSet;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {

            if (name == null)
            {
                throw new ArgumentNullException();
            }

            if (!Regex.IsMatch(name, validpattern))
            {
                throw new InvalidNameException();
            }

            foreach (string s in Graph.GetDependees(name))
            {
                yield return s;
            }
        }
        //public override bool Equals(object obj)
        //{
        //    return base.Equals(obj);
        //}
        
        
        
    }
    class Cell
    {
        private string name;
        private object contents;
        private object value;

        /// <summary>
        /// Creates a new cell with a Name, Contents, and Value.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Contents"></param>
        /// <param name="Value"></param>
        public Cell(string Name, object Contents, object Value)
        {
            name = Name;
            contents = Contents;
            value = Value;
        }
        /// <summary>
        /// Returns cell's name.
        /// </summary>
        public string Name
        {
            get { return name; }
            //set { name = value; }
        }
        /// <summary>
        /// Returns cell's contents.
        /// </summary>
        public object Contents
        {
            get { return contents; }
            set { contents = value; }
        }
        ///// <summary>
        ///// Returns cell's value.
        ///// </summary>
        //public object Value
        //{
        //    get { return value; }
        //    set { value }
        //}
        
    }
}
