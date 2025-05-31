using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameSpeedFetcher : MonoBehaviour
{
    [SerializeField] private string webhookUrl = "https://n8n-1-rayu.onrender.com/webhook/game-speed";

    public void FetchSpeedFromScore(int score)
    {
        StartCoroutine(SendScoreToWebhook(score));
    }

    private IEnumerator SendScoreToWebhook(int score)
    {
        string urlWithQuery = webhookUrl + "?score=" + score;

        using (UnityWebRequest request = UnityWebRequest.Get(urlWithQuery))
        {
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string rawJson = request.downloadHandler.text;

                // Gói lại mảng thành object JSON để JsonUtility xử lý được
                string wrappedJson = "{ \"items\": " + rawJson + " }";

                OutputListWrapper result = JsonUtility.FromJson<OutputListWrapper>(wrappedJson);

                if (result != null && result.items.Length > 0)
                {
                    string outputStr = result.items[0].output;

                    if (int.TryParse(outputStr, out int speed))
                    {
                        Debug.Log("Received speed: " + speed);

                        if (GameSettings.Instance != null)
                        {
                            GameSettings.Instance.adjustSpeed = speed;
                            Debug.Log("GameSettings.adjustSpeed set to: " + speed);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Failed to parse output from JSON.");
                }
            }
            else
            {
                Debug.LogError("Webhook request failed: " + request.error);
            }
        }
    }
}

[System.Serializable]
public class OutputItem
{
    public string output;
}

[System.Serializable]
public class OutputListWrapper
{
    public OutputItem[] items;
}
