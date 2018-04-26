/*
	CSVReader by Dock. (24/8/11)
	http://starfruitgames.com
 
	usage: 
	CSVReader.SplitCsvGrid(textString)
 
	returns a 2D string array. 
 
	Drag onto a gameobject for a demo of CSV parsing.
*/

using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Experimental.UIElements;
using System.IO;

public class CSVReader : MonoBehaviour
{
    public InputField filePath;
    //public Text testing;

    public string[,] data;

    public void Start()
    {


        //used for debugging
        //uitext.text =  DebugOutputGrid(grid);
    }

    // outputs the content of a 2D array, useful for checking the importer
    static public string DebugOutputGrid(string[,] grid)
    {
        string textOutput = "";
        for (int y = 0; y < grid.GetUpperBound(1); y++)
        {
            for (int x = 0; x < grid.GetUpperBound(0); x++)
            {

                textOutput += grid[x, y];
                textOutput += "|";
            }
            textOutput += "\n";
        }


        Debug.Log(textOutput);
        return textOutput;
    }

    // splits a CSV file into a 2D string array
    public string[,] SplitCsvGrid(string csvText)
    {
        string[] lines = csvText.Split("\n"[0]);

        // finds the max width of row
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        // creates new 2D string grid to output to
        string[,] outputGrid = new string[width, lines.Length];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCsvLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];

                // This line was to replace "" with " in my output. 
                // Include or edit it as you wish.
                outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
            }
        }

        return outputGrid;
    }

    // splits a CSV row 
    public string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
        System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }


    public void ReadCSV()
    {
        string path = filePath.text;
        string fileText = "";

        data = null;

        try
        {
            //path = "C:\\Users\\Brian\\Documents\\GitHub\\StartSheet.csv";
            StreamReader reader = new StreamReader(path);
            fileText += reader.ReadToEnd();
            data = SplitCsvGrid(fileText);
            //testing.text = DebugOutputGrid(data);
            //Debug.Log("size = " + (1 + data.GetUpperBound(0)) + "," + (1 + data.GetUpperBound(1)));
            reader.Close();
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("Error in file read " + path);
        }
        catch(IOException e)
        {
            Debug.Log("The file is open in another program");
        }
        catch(System.ArgumentException e)
        {
            Debug.Log("Empty Path Not Allowed");
        }

        
    }
}
