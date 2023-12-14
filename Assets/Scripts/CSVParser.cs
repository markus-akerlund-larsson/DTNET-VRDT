using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class CSVParser
{
    public List<string[]> rowData { get; private set; } // Store parsed CSV data here

    public CSVParser(string filePath)
    {
        rowData = new List<string[]>();
        // Load the CSV file as a TextAsset from the Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);

        Regex lineSplitter = new Regex("\n(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

        // Split the CSV file's contents into rows using the newline character as a delimiter
        string[] rows = lineSplitter.Split(csvFile.text);

        Debug.Log(rows.Length);

        // Create a regular expression to match cells that are encapsulated in quotes
        Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

        // Loop through each row
        for (int i = 0; i < rows.Length; i++)
        {
            // Split the row into cells using the regular expression
            string[] cells = csvParser.Split(rows[i]);

            // Remove any quotes from the beginning and end of each cell
            for (int j = 0; j < cells.Length; j++)
            {
                cells[j] = cells[j].Trim('\"');
            }

            // Add the cells to the rowData list
            rowData.Add(cells);
        }
    }
}