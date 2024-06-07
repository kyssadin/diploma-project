using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.Analytics;

public class SendDataScript : MonoBehaviour
{

    IEnumerator Upload(string profile, System.Action<bool> callback = null)
    {
        using (UnityWebRequest request = new UnityWebRequest("https://eu-central-1.aws.data.mongodb-api.com/app/data-bfsrohd/endpoint/data/v1/action/insertOne", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/ejson");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("apiKey", "QrHsoZq0N7tHTfRIPnBPbMdHR7YKltGpOCYxgP0WWA1sYWjBLjMKzmDpF7GE3Lpc");

            byte[] bodyRaw = Encoding.UTF8.GetBytes(profile);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if (callback != null)
                {
                    callback.Invoke(false);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke(request.downloadHandler.text != "{}");
                }
            }
        }
    }

    [System.Serializable]
    public class ScoreData
    {
        public string dataSource;
        public string database;
        public string collection;
        public DocumentData document;
    }

    [System.Serializable]
    public class DocumentData
    {
        public float score;
        public int correctGuesses;
        public float timeElapsed;
        public string deviceID;
        public long sessionID;
        public int levelID;
        public float difficultyID;
    }

    public void UploadButtonClick()
    {
        LevelManager manager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();

        float score = manager.currentScore;
        int corrects = manager.correctGuesses;
        float totalTime = manager.totalTimeElapsed;
        string device = SystemInfo.deviceUniqueIdentifier;
        long session = AnalyticsSessionInfo.sessionId;
        int level = manager.isLevel2 ? 2 : 1;
        float difficulty = manager.difficulty;
        var scoreData = new ScoreData
        {
            dataSource = "Cluster0",
            database = "ScoresDB",
            collection = "scores",
            document = new DocumentData
            {
                score = score,
                correctGuesses = corrects,
                timeElapsed = totalTime,
                deviceID = device,
                sessionID = session,
                levelID = level,
                difficultyID = difficulty
            }
        };

        string jsonProfile = JsonUtility.ToJson(scoreData);

        StartCoroutine(Upload(jsonProfile, (success) =>
        {
            if (success)
            {
                Debug.Log("Upload successful!");
            }
            else
            {
                Debug.Log("Upload failed.");
            }
        }));
    }
}
