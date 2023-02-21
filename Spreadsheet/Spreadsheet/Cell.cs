using System;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Newtonsoft.Json;
using SpreadsheetUtilities;

namespace SS
{
    /// <summary>
    /// this class represent a cell, each cell contain a name
    /// the cell content can be either a double, a string or a formula
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Cell
    {
        private string cellName;
        //private object cellContent;//a double, a string or a formula
        private object cellValue;//a double, a string or a formulaError

        [JsonProperty(PropertyName = "stringForm")]
        private string cellContentString;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="c"></param>
        public Cell(string name, object v, string s)
        {
            cellName = name;
            //cellContent = c;
            cellValue = v;
            cellContentString = s;
        }

        /// <summary>
        /// return the cell content
        /// a doubld, a string or a formula
        /// </summary>
        /// <returns></returns>
        public object getContent()
        {
            //return cellContent;

            double tempNumber;
            if (Double.TryParse(cellContentString, out tempNumber))
            {
                return tempNumber;
            }else if((cellContentString.Length > 0))
            {
                if (cellContentString[0] == '=')
                {
                    string formula = cellContentString.Substring(1);
                    Formula tempF = new Formula(formula);
                    return tempF;
                }
            }
            return cellContentString;
        }

        /// <summary>
        /// return the cell value
        /// a string, a double, or a formulaError
        /// </summary>
        /// <returns></returns>
        public object getValue()
        {
            return cellValue;
        }

        /// <summary>
        /// set the value of the cell
        /// </summary>
        /// <param name="v"></param>
        public void setCellValue(object v)
        {
            cellValue = v;
        }

        /// <summary>
        /// recalculate all the cell value that depends on the current cell
        /// </summary>
        /// <param name="list"></param>
        /// <param name="cellNames"></param>
        /// <param name="lookup"></param>
        public void reSetAllDependents(IList<string> list, Dictionary<string, Cell> cellNames, Func<string,double> lookup)
        {
            foreach(string c in list)
            {
                Cell? temp;
                if (cellNames.TryGetValue(c, out temp))
                {
                    Object content = temp.getContent();
                    if(content.GetType() == typeof(Formula))
                    {
                        Formula f = (Formula)content;
                        temp.setCellValue(f.Evaluate(lookup));
                    }
                    
                }
    
            }
        }

        
    }
}

