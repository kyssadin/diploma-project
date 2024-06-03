using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class ResultYieldScript
{
    private static string filePath = Application.persistentDataPath + "/Data/Data.json";
    public static ResultsList GetData()
    {
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }
        if (!File.Exists(filePath))
        {
            string jsonData = "{}";
            File.WriteAllText(filePath, jsonData);
            return new ResultsList();
        }
        else
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<ResultsList>(json);
        }
    }

    public static void SaveData(ResultsList resultList)
    {
        string jsonData = JsonUtility.ToJson(resultList, true);
        File.WriteAllText(filePath, jsonData);
    }

    public static ResultsList UpdateData(ResultsList resultsList, ResultsDataHolder currentResult)
    {
        ResultsDataHolder[] results = new ResultsDataHolder[10];
        if (resultsList.values == null)
        {
            results[0] = currentResult;
            for (int i = 1; i < 10; i++)
            {
                results[i] = new() { score = 0, timeElapsed = 0 };
                
            }
            return new ResultsList { values = results };
        }
        bool added = false;
        int k = 0;
        for (int i = 0; i < resultsList.values.Length && k < 10; i++)
        {
            if (added || resultsList.values[i].score > currentResult.score)
            {
                results[k] = resultsList.values[i];
            }
            else if (resultsList.values[i].score == currentResult.score && resultsList.values[i].timeElapsed <= currentResult.timeElapsed)
            {
                results[k] = resultsList.values[i];
            }
            else
            {
                results[k] = currentResult;
                added = true;
                i--;
            }
            k++;
        }
        return new ResultsList { values = results };
    }
}
