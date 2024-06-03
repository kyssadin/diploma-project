using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

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

    public void UploadButtonClick()
    {
        string jsonProfile = @"
        {
            ""dataSource"": ""Cluster0"",
            ""database"": ""ScoresDB"",
           ""collection"": ""scores"",
            ""document"": {
               ""score"": ""1"",
               ""timeElapsed"": ""1""
           }
        }";
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
