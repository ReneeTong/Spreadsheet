// These tests are for private use only
// Redistributing this file is strictly against SoC policy.

using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;
using System.Linq;

namespace GradingTests
{


    /// <summary>
    ///This is a test class for SpreadsheetTest and is intended
    ///to contain all SpreadsheetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SpreadsheetTest
    {
        /// <summary>
        /// test cell name 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        public void constructorWithZeroArgument1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=3+2");
            Assert.AreEqual(5.0, s.GetCellValue("a1"));
        }

        /// <summary>
        /// test non exist cell name
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        public void constructorWithZeroArgument2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=3+2");
            Assert.AreEqual("", s.GetCellValue("A1"));
        }

        /// <summary>
        /// test non exist cell name
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        public void constructorWithZeroArgument3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=3+2");
            Assert.AreEqual("", s.GetCellContents("A1"));
        }

        /// <summary>
        /// test non exist cell name
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        public void constructorWithThreeArgument1()
        {
            Spreadsheet s = new Spreadsheet(s=> s.Equals(s.ToUpper()), s=> s.ToUpper(), "default");
            s.SetContentsOfCell("a1", "=3+2");
            Assert.AreEqual(5.0, s.GetCellValue("A1"));
        }

        /// <summary>
        /// test invalid method return false
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        [ExpectedException(typeof(InvalidNameException))]
        public void constructorWithThreeArgument2()
        {
            Spreadsheet s = new Spreadsheet(s => s.Equals(s.ToUpper()), s => s.ToLower(), "default");
            s.SetContentsOfCell("A1", "=3+2");
            s.GetCellContents("A1");
        }

        ///// <summary>
        ///// test cell name 
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("1")]
        //public void constructorWithFourArgument1()
        //{
        //    Spreadsheet f = new Spreadsheet();
        //    f.SetContentsOfCell("a1", "=a2+3");
        //    f.SetContentsOfCell("a2", "2");
        //    f.Save("constructor1.txt");

        //    Spreadsheet s = new Spreadsheet("constructor1.txt", s => true, s => s, "default");
        //    Assert.AreEqual(5.0, s.GetCellValue("a1"));
        //}

        ///// <summary>
        ///// test creating a new file 
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("1")]
        //public void constructorWithFourArgument2()
        //{
        //    Spreadsheet f = new Spreadsheet();
        //    f.SetContentsOfCell("a1", "=a2+3");
        //    f.SetContentsOfCell("a2", "=2");
        //    f.Save("constructor2.txt");
        //    Formula formula = new Formula("a2+3");
        //    Spreadsheet s = new Spreadsheet("constructor2.txt", s => true, s => s, "default");
        //    Assert.AreEqual(formula, s.GetCellContents("a1"));
        //}

        ///// <summary>
        ///// read an empty file
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("1")]
        ////[ExpectedException(typeof(InvalidNameException))]
        //public void constructorWithFourArgument3()
        //{
        //    Spreadsheet f = new Spreadsheet();
        //    f.Save("constructor3.txt");
        //    Spreadsheet s = new Spreadsheet("constructor3.txt", s => true, s => s, "default");
        //    Assert.AreEqual("", s.GetCellContents("a1"));
        //}

        ///// <summary>
        ///// version of the saved spreadsheet does not match the version parameter provided to the constructor
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("1")]
        //[ExpectedException(typeof(SpreadsheetReadWriteException))]
        //public void SpreadsheetReadWriteException1()
        //{
        //    Spreadsheet f = new Spreadsheet();
        //    f.SetContentsOfCell("a1", "a2+3");
        //    f.SetContentsOfCell("a2", "2");
        //    f.Save("Exception1.txt");
        //    Spreadsheet s = new Spreadsheet("Exception1.txt", s => true, s => s, "v.1");
        //}

        ///// <summary>
        ///// names contained in the saved spreadsheet are invalid
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("1")]
        //[ExpectedException(typeof(SpreadsheetReadWriteException))]
        //public void SpreadsheetReadWriteException2()
        //{
        //    String invalidNameString = "{\"Cells\":{\"a_1\":{\"stringForm\":\"=a2+3\"},\"a2\":{\"stringForm\":\"2\"}},\"Version\":\"default\"}";
        //    File.WriteAllText("InvalidName.txt", invalidNameString);
        //    Spreadsheet s = new Spreadsheet("InvalidName.txt", s => true, s => s, "default");
        //}

        ///// <summary>
        ///// invalid formulas is encountered
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("1")]
        //[ExpectedException(typeof(SpreadsheetReadWriteException))]
        //public void SpreadsheetReadWriteException3()
        //{
        //    String invalidNameString = "{\"Cells\":{\"a1\":{\"stringForm\":\"=a2*+3\"},\"a2\":{\"stringForm\":\"2\"}},\"Version\":\"default\"}";
        //    File.WriteAllText("InvalidFormula.txt", invalidNameString);
        //    Spreadsheet s = new Spreadsheet("InvalidFormula.txt", s => true, s => s, "default");
        //}

        ///// <summary>
        ///// circular dependencies is encountered
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("1")]
        //[ExpectedException(typeof(SpreadsheetReadWriteException))]
        //public void SpreadsheetReadWriteException4()
        //{
        //    String invalidNameString = "{\"Cells\":{\"a1\":{\"stringForm\":\"=a2+3\"},\"a2\":{\"stringForm\":\"=a1*2\"}},\"Version\":\"default\"}";
        //    File.WriteAllText("CircularDependencies.txt", invalidNameString);
        //    Spreadsheet s = new Spreadsheet("CircularDependencies.txt", s => true, s => s, "default");
        //}

        ///// <summary>
        ///// problems opening, reading, or closing the file (such as the path not existing)
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("1")]
        //[ExpectedException(typeof(SpreadsheetReadWriteException))]
        //public void SpreadsheetReadWriteException5()
        //{
        //    Spreadsheet s = new Spreadsheet("/missing/noFile.txt", s => true, s => s, "default");
        //}

        ///// <summary>
        ///// problems opening, reading, or closing the file (such as the path not existing)
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("1")]
        //public void SpreadsheetReadWriteException6()
        //{
        //    File.WriteAllText("empty.txt", "");
        //    Spreadsheet s = new Spreadsheet("empty.txt", s => true, s => s, "default");
        //    Assert.AreEqual("", s.GetCellContents("a1"));
        //}

        /// <summary>
        /// test cell name 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidCellName1()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellValue("1A");
        }

        /// <summary>
        /// test cell name 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidCellName2()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellValue("_1");
        }

        /// <summary>
        /// test cell name 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidCellName3()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellValue("X_1");
        }

        /// <summary>
        /// test cell name 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestInvalidCellName4()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("_1");
        }

        /// <summary>
        /// test cell name 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        public void TestInvalidCellName5()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("A15");
        }

        /// <summary>
        /// test cell name 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        public void TestInvalidCellName6()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("a15");
        }

        /// <summary>
        /// test cell name 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        public void TestInvalidCellName7()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("XY032");
        }

        /// <summary>
        /// test cell name 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("1")]
        public void TestInvalidCellName8()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("BC7");
        }

        /// <summary>
        /// the spreadsheet has not been modified since created
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("2")]
        public void Changed1()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual(false, s.Changed);
        }

        /// <summary>
        /// the spreadsheet has been modified since created
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("2")]
        public void Changed2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a2+3");
            Assert.AreEqual(true, s.Changed);
        }

        ///// <summary>
        ///// the spreadsheet has been modified since created but not modified since saved
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("2")]
        //public void Changed3()
        //{
        //    Spreadsheet s = new Spreadsheet();
        //    Assert.AreEqual(false, s.Changed);

        //    s.SetContentsOfCell("a1", "=a2+3");
        //    s.Save("change3Test.txt");
        //    Assert.AreEqual(false, s.Changed);
        //}

        ///// <summary>
        ///// the spreadsheet has been modified since created and saved correctly
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("2")]
        //public void Changed4()
        //{
        //    Spreadsheet s = new Spreadsheet();
        //    Assert.AreEqual(false, s.Changed);

        //    s.SetContentsOfCell("a1", "=a2+3");

        //    s.Save("change4Test.txt");//unsure filename
        //    Assert.AreEqual(false, s.Changed);
        //}

        ///// <summary>
        ///// the spreadsheet has been modified since created and but failed to save
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("2")]
        ////[ExpectedException(typeof(SpreadsheetReadWriteException))]
        //public void Save1()
        //{
        //    Spreadsheet s = new Spreadsheet();
        //    Assert.AreEqual(false, s.Changed);

        //    s.SetContentsOfCell("a1", "=a2+3");
        //    s.Save("save1.txt");

        //    Assert.IsFalse(s.Changed);
        //}

        ///// <summary>
        ///// if it save correctly
        ///// </summary>
        //[TestMethod(), Timeout(5000)]
        //[TestCategory("2")]
        //public void Save2()
        //{
        //    Spreadsheet s = new Spreadsheet();
        //    Assert.AreEqual(false, s.Changed);

        //    s.SetContentsOfCell("a1", "=a2+3");
        //    s.SetContentsOfCell("a2", "=2");

        //    s.Save("save2.txt");

        //    Spreadsheet reopen = new Spreadsheet("save2.txt", s => true, s => s, "default");
        //    Assert.AreEqual(s.GetCellContents("a1"), reopen.GetCellContents("a1"));
        //}

        /// <summary>
        /// save gets a empty string
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("2")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Save3()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual(false, s.Changed);

            s.SetContentsOfCell("a1", "=a2+3");
            s.SetContentsOfCell("a2", "=2");
            s.Save("");
        }

        /// <summary>
        /// the spreadsheet has been modified since created and but failed to save
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("2")]
        public void GetCellValue1()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual(false, s.Changed);

            s.SetContentsOfCell("a1", "=a2+3");

            s.Save("cellValue1.txt");//unsure filename
            Assert.AreEqual(false, s.Changed);
        }

        /// <summary>
        /// the spreadsheet has been modified since created and but failed to save
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("2")]
        public void GetCellValu2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a2+3");
            s.SetContentsOfCell("a2", "=2");

            Assert.AreEqual(5.0, s.GetCellValue("a1"));
        }

        /// <summary>
        /// the spreadsheet has been modified since created and but failed to save
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("2")]
        public void GetCellValu3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a2+3");
            s.SetContentsOfCell("a2", "=a3+4");
            s.SetContentsOfCell("a3", "=a4+5");
            s.SetContentsOfCell("a4", "6");
            Assert.AreEqual(18.0, s.GetCellValue("a1"));

            s.SetContentsOfCell("a4", "3");
            Assert.AreEqual(15.0, s.GetCellValue("a1"));
        }

        /// <summary>
        /// the spreadsheet has been modified since created and but failed to save
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("2")]
        public void GetCellValu4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a2+3");
            Assert.IsTrue(s.GetCellValue("a1") is FormulaError);
        }

        /// <summary>
        /// empty spreadsheet
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetNamesOfAllNonemptyCells1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            List<string> result = sheet.GetNamesOfAllNonemptyCells().ToList();
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// test nonempty cells
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetNamesOfAllNonemptyCells2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "3.5");
            sheet.SetContentsOfCell("a2", "sales");
            sheet.SetContentsOfCell("a3", "=3.2+x1*5");
            List<string> result = sheet.GetNamesOfAllNonemptyCells().ToList();
            List<string> answer = new List<string> { "a1", "a2", "a3" };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test reset cells
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetNamesOfAllNonemptyCells3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "3.5");
            sheet.SetContentsOfCell("a2", "sales");
            sheet.SetContentsOfCell("a2", "=3.2+x1*5");
            List<string> result = sheet.GetNamesOfAllNonemptyCells().ToList();
            List<string> answer = new List<string> { "a1", "a2" };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test set a cell to empty
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetNamesOfAllNonemptyCells4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "3.5");
            sheet.SetContentsOfCell("a2", "sales");
            sheet.SetContentsOfCell("a2", "");
            List<string> result = sheet.GetNamesOfAllNonemptyCells().ToList();
            List<string> answer = new List<string> { "a1" };
            CollectionAssert.AreEqual(answer, result);
        }


        /// <summary>
        /// cell that contain double
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetCellContents1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "10");
            Assert.AreEqual(10.0, sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// cell that contain text
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetCellContents2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "test");
            Assert.AreEqual("test", sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// cell test not string with spaces
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetCellContents3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=3.2+x1*5");
            Formula f1 = new Formula("3.2 + x1 * 5");
            Assert.AreEqual(f1, sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// cell that's empty
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetCellContents4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "");
            Assert.AreEqual("", sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// test reset cell
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetCellContents5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "");
            sheet.SetContentsOfCell("a1", "hi");
            Assert.AreEqual("hi", sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// test reset cell
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetCellContents6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a11", "hi");
            Assert.AreEqual("hi", sheet.GetCellContents("a11"));
        }

        /// <summary>
        /// test set a cell to empty
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetCellContents7()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "3.5");
            sheet.SetContentsOfCell("a2", "sales");
            sheet.SetContentsOfCell("a2", "3.2+x1*5");
            Assert.AreEqual("3.2+x1*5", sheet.GetCellContents("a2"));
        }

        /// <summary>
        /// cell with non-exist variable name
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void GetCellContentsException1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "10");
            Assert.AreEqual("", sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// cell with invalid name
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsException2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "10");
            sheet.GetCellContents("1a");
        }

        /// <summary>
        /// the spreadsheet has been modified since created and but failed to save
        /// </summary>
        [TestMethod(), Timeout(2000)]
        [TestCategory("2")]
        public void SetContentOfCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=a2+3");
            s.SetContentsOfCell("a2", "2");

            Assert.AreEqual(5.0, s.GetCellValue("a1"));
        }

        /// <summary>
        /// see if depends list return correctly
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentOfCell1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "10");
            sheet.SetContentsOfCell("b1", "=a1*2");
            sheet.SetContentsOfCell("c1", "=b1+a1");
            List<string> result = sheet.SetContentsOfCell("a1", "12").ToList();
            List<string> answer = new List<string> { "a1", "b1", "c1" };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test cell to recalculate
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "5");
            sheet.SetContentsOfCell("b1", "=a1+2");
            sheet.SetContentsOfCell("c1", "=a1+b1");
            sheet.SetContentsOfCell("d1", "=a1*7");
            sheet.SetContentsOfCell("e1", "15");
            List<string> result = sheet.SetContentsOfCell("a1", "12").ToList();
            List<string> answer = new List<string> { "a1", "d1", "b1", "c1", };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test cell to recalculate
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "hi");
            sheet.SetContentsOfCell("b1", "=a1+2");
            sheet.SetContentsOfCell("c1", "=a1+b1");
            sheet.SetContentsOfCell("d1", "=a1*7");
            sheet.SetContentsOfCell("e1", "15");
            List<string> result = sheet.SetContentsOfCell("a1", "hello").ToList();
            List<string> answer = new List<string> { "a1", "d1", "b1", "c1", };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test cell to recalculate
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "3");

            sheet.SetContentsOfCell("b1", "=a1*e1");

            sheet.SetContentsOfCell("c1", "=b1+a1");

            sheet.SetContentsOfCell("d1", "5");

            sheet.SetContentsOfCell("e1", "=a1+2");

            List<string> result = sheet.SetContentsOfCell("b1", "4").ToList();
            List<string> answer = new List<string> { "b1", "c1" };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test cell to recalculate by reset cells
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1","3");

            sheet.SetContentsOfCell("b1", "=a1*e1");

            sheet.SetContentsOfCell("c1", "=b1+a1");

            sheet.SetContentsOfCell("d1", "5");

            sheet.SetContentsOfCell("e1", "=a1+2");

            List<string> result = sheet.SetContentsOfCell("a1", "5").ToList();
            List<string> answer = new List<string> { "a1", "e1", "b1", "c1", };
            CollectionAssert.AreEqual(answer, result);

            sheet.SetContentsOfCell("b1", "");

            List<string> result2 = sheet.SetContentsOfCell("a1", "4").ToList();
            List<string> answer2 = new List<string> { "a1", "e1", "c1" };
            CollectionAssert.AreEqual(answer2, result2);
        }

        /// <summary>
        /// test cell to recalculate
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "3");

            sheet.SetContentsOfCell("b1", "=a1*e1");

            sheet.SetContentsOfCell("c1", "=b1+a1");

            sheet.SetContentsOfCell("d1", "5");

            sheet.SetContentsOfCell("e1", "=a1+2");

            List<string> result = sheet.SetContentsOfCell("a1", "yes").ToList();
            List<string> answer = new List<string> { "a1", "e1", "b1", "c1", };
            CollectionAssert.AreEqual(answer, result);

            sheet.SetContentsOfCell("b1", "2");

            List<string> result2 = sheet.SetContentsOfCell("a1", "no").ToList();
            List<string> answer2 = new List<string> { "a1", "e1", "c1" };
            CollectionAssert.AreEqual(answer2, result2);
        }

        /// <summary>
        /// test indirect dependency
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell7()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=b1+1");

            sheet.SetContentsOfCell("b1", "=c1+2");

            sheet.SetContentsOfCell("c1", "=d1+3");

            List<string> result = sheet.SetContentsOfCell("d1", "4").ToList();
            List<string> answer = new List<string> { "d1", "c1", "b1", "a1" };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test indirect dependency
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell8()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=b1+1");

            sheet.SetContentsOfCell("b1", "=c1+2");

            sheet.SetContentsOfCell("c1", "=d1+3");

            List<string> result = sheet.SetContentsOfCell("d1", "hi").ToList();
            List<string> answer = new List<string> { "d1", "c1", "b1", "a1" };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test indirect dependency
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell9()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=b1+1");

            sheet.SetContentsOfCell("b1", "=c1+2");

            sheet.SetContentsOfCell("c1", "=d1+3");

            sheet.SetContentsOfCell("e1", "3");

            List<string> result = sheet.SetContentsOfCell("d1", "=e1+3").ToList();
            List<string> answer = new List<string> { "d1", "c1", "b1", "a1" };
            CollectionAssert.AreEqual(answer, result);
        }

        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentOfCells10()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "10");
            sheet.SetContentsOfCell("b1", "=a1*2");
            sheet.SetContentsOfCell("c1", "=b1+3");
            List<string> result = sheet.SetContentsOfCell("a1", "12").ToList();
            List<string> answer = new List<string> { "a1", "b1", "c1" };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test indirect dependency
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell11()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=b1+1");

            sheet.SetContentsOfCell("c1", "=d1+3");

            sheet.SetContentsOfCell("b1", "=c1+2");

            sheet.SetContentsOfCell("e1", "3");

            List<string> result = sheet.SetContentsOfCell("d1", "=e1+3").ToList();
            List<string> answer = new List<string> { "d1", "c1", "b1", "a1" };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test indirect dependency
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetCellContents12()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "5");

            sheet.SetContentsOfCell("b1", "=a1+2");

            sheet.SetContentsOfCell("c1", "=a1+b1");

            sheet.SetContentsOfCell("d1", "=a1*7");

            sheet.SetContentsOfCell("e1", "15");

            List<string> result = sheet.SetContentsOfCell("a1", "e=1+2").ToList();
            List<string> answer = new List<string> { "a1", "d1", "b1", "c1", };
            CollectionAssert.AreEqual(answer, result);
        }

        /// <summary>
        /// test cell content after circularException
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell13()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "5");

            sheet.SetContentsOfCell("b1", "=a1+2");

            try
            {
                sheet.SetContentsOfCell("a1", "=b1+3");
            }
            catch (CircularException)
            {
                double result = (double)sheet.GetCellContents("a1");
                Assert.AreEqual(5.0, result);
            }

        }

        /// <summary>
        /// test cell to recalculate by reset cells
        /// </summary>
        [TestMethod, Timeout(5000)]
        [TestCategory("1")]
        public void SetContentsOfCell14()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "3");
            sheet.SetContentsOfCell("b1", "=a1*e1");
            sheet.SetContentsOfCell("c1", "=b1+a1");
            sheet.SetContentsOfCell("d1", "5");
            sheet.SetContentsOfCell("e1", "=a1+2");

            List<string> result = sheet.SetContentsOfCell("a1", "5").ToList();
            List<string> answer = new List<string> { "a1", "e1", "b1", "c1", };
            CollectionAssert.AreEqual(answer, result);

            sheet.SetContentsOfCell("b1", "2");

            List<string> result2 = sheet.SetContentsOfCell("a1", "4").ToList();
            List<string> answer2 = new List<string> { "a1", "e1", "c1" };
            CollectionAssert.AreEqual(answer2, result2);
        }

        /// <summary>
        /// test cell to recalculate by reset cells
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("1")]
        public void SetContentsOfCell15()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "a1");
            s.SetContentsOfCell("b1", "=a1*e1");
            Assert.IsTrue(s.GetCellValue("b1") is FormulaError);
        }

        /// <summary>
        /// test cell to recalculate by reset cells
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("1")]
        public void SetContentsOfCell16()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "a2");
            s.SetContentsOfCell("a1", "a2*e1");
            Assert.AreEqual("a2*e1", s.GetCellValue("a1"));
        }

        /// <summary>
        /// test cell to recalculate by reset cells
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("1")]
        public void SetContentsOfCell17()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "hi");
            s.Save("save.txt");

            Spreadsheet s2 = new Spreadsheet("save.txt", s => true, s => s, "default");
            Assert.AreEqual("hi", s.GetCellValue("A1"));
        }

        // STRESS TESTS
        [TestMethod(), Timeout(5000)]
        [TestCategory("31")]
        public void TestStress1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1+B2");
            s.SetContentsOfCell("B1", "=C1-C2");
            s.SetContentsOfCell("B2", "=C3*C4");
            s.SetContentsOfCell("C1", "=D1*D2");
            s.SetContentsOfCell("C2", "=D3*D4");
            s.SetContentsOfCell("C3", "=D5*D6");
            s.SetContentsOfCell("C4", "=D7*D8");
            s.SetContentsOfCell("D1", "=E1");
            s.SetContentsOfCell("D2", "=E1");
            s.SetContentsOfCell("D3", "=E1");
            s.SetContentsOfCell("D4", "=E1");
            s.SetContentsOfCell("D5", "=E1");
            s.SetContentsOfCell("D6", "=E1");
            s.SetContentsOfCell("D7", "=E1");
            s.SetContentsOfCell("D8", "=E1");
            IList<String> cells = s.SetContentsOfCell("E1", "0");
            Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
        }

        // Repeated for extra weight
        [TestMethod(), Timeout(5000)]
        [TestCategory("32")]
        public void TestStress1a()
        {
            TestStress1();
        }
        [TestMethod(), Timeout(5000)]
        [TestCategory("33")]
        public void TestStress1b()
        {
            TestStress1();
        }
        [TestMethod(), Timeout(5000)]
        [TestCategory("34")]
        public void TestStress1c()
        {
            TestStress1();
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("35")]
        public void TestStress2()
        {
            Spreadsheet s = new Spreadsheet();
            ISet<String> cells = new HashSet<string>();
            for (int i = 1; i < 200; i++)
            {
                cells.Add("A" + i);
                String formula = "=" + new Formula("A" + (i + 1).ToString());
                Assert.IsTrue(cells.SetEquals(s.SetContentsOfCell("A" + i, formula)));
            }
        }
        [TestMethod(), Timeout(5000)]
        [TestCategory("36")]
        public void TestStress2a()
        {
            TestStress2();
        }
        [TestMethod(), Timeout(5000)]
        [TestCategory("37")]
        public void TestStress2b()
        {
            TestStress2();
        }
        [TestMethod(), Timeout(5000)]
        [TestCategory("38")]
        public void TestStress2c()
        {
            TestStress2();
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("39")]
        public void TestStress3()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 1; i < 200; i++)
            {
                s.SetContentsOfCell("A" + i, "=" + new Formula("A" + (i + 1).ToString()));
            }
            try
            {
                s.SetContentsOfCell("A150", "=A50");
                Assert.Fail();
            }
            catch (CircularException)
            {
            }
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("40")]
        public void TestStress3a()
        {
            TestStress3();
        }
        [TestMethod(), Timeout(5000)]
        [TestCategory("41")]
        public void TestStress3b()
        {
            TestStress3();
        }
        [TestMethod(), Timeout(5000)]
        [TestCategory("42")]
        public void TestStress3c()
        {
            TestStress3();
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("43")]
        public void TestStress4()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 0; i < 500; i++)
            {
                String formula = "=" + new Formula("A1" + (i + 1).ToString());
                s.SetContentsOfCell("A1" + i, formula);
            }
            LinkedList<string> firstCells = new LinkedList<string>();
            LinkedList<string> lastCells = new LinkedList<string>();
            for (int i = 0; i < 250; i++)
            {
                firstCells.AddFirst("A1" + i);
                lastCells.AddFirst("A1" + (i + 250));
            }
            Assert.IsTrue(s.SetContentsOfCell("A1249", "25.0").SequenceEqual(firstCells));
            Assert.IsTrue(s.SetContentsOfCell("A1499", "0").SequenceEqual(lastCells));
        }
        [TestMethod(), Timeout(5000)]
        [TestCategory("44")]
        public void TestStress4a()
        {
            TestStress4();
        }
        [TestMethod(), Timeout(5000)]
        [TestCategory("45")]
        public void TestStress4b()
        {
            TestStress4();
        }
        [TestMethod(), Timeout(5000)]
        [TestCategory("45")]
        public void TestStress4c()
        {
            TestStress4();
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("47")]
        public void TestStress5()
        {
            RunRandomizedTest(47, 2519);
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("48")]
        public void TestStress6()
        {
            RunRandomizedTest(48, 2521);
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("49")]
        public void TestStress7()
        {
            RunRandomizedTest(49, 2526);
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("50")]
        public void TestStress8()
        {
            RunRandomizedTest(50, 2521);
        }

        /// <summary>
        /// Sets random contents for a random cell 10000 times
        /// </summary>
        /// <param name="seed">Random seed</param>
        /// <param name="size">The known resulting spreadsheet size, given the seed</param>
        public void RunRandomizedTest(int seed, int size)
        {
            Spreadsheet s = new Spreadsheet();
            Random rand = new Random(seed);
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    switch (rand.Next(3))
                    {
                        case 0:
                            s.SetContentsOfCell(randomName(rand), "3.14");
                            break;
                        case 1:
                            s.SetContentsOfCell(randomName(rand), "hello");
                            break;
                        case 2:
                            s.SetContentsOfCell(randomName(rand), randomFormula(rand));
                            break;
                    }
                }
                catch (CircularException)
                {
                }
            }
            ISet<string> set = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(size, set.Count);
        }

        /// <summary>
        /// Generates a random cell name with a capital letter and number between 1 - 99
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        private String randomName(Random rand)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(rand.Next(26), 1) + (rand.Next(99) + 1);
        }

        /// <summary>
        /// Generates a random Formula
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        private String randomFormula(Random rand)
        {
            String f = randomName(rand);
            for (int i = 0; i < 10; i++)
            {
                switch (rand.Next(4))
                {
                    case 0:
                        f += "+";
                        break;
                    case 1:
                        f += "-";
                        break;
                    case 2:
                        f += "*";
                        break;
                    case 3:
                        f += "/";
                        break;
                }
                switch (rand.Next(2))
                {
                    case 0:
                        f += 7.2;
                        break;
                    case 1:
                        f += randomName(rand);
                        break;
                }
            }
            return f;
        }

    }
}