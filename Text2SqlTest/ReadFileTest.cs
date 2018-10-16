using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using text2sql;
using System.Collections.Generic;

namespace txt2SqlTest
{
    [TestClass]
    public class ReadFileTest
    {
        [TestMethod]
        public void ReadTextSequence_IspathExists_NotFileFound()
        {
            var readFile = new ReadFile();
            string FileName = "test1.txt";
            List<string> ExpectedFileLines;
            

            ExpectedFileLines=readFile.ReadTextSequence(FileName);

            Assert.IsNull(ExpectedFileLines);
        }
        [TestMethod]
        public void ReadTextSequence_IspathExists_FileFound()
        {
            var readFile = new ReadFile();
            string FileName = "file1.txt";
            List<string> ExpectedFileLines;


            ExpectedFileLines = readFile.ReadTextSequence(FileName);

            Assert.IsNotNull(ExpectedFileLines);
        }
    }
}
