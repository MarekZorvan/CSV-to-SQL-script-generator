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

        public string MakeScript(string filePath)
        {
            string script = "";

            if (!string.IsNullOrEmpty(filePath) && filePath.Substring(filePath.Length - 4).Contains(permittedExtension))
            {
                // The file extension is valid

                // column names from headers, are expected to be in the 1st row of CSV file
                string[] firstRowValues;

                using (var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var reader = new StreamReader(file);

                    var parser = new CsvParser(reader);

                    // save headers
                    firstRowValues = parser.Read();

                    string commandStart = "INSERT INTO table(";

                    for (int i = 0; i < firstRowValues.Length - 1; i++)
                    {
                        commandStart += firstRowValues[i] + ", ";
                    }

                    commandStart += firstRowValues[firstRowValues.Length - 1] + ")\n";
                    commandStart += "VALUES\n";

                    // values of 1 row
                    string[] row = parser.Read();

                    while (row != null)
                    {
                        //TODO: ADD INSERT INTO SQL COMMAND TO variable script which will be returned

                        // INSERT INTO table(column1, column2, …)
                        // VALUES
                        // (value1, value2, …);

                        string command = commandStart + "(";

                        for (int i = 0; i < row.Length - 1; i++)
                        {
                            command += row[i] + ", ";
                        }

                        command += row[row.Length - 1] + ")\n;";

                        script += command;

                        row = parser.Read();
                    }
                }
            }

            return script;
        }
    }
}
