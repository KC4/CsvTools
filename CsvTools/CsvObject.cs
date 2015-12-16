using System;
using System.IO;
using System.Net;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvTools
{
    public class CsvObject
    {
        //The Csv in before parsing, for now
        //Eventually will be sanitized csv
        private string Csv;
        //The Data table that holds the csv data
        private DataTable dataStore;

        /// <summary>
        /// Create a new CsvObject
        /// </summary>
        /// <param name="csvText"></param>
        public CsvObject(string csvText)
        {
            dataStore = new DataTable();
            Csv = csvText;
            Parse();
        }

        /// <summary>
        /// Convert the datatable to quoted csv format
        /// </summary>
        /// <remarks>UNIMPLEMENTED</remarks>
        /// <param name="quoted"></param>
        /// <returns></returns>
        public string ToCSV(bool quoted)
        {
            throw new ApplicationException("Not Yet Implemented");
        }

        /// <summary>
        /// Get the data table from the object
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            return dataStore;
        }

        /// <summary>
        /// Load a csv file into an object
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static CsvObject Load(string filePath)
        {
            if(File.Exists(filePath))
            {
                var csvFileText = File.ReadAllText(filePath);
                return new CsvObject(csvFileText);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parse the new csv string
        /// </summary>
        private void Parse()
        {
            //Create a new string reader
            var stringReader = new StringReader(Csv);
            //Keep the rows value
            var row = "";
            //Separate the header and the rows
            var isHeader = true;
            //Iterate over the lines
            while((row = stringReader.ReadLine()) != null)
            {
                //If it's the header
                if(isHeader)
                {
                    ParseHeader(row);
                    isHeader = false;
                }
                //If it's a body row
                else
                {
                    ParseRow(row);
                }
            }
            stringReader.Close();
        }

        /// <summary>
        /// Convert a csv row into a data table row
        /// </summary>
        /// <param name="row"></param>
        private void ParseRow(string row)
        {
            //Keep track of whether or not we're in a quote
            var quote = false;
            //Get a new blank row
            var newRow = dataStore.NewRow();
            //Hold the value of the current row
            var rowItem = "";
            //Keep track of which column in the row we're writing to 
            var location = 0;
            //Loop through all the characters in the row
            for(int i = 0; i < row.Length; ++i)
            {
                //If it's a quotation mark
                if (row[i] == '"')
                {
                    //Toggle the quote flag
                    quote = !quote;
                }
                //If it's a comma outside of a quote
                else if ((row[i] == ',') && !quote)
                {
                    //Add the value to the row
                    newRow.SetField(location, rowItem.Trim());
                    //Move to the next column
                    ++location;
                    //Reset the Row item
                    rowItem = "";
                }
                //Otherwise
                else
                {
                    //Add the character to the array
                    rowItem += row[i];
                }
            }
            //If there is still something left
            if (rowItem != "")
            {
                //Add it to the row
                newRow.SetField(location, rowItem.Trim());
                ++location;
                rowItem = "";
            }
            //Add it to the table
            dataStore.Rows.Add(newRow);
        }

        /// <summary>
        /// Convert header values to column names
        /// </summary>
        /// <param name="header"></param>
        private void ParseHeader(string header)
        {
            //Track if we're looking inside of quotes
            var quote = false;
            //Hold the value of the current column
            var columnName = "";
            //Loop through all the characters
            for(int i = 0; i < header.Length; ++i)
            {
                //If it's a quotation mark
                if(header[i] == '"')
                {
                    //Toggle the quote flag
                    quote = !quote;
                }
                //If it's a comma and not inside of a quote
                else if((header[i] == ',') && !quote)
                {
                    //Add the column to the data store
                    dataStore.Columns.Add(columnName.Trim(), typeof(string));
                    //And clear the colum name holder
                    columnName = "";
                }
                //Otherwise
                else
                {
                    //Add the character to the columnName
                    columnName += header[i];
                }
            }
            // If there is still a column name to add
            if(columnName != "")
            {
                //Add the Column to the data store
                dataStore.Columns.Add(columnName.Trim(), typeof(string));
            }
        }
    }
}
