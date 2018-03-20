// Name of Updater: Pat Mac Millan
// Last Updated:    March 20, 2018

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class CSVWriter
{
    private string filepath;
    private StringBuilder csvContent; // this is the csv file
    
    /// <summary>
    /// Initializes CSVWriter object
    /// </summary>
    public CSVWriter()
    {
        newCSV();
    }

    /// <summary>
    /// Sets the filepath of where the CSV file will be written to.
    /// </summary>
    /// <param name="filepath"></param>     File path of the CSV file
    public void setFilePath(string filepath)
    {
        this.filepath = filepath;
    }

    /// <summary>
    /// Returns the file path
    /// </summary>
    /// <returns></returns>
    public string getFilePath() 
    {
        if (filepath != null)
            return filepath;
        else
            throw new NullStringException("File path was not defined.");
    }    
    
    /// <summary>
    /// Appends lines to the CSV file with the given fields. 
    /// </summary>
    /// <param name="id"></param>       The ID string of the target
    /// <param name="time"></param>     Time the target was spotted
    /// <param name="xPos"></param>     X position where the target was spotted
    /// <param name="yPos"></param>     Y position where the target was spotted
    /// <param name="zPos"></param>     Z position where the target was spotted
    public void appendCSV(string id, double time, float xPos, float yPos, float zPos)
    {
        csvContent.AppendLine(id + "," 
                            + time.ToString() + "," 
                            + xPos.ToString() + "," 
                            + yPos.ToString() + "," 
                            + zPos.ToString());
    }
    
    /// <summary>
    /// Writes the csv file created to the specified location delineated by the filepath.
    /// Creates a filename called detectiondata_[date]_[time]
    /// </summary>
    public void writeFile()
    {
        Debug.Log("Saving file");

        string fileName = "Detection_Data_" + System.DateTime.Now.ToString("MMMd_HHmm") + ".csv";
        // MAKE SURE TO CHANGE BACKSLASH TO FORWARDSLASH SO IT WORKS IN LINUX
        string writePath = filepath + "\\" + fileName;

        BinaryFormatter bf = new BinaryFormatter();

        using (var file = File.Open(writePath, FileMode.OpenOrCreate))
        {
            bf.Serialize(file, csvContent);
        }

        Debug.Log("File saved");
        
        // Overwrite csvContent for continued use.
        newCSV();
    }
    
    /// <summary>
    /// Initializes the StringBuilder object 
    /// which will be utilized to create the CSV file and then 
    /// appends the first row to delineate the columns.
    /// </summary>
    private void newCSV()
    {
        csvContent = new StringBuilder();
        // write first row:
        csvContent.AppendLine("ID,Time,X Position,Y Position,Z Position");
    }
}

// When the file path is not defined.
public class NullStringException: Exception
{
    public NullStringException(string message)
    {
        Debug.Log(message);
    }
}
