using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScriptGenerator
{
    public class Generator
    {
        private static string permittedExtension = ".csv";
        public Generator()
        {

        }

        public string MakeScript(string filePath, string tableName)
        {
            string script = "";

            if (!string.IsNullOrEmpty(filePath) && filePath.Substring(filePath.Length - 4).Contains(permittedExtension))
            {
                // The file extension is valid

                // column names from headers, are expected to be in the 1st row of CSV file
                string[] firstRowValues;


                using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var reader = new StreamReader(file, System.Text.Encoding.UTF8);

                    var parser = new CsvParser(reader);
                    parser.Configuration.Encoding = System.Text.Encoding.UTF8;
                    //parser.Configuration.Delimiter = ",";

                    // save headers
                    firstRowValues = parser.Read();

                    string commandStart = "INSERT INTO " + tableName + "(";

                    for (int i = 0; i < firstRowValues.Length - 1; i++)
                    {
                        commandStart += "\"" + firstRowValues[i] + "\"" + ", ";
                    }

                    commandStart += "\"" + firstRowValues[firstRowValues.Length - 1] + "\")\n";
                    commandStart += "VALUES\n\t";

                    // values of 1 row
                    string[] row = parser.Read();

                    while (row != null)
                    {
                        string command = commandStart + "(";

                        for (int i = 0; i < firstRowValues.Length; i++)
                        {
                            if (firstRowValues[i] == "createdAt" || firstRowValues[i] == "updatedAt")
                            {
                                command += "now(), ";
                            }
                            else
                            {

                                // fix apostrophe in string for SQL command
                                string fixedRow = "";
                                foreach (var ch in row[i])
                                {
                                    fixedRow += ch;
                                    if (ch == '\'')
                                    {
                                        fixedRow += "'";
                                    }
                                }
                                command += "'" + fixedRow + "', ";
                            }
                        }

                        command = command.Remove(command.Length - 2);

                        command += ");\n";


                        script += command;

                        row = parser.Read();
                    }
                }
            }

            return script;
        }
    }
}
