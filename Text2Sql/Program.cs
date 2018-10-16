using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text2sql
{ 
    public class Customer_Model
    {
        public int Id { get; set; }
        public string FirstName {get;set;}
        public string LastName { get; set; }
        public char Gender { get; set; }
        public string DoB { get; set; }
        public bool marital_status { get; set; }
    }
    public class Error
    {
        public void ErrorHandler(bool inputError,bool textError,string fileName)
        {
            if(inputError) Console.WriteLine("\n ERROR Msg: Input '" + fileName + "' Is in invalid format");
            else if(textError) Console.WriteLine("\n ERROR Msg: File '" + fileName + "' Can Not Be Found");
            else Console.WriteLine("\n ERROR Msg: Unknown error");
        }
    }
    public class Input
    {
        private int Sequence { get; set; }
        private string FileName { get; set; }
        private string UserInputs { get; set; }
        public void UserPrompt(bool Isattempted)
        {
            ReadFile readFile = new ReadFile();
            CreateSQl sql = new CreateSQl();
            List<string> TextLines = new List<string>();

            if (!Isattempted) Console.WriteLine("\n Input text file and sequence");
            else Console.WriteLine("\nIncorrect Format Please Re_Enter! 'text file and sequence' ");
            string UserInputs = Console.ReadLine();

            setFileNamenSequenceNo(UserInputs);
            TextLines = readFile.ReadTextSequence(FileName);
            if(TextLines != null) sql.CreateSQlLines(TextLines, FileName, Sequence);

        }
        public void setFileNamenSequenceNo(string getUserInputs)
        {
            string[] FileNamenSequence = getUserInputs.Split(' ');
            int IsConverted;
            
            if(FileNamenSequence.Length >1)
            {
                if(Int32.TryParse(FileNamenSequence[1], out IsConverted))
                {
                    FileName = FileNamenSequence[0];
                    Sequence = Int32.Parse(FileNamenSequence[1]);
                }
                else
                {
                    Error error = new Error();
                    Input inputs = new Input();
                    error.ErrorHandler(true, false, FileNamenSequence[1]);
                    inputs.UserPrompt(true);
                }

            }
            else
            {
                Error error = new Error();
                Input inputs = new Input();
                error.ErrorHandler(true, false, getUserInputs);
                inputs.UserPrompt(true);
            }
            
        }
       
        
    }
    public class Instruction
    {
        public void InstructionsFile()
        {
            FileInfo IspathExists = new FileInfo(@"Instruction.txt");
            string line;

            if (IspathExists.Exists)
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@"Instruction.txt");
                while ((line = file.ReadLine()) != null)
                {
                    System.Console.WriteLine(line);
                }

                file.Close();

            }
            else     Console.WriteLine("WARNING! Msg: Instruction File Could Not Be Found!");
     
        }
    }
    public class ReadFile
    {
        
        public List<string> ReadTextSequence(string fileName)
        {
            int counter = 0;
            string line;
            List<string> lines = new List<string>();

            FileInfo IspathExists = new FileInfo(@"TxtFiles/" + fileName);
     
            
            if(IspathExists.Exists){
                System.IO.StreamReader file = new System.IO.StreamReader(@"TxtFiles/" + fileName);
                while ((line = file.ReadLine()) != null){
                    lines.Add(line);
                    counter++;
                }

                file.Close();
                Console.WriteLine("\n"+counter+" Lines Interpreted Successfully...");
                return lines;
            }
            else{
                
                Error error = new Error();
                Input inputs = new Input();
                error.ErrorHandler(false,true,fileName);
                inputs.UserPrompt(true);
                return null;
            }
             
        }
    }
    public class CreateSQl
    {
        
        public void CreateSQlLines(List<string> lines,string Fname,int sequence)
        {
            Customer_Model customer = new Customer_Model();
            string sql="";
            string value = "";
            bool StopSqlLines = false;
            string[] data = { "" };
            List<string> CustObject = new List<string>();
            List<string> SqlLines = new List<string>();
            
            foreach (string line in lines)
            {
                if(Fname.ToLower()== "file1.txt")
                {
                    data = line.Split(',');
                    string[] fullName=data[0].Split(' ');
                    customer.FirstName = fullName[0];
                    customer.LastName = fullName[1];
                    customer.Gender = data[1].Trim().ToCharArray()[0];
                    customer.DoB = data[2];
                    if (data[3].ToUpper() == "Y") customer.marital_status = true;
                    else customer.marital_status = false;
                    
                }
                else if (Fname.ToLower() == "file2.txt")
                {
                    data = line.Split('|');
                    
                    customer.FirstName = data[0].Substring(1,data[0].Length-2);
                    customer.LastName = data[1].Substring(1, data[1].Length-2);
                    customer.DoB = data[2].Substring(1, data[2].Length-2);
                   
                    data[3] = data[3].Trim().ToLower();
                    if (data[3].Substring(1, data[3].Length - 2) == "married") customer.marital_status = true;
                    else customer.marital_status = false;

                    data[4] = data[4].Trim().ToLower();
                    if (data[4].Substring(1, data[4].Length - 2) == "male") customer.Gender = 'M';
                    else customer.Gender = 'F';
                }
                else if (Fname.ToLower() == "file3.txt")
                {
                    StopSqlLines = true;
                    if(!line.Contains('#') && line.Length != 0)
                    { 
                        data = line.Split(':');
                        value = data[1];
                        CustObject.Add(value);
                        if(CustObject.Count == 5)
                        {
                            StopSqlLines = false;
                            customer.FirstName = CustObject[0];
                            customer.LastName = CustObject[1];
                            customer.DoB = CustObject[2];

                            CustObject[3] = CustObject[3].Trim().ToLower();
                            if (CustObject[3] == "yes") customer.marital_status = true;
                            else customer.marital_status = false;

                            CustObject[4] = CustObject[4].Trim().ToLower();
                            if (CustObject[4] == "male") customer.Gender = 'M';
                            else customer.Gender = 'F';
                            CustObject = new List<string>();
                        }
                    }
                  
                }
                if (!StopSqlLines)
                {
                    sql = string.Format("insert into customers(id, first_name, last_name, gender, date_of_birth, marital_status) values('" + sequence
                                            + "','" + customer.FirstName + "', '" + customer.LastName + "','"
                                            + customer.Gender + "', " + customer.DoB + ", " + customer.marital_status + ");");
                    SqlLines.Add(sql);
                    sequence++;
                }
                
            }
            
            CreateSQlFile(SqlLines,Fname);
        }
        private void CreateSQlFile(List<string> SqlLines,string fName)
        {
            string[] withoutExtention =fName.Split('.');
        
            System.IO.File.WriteAllLines(@"SqlFiles/" + withoutExtention[0] + ".sql", SqlLines);
            Console.WriteLine("\nSuccess Msg: " + withoutExtention[0]+".sql Created Successfully...");
            Console.WriteLine("View Created File(.sql) at Project Folder in folder 'SqlFiles'");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            Instruction Instructions = new Instruction();
            Input inputs = new Input();

            bool IsAttempted = false;
       
            Instructions.InstructionsFile();
            inputs.UserPrompt(IsAttempted);
            
            System.Console.ReadLine();
        }
    }
}

