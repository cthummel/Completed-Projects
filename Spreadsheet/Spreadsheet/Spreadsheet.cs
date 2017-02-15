//Written by Corin Thummel, February 2017.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Formulas;
using Dependencies;

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
        public override ISet<string> SetCellContents(string name, double number)
        {
            var ReturnSet = new HashSet<string>();
            var NewDependents = new List<string>();
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
                        //if (DependName == name)
                        //{
                        //    throw new CircularException();
                        //}
                        //else
                        //{
                            ReturnSet.Add(DependName);
                        //}
                    }
                    NewDependents.Add(name);

                    //Since we found that the cell already existed, replace the contents and replace any pre-exisiting dependents.
                    cell.Contents = number;
                    foreach (string dependee in Graph.GetDependees(name))
                    {
                        Graph.RemoveDependency(name, dependee);
                    }
                    //Graph.ReplaceDependents(name, NewDependents);
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
                //Graph.AddDependency(name, name);
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
        public override ISet<string> SetCellContents(string name, string text)
        {
            var ReturnSet = new HashSet<string>();
            var NewDependents = new List<string>();
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
                    //If the replacement text is an empty string we are emptying the cell, otherwise we are changing a cell to be used.
                    if (text != string.Empty)
                    {
                        foreach (string DependName in GetCellsToRecalculate(name))
                        {
                            if (DependName == name)
                            {
                                throw new CircularException();
                            }
                            else
                            {
                                ReturnSet.Add(DependName);
                            }
                        }
                        NewDependents.Add(name);

                        //Since we found that the cell already existed, replace the contents and replace any pre-exisiting dependents.
                        cell.Contents = text;
                        Graph.ReplaceDependents(name, NewDependents);
                        found = true;
                        break;
                    }
                    else
                    {
                        //Exxample.
                        //A1 contains 3
                        //A2 contains 5
                        //A3 contains A1 + A2
                        //Replacing A3 with an empty text should remove
                        CellList.Remove(cell);


                        

                    }
                    
                }
            }

            //If the name wasnt in the list already we can add it as a new cell.
            //Fix the value in the constructor later.
            if (found == false)
            {
                Cell newcell = new Cell(name, text, text);
                CellList.Add(newcell);
                Graph.AddDependency(name, name);
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
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            var ReturnSet = new HashSet<string>();
            var NewDependees = new List<string>();
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
                        if (DependName == name)
                        {
                            throw new CircularException();
                        }
                        else
                        {
                            ReturnSet.Add(DependName);
                        }
                    }

                    //Creates list of all cells that the given formula uses.
                    foreach (string var in formula.GetVariables())
                    {
                        NewDependees.Add(var);
                    }

                    //Since we found that the cell already existed, replace the contents and replace any pre-exisiting dependents.
                    cell.Contents = formula;
                    Graph.ReplaceDependents(name, NewDependees);
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
                    NewDependees.Add(var);
                }

                Graph.ReplaceDependees(name, NewDependees);
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

            foreach (string s in Graph.GetDependents(name))
            {
                yield return s;
            }
        }
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
            set { name = value; }
        }
        /// <summary>
        /// Returns cell's contents.
        /// </summary>
        public object Contents
        {
            get { return contents; }
            set { contents = value; }
        }
        /// <summary>
        /// Returns cell's value.
        /// </summary>
        //public object Value
        //{
        //    get { return value; }
        //    set { value }
        //}
        
    }
}
