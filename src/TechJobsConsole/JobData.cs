using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TechJobsConsole
{
    class JobData
        // imports data from CSV and parses it into list <dictionary> form.
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

        public static List<Dictionary<string, string>> FindAll()
        {
            LoadData();
            return AllJobs;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                string aValue = job[column];


                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            }
            return values;
        }

        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            // column is variable for user input for key in key value pairs
            // value is variable for user input for search term

            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            // loops through each dictionary/job, foreach job, looks for column,
            // assigns that column the variable aValue, then searches for value/ search term
            // Then adds that entire dictionary/job/row to the jobs list

            foreach (Dictionary<string, string> row in AllJobs)
            {
                string aValue = row[column].ToLower();

                if (aValue.Contains(value.ToLower()))
                {
                    jobs.Add(row);
                }
            }

            return jobs;
        }

        public static List<Dictionary<string, string>> FindByValue(string value)
        {
            LoadData();

            // LoadData returns list of dictionaries of all jobs in CSV
            // value is variable for user input for search term, like in FindByColumnAndValue

            List< Dictionary <string, string>> jobs = new List< Dictionary<string, string>> ();

            // loops through each dictionary/job, foreach job, looks for value,
            // loops through each kvp in each row
            // Then adds that entire dictionary/job/row to the jobs list
            // looks for
            string aValue = value.ToLower();
            foreach (Dictionary<string, string> row in AllJobs)
            {
                foreach (var kvp in row)
                {
                    if (kvp.Value.ToLower().Contains(aValue) && !jobs.Contains(row))
                    {
                        jobs.Add(row);
                    }
                }
            }

            return jobs;
        }




        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
            // it loads data and stores it into AllJobs ( list <dictionary> )
            // Thanks, Blake 
        {

            if (IsDataLoaded)
            {
                return;
                // why not continue? We aren't returning anything?
            }

            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArrray = CSVRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                AllJobs.Add(rowDict);
            }

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }
    }
}


/* Here are some questions to ask yourself while reading this code:

    What is the data type of a job?
        dictionary of string key : string value

    Why does FindAll(string) return something of type List<string>
        it's returning a list of column values
        
    
    while FindByColumnAndValue(string, string) and FindAll() return something
    of type List<Dictionary<string, string>>?
        they're returning full job/row dictionaries


    Why is LoadData() called at the top of each of these four methods?
    Does this mean that we load the data from the CSV file each time one of them is called?
        It checks to make sure data is loaded first, so not necessarily.

*/