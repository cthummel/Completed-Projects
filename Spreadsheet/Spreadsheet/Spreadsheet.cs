//Written by Corin Thummel, February 2017.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
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
        private List<Cell> CellList;
        private DependencyGraph Graph;
        private Regex IsValid;
        private bool changed;
        private bool PassesSchema;

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            CellList = new List<Cell>();
            Graph = new DependencyGraph();
            changed = false;
            IsValid = new Regex(@".*?");
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter.
        /// </summary>
        public Spreadsheet(Regex isValid)
        {
            CellList = new List<Cell>();
            Graph = new DependencyGraph();
            changed = false;
            IsValid = isValid;
        }

        /// <summary>
        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        ///
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  
        ///
        /// If there's a problem reading source, throws an IOException.
        ///
        /// Else if the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  
        ///
        /// Else if the IsValid string contained in source is not a valid C# regular expression, throws
        /// a SpreadsheetReadException.  (If the exception is not thrown, this regex is referred to
        /// below as oldIsValid.)
        ///
        /// Else if there is a duplicate cell name in the source, throws a SpreadsheetReadException.
        /// (Two cell names are duplicates if they are identical after being converted to upper case.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a 
        /// SpreadsheetReadException.  (Use oldIsValid in place of IsValid in the definition of 
        /// cell name validity.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetVersionException.  (Use newIsValid in place of IsValid in the definition of
        /// cell name validity.)
        ///
        /// Else if there's a formula that causes a circular dependency, throws a SpreadsheetReadException. 
        ///
        /// Else, create a Spreadsheet that is a duplicate of the one encoded in source except that
        /// the new Spreadsheet's IsValid regular expression should be newIsValid.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newIsValid"></param>
        public Spreadsheet(TextReader source, Regex newIsValid)
        {
            CellList = new List<Cell>();
            Graph = new DependencyGraph();
            PassesSchema = true;

            var CellDictionary = new Dictionary<string, string>();

            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add(null, "spreadsheet.xsd");
            // Configure validation.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            //settings.ValidationEventHandler += ValidationCallback;

            using (XmlReader reader = XmlReader.Create(source, settings))
            {
                try
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    string oldisvalid = reader["IsValid"];

                                    try
                                    {
                                        Regex OldIsValid = new Regex(oldisvalid);
                                        IsValid = OldIsValid;
                                    }
                                    catch
                                    {
                                        throw new SpreadsheetReadException("Old Regex from source is invalid.");
                                    }
                                    break;

                                case "cell":
                                    string name = reader["name"];
                                    string contents = reader["contents"];

                                    if (name == null || contents == null)
                                    {
                                        throw new XmlSchemaValidationException();
                                    }

                                    try
                                    {
                                        CellDictionary.Add(name.ToUpper(), contents);
                                    }
                                    catch
                                    {
                                        throw new SpreadsheetReadException("Duplicate cell names in source.");
                                    }
                                    break;

                                default:
                                    PassesSchema = false;
                                    break;
                            }
                        }
                    }
                    if (PassesSchema == false)
                    {
                        throw new XmlSchemaValidationException();
                    }
                    
                }
                catch (XmlSchemaValidationException)
                {
                    throw new SpreadsheetReadException("Issues validating with schema. Improper fomatting of xml.");
                }
                catch (IOException)
                {
                    throw new IOException("Issues reading from source.");
                }
            }

            
            


            // Now we have collected a Dictionary containing all the cells that are looking to be input to the new spreadsheet.
            foreach (string name in CellDictionary.Keys)
            {
                //First check each potential cell using OldIsValid
                string contents;
                CellDictionary.TryGetValue(name, out contents);
                
                try
                {
                    SetContentsOfCell(name, contents);
                }
                catch (InvalidNameException)
                {
                    throw new SpreadsheetReadException("Error building spreadsheet using IsValid Regex read from xml.");
                }
                catch (CircularException)
                {
                    throw new SpreadsheetReadException("Xml contained a circular dependency.");
                }
            }

            IsValid = newIsValid;

            foreach (string name in CellDictionary.Keys)
            {
                //Next check each potential cell using newIsValid
                string contents;
                CellDictionary.TryGetValue(name, out contents);
                try
                {
                    SetContentsOfCell(name, contents);
                }
                catch (InvalidNameException)
                {
                    throw new SpreadsheetVersionException("Regex from xml and new Regex in parameter are not compatible.");
                }
                catch (CircularException)
                {
                    throw new SpreadsheetReadException("Xml contained a circular dependency.");
                }
            }
            Changed = false;
        }

        //private static void ValidationCallback(object sender, ValidationEventArgs e)
        //{
        //    Console.WriteLine(" *** Validation Error: {0}", e.Message);
        //}


        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return changed;
            }
            protected set
            {
                changed = value;
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
            try
            {
                using (XmlWriter writer = XmlWriter.Create(dest))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("", "spreadsheet", "urn:spreadsheet-schema");
                    writer.WriteAttributeString("IsValid", IsValid.ToString());

                    foreach (Cell cell in CellList)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteAttributeString("name", cell.Name);

                        //Different contents options.
                        if (cell.Contents.GetType() == (typeof(Formula)))
                        {
                            writer.WriteAttributeString("contents", "=" + cell.Contents.ToString());
                        }
                        else
                        {
                            writer.WriteAttributeString("contents", cell.Contents.ToString());
                        }

                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch
            {
                throw new IOException("Issue writing to the destination.");
            }
            
            //make sure to change the "changed" value so that we know if things are modified after a save.
            Changed = false;
        }

        

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if (name == null || !IsValid.IsMatch(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            foreach (Cell cell in CellList)
            {
                if (cell.Name == name)
                {
                    return cell.Value;
                }
            }
            return string.Empty;
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
            if (name == null || !IsValid.IsMatch(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
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
            var ReturnSet = new HashSet<string>();
            if (content == null)
            {
                throw new ArgumentNullException();
            }

            if (name == null || !IsValid.IsMatch(name))
            {
                throw new InvalidNameException();
            }

            //Double case.
            double number;
            if (double.TryParse(content, out number))
            {
                foreach (string s in SetCellContents(name.ToUpper(), number))
                {
                    ReturnSet.Add(s);
                }
            }
            //Formula case.
            else if (content != string.Empty && content[0] == '=')
            {
                string text = content.Substring(1);
                Formula form = new Formula(text, s => s.ToUpper(), s => IsValid.IsMatch(s));
                foreach (string redo in SetCellContents(name.ToUpper(), form))
                {
                    ReturnSet.Add(redo);
                }
            }

            //Text case.
            else
            {
                foreach (string s in SetCellContents(name.ToUpper(), content))
                {
                    ReturnSet.Add(s);
                }
            }


            //Now we have all the cells to Recalculate in ReturnSet.
            foreach (string recalc in ReturnSet)
            {
                foreach (Cell cell in CellList)
                {
                    if (cell.Name == recalc)
                    {
                        //Only need to recalcuate cells whose contents are formulas. Other cells are self sufficient and values set earlier.
                        if (cell.Contents.GetType() == typeof(Formula))
                        {
                            Formula contents = (Formula)cell.Contents;
                            try
                            {
                                cell.Value = contents.Evaluate(s => (double)GetCellValue(s));
                            }
                            catch (InvalidCastException)
                            {
                                cell.Value = new FormulaError("One or more variables have an undefined value.");
                            }
                            catch (FormulaEvaluationException)
                            {
                                cell.Value = new FormulaError("Issue evaluating formula.");
                            }
                            
                        }
                    }
                }
            }
            return ReturnSet;
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
                    cell.Value = number;

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
            if (found == false)
            {
                Cell newcell = new Cell(name, number, number);
                CellList.Add(newcell);
                foreach (string var in GetCellsToRecalculate(name))
                {
                    ReturnSet.Add(var);
                }
            }
            Changed = true;
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
                        cell.Value = text;

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
                        //Example.
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
            Changed = true;
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
            var NewDependents = new List<string>();
            var OldDependees = new List<string>();
            var VariableValues = new Dictionary<string, double>();
            bool found = false;
            bool InGraph = false;

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
                            throw new CircularException("Spreadsheet contains a Circular Dependancy");
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
                    
                    cell.Contents = formula;
                    found = true;
                    break;
                }
            }

            //If the name wasnt in the list already we can add it as a new cell.
            if (found == false)
            {
                Cell newcell = new Cell(name, formula, new FormulaError("One or more variables have an undefined value."));
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
            Changed = true;
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

            if (!IsValid.IsMatch(name))
            {
                throw new InvalidNameException();
            }

            foreach (string s in Graph.GetDependees(name))
            {
                yield return s;
            }
        }
    }

    class Cell
    {
        private string name;
        private object contents;
        private object cellvalue;

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
            cellvalue = Value;
        }
        /// <summary>
        /// Returns cell's name.
        /// </summary>
        public string Name
        {
            get { return name; }
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
        public object Value
        {
            get { return cellvalue; }
            set { cellvalue = value; }
        }

    }
}
