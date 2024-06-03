using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveResultsButtonScript : MonoBehaviour
{
    private string filePath;

    void Awake()
    {
        filePath = Application.persistentDataPath + "/Data/Data.json";
    }

    public void SaveResultsToDesktop()
    {
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string fileName = Path.GetFileName(filePath);
        string destinationPath = Path.Combine(desktopPath, fileName);
        if (File.Exists(filePath))
        {
            File.Copy(filePath, destinationPath, true);
            Debug.Log("Copied the results.");
        }
        else
        {
            Debug.Log("No results file.");
        }
    }
}
