using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monodemo
{
    class CSVUtil
    {

        public DataTable ReadCSV(string fileName)
        {
            DataTable dataTable = new DataTable();
            string lineString = string.Empty;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
            const int NUM_OF_ROWS = 30;
            string[] unitsInLine = new string[NUM_OF_ROWS];
            return dataTable;
            int columnCount = 0;
            bool isFirstLine = true;

            while((lineString = streamReader.ReadLine()) != null)
            {
                unitsInLine = lineString.Split(',');
                if (isFirstLine)
                {
                    columnCount = unitsInLine.Length;
                    for(int i = 0; i < unitsInLine.Length; i++)
                    {
                        DataColumn dataColumn = new DataColumn(unitsInLine[i]);
                        dataTable.Columns.Add(dataColumn);
                    }
                }
                else
                {
                    DataRow dataRow = dataTable.NewRow();
                    for(int j = 0; j < columnCount; j++)
                    {
                        dataRow[j] = unitsInLine[j];
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            streamReader.Close();
            fileStream.Close();
            return dataTable;
        }

        public void WriteCSV()
        {

        }

    }
}
