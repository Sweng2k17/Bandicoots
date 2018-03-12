// Name of Updater: Pat Mac Millan
// Last Updated:    March 12, 2018

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class CSVWriter
{
    string filepath;
    StringBuilder csvContent; // this is the csv file
    
    /// <summary>
    /// Creates a CSV file with the given filepath. Initializes the StringBuilder object which will
    /// be utilized to create the CSV file and then appends the first row to delineate the columns.
    /// </summary>
    /// <param name="filepath"></param>     File path of the CSV file
    public CSVWriter(string filepath)
    {
        this.filepath = filepath;

        csvContent = new StringBuilder();
        // write first row:
        csvContent.AppendLine("ID,Time,X Position,Y Position,Z Position");

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
    }
 
}
