using System;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;
using Newtonsoft.Json;

namespace SS
{
    /// <summary>
    /// This class represent a sheet, which contain cells, each cell can either
    /// contain a double, a string or a formula. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Spreadsheet : AbstractSpreadsheet
    {
        private DependencyGraph graph; //keep track of cells dependency

        [JsonProperty(PropertyName = "Cells")]
        private Dictionary<string, Cell> cellNames;//contain all current cells
        private bool changed; // true if modified, false otherwise

        /// <summary>
        /// create an empty sheet
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            graph = new();
            cellNames = new();
            Changed = false;
        }

        /// <summary>
        /// constructor takes in a validertor, a normalizor and a version
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            graph = new();
            cellNames = new();
            Changed = false;
        }

        /// <summary>
        /// constructor taks four argument
        /// a filepath, a validertor, a normalizor and a version
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        /// <exception cref="SpreadsheetReadWriteException"></exception>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : this(isValid, normalize, version)
        {
            graph = new();
            cellNames = new();
            Changed = false;

            //read the json file and convert everything into a spreadsheet if valid
            try
                {
                    using (StreamReader r = new StreamReader(filePath))
                    {
                    //deseralize the file
                        string json = r.ReadToEnd();
                        Spreadsheet? s = JsonConvert.DeserializeObject<Spreadsheet>(json);
                    //if the file is null
                        if (s == null)
                        {
                            return;
                        }
                        else
                        {
                            if (s.Version != version) // if the version doesn't match, throw exception
                            {
                                throw new SpreadsheetReadWriteException("Version of the given file does not match given version");
                            }
                            else
                            {//loop through all cells
                                foreach (string c in s.GetNamesOfAllNonemptyCells())
                                {
                                    String name = Normalize(c);
                                    if (nameCheck(name) & isValid(name))
                                    {
                                        try
                                        {//if the cell contain a formula, change it valid formula string form
                                            Object cellContent = s.GetCellContents(name);
                                            if (cellContent.GetType() == typeof(Formula))
                                            {
                                                String formulaString = "=";
                                                Formula formula = (Formula)cellContent;
                                                formulaString += cellContent.ToString();
                                                SetContentsOfCell(c, formulaString);
                                            }
                                            else //pass in the number of string
                                            {
                                                String content = "";
                                                content += cellContent.ToString();
                                                SetContentsOfCell(c, content);
                                            }
                                        }
                                        catch (CircularException)//if formula in file would cause circularException
                                        {
                                            throw new SpreadsheetReadWriteException("formula in the file is causing circularException");
                                        }
                                    }
                                    else//if the cell name in the file is not valid
                                    {
                                        throw new SpreadsheetReadWriteException("cell name in the saving file is invalid");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)//catch all other exception and throw expception
                {
                    throw new SpreadsheetReadWriteException("Trouble opening, reading or closing the file");
                }

            changed = false;

        }

        /// <summary>
        /// Inherited method
        /// </summary>
        public override bool Changed
        {
            get => changed;
            protected set => changed = value;
        }

        /// <summary>
        /// Inherited method
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        public override object GetCellContents(string name)
        {
            //check if the name is valid
            if (nameCheck(name))
            {
                Cell? temp;
                if (cellNames.TryGetValue(name, out temp))
                {
                    return temp.getContent();
                }
                else
                {
                    return "";
                }

            }
            throw new InvalidNameException();
        }

        /// <summary>
        /// Inherited method
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        public override object GetCellValue(string name)
        {
            if (nameCheck(name))
            {
                Cell? tempCell;
                if (cellNames.TryGetValue(name, out tempCell))
                {
                    return tempCell.getValue();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// Inherited method
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            List<string> allCells = new List<string>(cellNames.Keys);
            return allCells;
        }

        /// <summary>
        /// Inherited method
        /// </summary>
        /// <param name="filename"></param>
        public override void Save(string filename)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(this);
                File.WriteAllText(filename, jsonString);
                Changed = false;
            }
            catch
            {
                throw new SpreadsheetReadWriteException("trouble opening,wring or closing the spreadsheet");
            }


            //throw new SpreadsheetReadWriteException();
        }

        /// <summary>
        /// Inherited method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            name = Normalize(name);
            if(content.Equals(""))
            {
                List<string> cellList = new();
                cellNames.Remove(name);
                graph.ReplaceDependees(name, cellList);
                cellList.Add(name);
                return cellList;
            }
            //check if the name is valid
            if (nameCheck(name)&&(IsValid(name)))
            {
                Changed = true;

                //if it's a double number
                double tempNumber;

                if (Double.TryParse(content, out tempNumber))
                {
                    
                    IList<string> list = SetCellContents(name, tempNumber);
                    Cell? tempCell;
                    if (cellNames.TryGetValue(name, out tempCell))
                    {
                        tempCell.reSetAllDependents(list, cellNames, lookup);
                    }
                    return list;
                    
                }
                else if (content[0] == '=')//if it's a formula
                {
                    string formula = content.Substring(1);
                    Formula tempF = new Formula(formula);
                    IList<string> list = SetCellContents(name, tempF);
                    Cell? tempCell;
                    if (cellNames.TryGetValue(name, out tempCell))
                    {
                        tempCell.reSetAllDependents(list, cellNames, lookup);
                    }
                    return list;
                }
                else//if it's a string
                {

                    IList<string> list = SetCellContents(name, content);
                    Cell? tempCell;
                    if (cellNames.TryGetValue(name, out tempCell))
                    {
                        tempCell.reSetAllDependents(list, cellNames, lookup);
                    }
                    return list;
                }
            }
            else//if name is not valid, throw InvalidNameException
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// Inherited method
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return graph.GetDependents(name);
        }

        /// <summary>
        /// Inherited method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        protected override IList<string> SetCellContents(string name, double number)
        {
            //check if the cell already have contents
            //if yes, remove it and deleate its dependess
            List<string> cellList = new();

            if (cellNames.ContainsKey(name))
            {
                cellNames.Remove(name);
                graph.ReplaceDependees(name, cellList);
            }

            //put the new cell in it
            Cell tempCell = new(name, number, number.ToString());
            cellNames.Add(name, tempCell);

            //get all the cells that's dirctly or indircly relates to it
            cellList = GetCellsToRecalculate(name).ToList();
            return cellList;
        }

        /// <summary>
        /// Inherited method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected override IList<string> SetCellContents(string name, string text)
        {
            //check if the 
            List<string> cellList = new();
            if (text.Equals(""))
            {
                cellNames.Remove(name);
                graph.ReplaceDependees(name, cellList);
                cellList.Add(name);
                return cellList;
            }

            //check if the cell already have contents
            //if yes, remove it and deleate its dependess
            if (cellNames.ContainsKey(name))
            {
                cellNames.Remove(name);
                graph.ReplaceDependees(name, cellList);
            }

            //put the new cell in it
            Cell tempCell = new(name, text, text);
            cellNames.Add(name, tempCell);

            //get all the cells that's dirctly or indircly relates to it
            cellList = GetCellsToRecalculate(name).ToList();
            return cellList;
        }

        /// <summary>
        /// Inherited method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        /// <exception cref="CircularException"></exception>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            List<string> cellList = new();
            Cell? temp; // store the current cell contents if has any
                        //if the cell name alreay exist, remove it
            if (cellNames.TryGetValue(name, out temp))
            {
                cellNames.Remove(name);
                graph.ReplaceDependees(name, cellList);
            }

            //create a new cell
            String cellString = "=";
            cellString += formula.ToString();
            Cell tempCell = new(name, formula.Evaluate(lookup),cellString);
            cellNames.Add(name, tempCell);

            //add all the variables contain in the formula as dependents of current cell
            IEnumerable<string> variables = formula.GetVariables();
            foreach (string v in variables)
            {
                graph.AddDependency(v, name);
            }

            try//recalculate current cells and return a list if there's no circularException
            {
                cellList = GetCellsToRecalculate(name).ToList();
                return cellList;
            }
            catch (CircularException)
            {
                //put what was in the cell orginally back to the cell
                if (temp != null)
                {
                    cellNames.Remove(name);
                    cellNames.Add(name, temp);
                }
                else
                {
                    SetCellContents(name, "");
                }

                throw new CircularException();
            }

        }

        /// <summary>
        /// This method check if the cell name is valid
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool nameCheck(string name)
        {
            string pattern = "^[A-Za-z]+[0-9]+$";
            return Regex.IsMatch(name, pattern);
        }

        /// <summary>
        /// lookup method for formula variable
        /// takes in the cell name, and return the cell value
        /// throw argumentException if there's no value for the cell
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private double lookup(string s)
        {
            Object value = GetCellValue(s);
            if (value.GetType() == typeof(double))
            {
                return (double)value;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}

