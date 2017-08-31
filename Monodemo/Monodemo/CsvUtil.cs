﻿using System;
using System.Data;
using System.IO;
using System.Diagnostics;
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
            //read csv data to datatable
            DataTable dataTable = new DataTable();
            string lineString = string.Empty;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
            const int NUM_OF_COLS = 5;
            string[] unitsInLine = new string[NUM_OF_COLS];//number of columns
            int columnCount = 0;
            bool isFirstLine = true;

            while ((lineString = streamReader.ReadLine()) != null)//read a line
            {
                unitsInLine = lineString.Split(',');
                if (isFirstLine == true)
                {
                    columnCount = unitsInLine.Length;
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dataColumn = new DataColumn(unitsInLine[i]);
                        dataTable.Columns.Add(dataColumn);
                    }
                    isFirstLine = false;
                }
                else
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dataRow[j] = unitsInLine[j];
                        Debug.WriteLine(dataRow[j]);
                        Debug.WriteLine(unitsInLine[j]);
                        
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