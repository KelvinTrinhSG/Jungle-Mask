using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class NPCWebhookCaller : MonoBehaviour
{
    public TextMeshProUGUI myText;
    public GameObject bg_1;
    public GameObject playButton;
    public GameObject exitButton;

    void Start()
    {
        // Lấy điểm số từ PlayerPrefs và gọi webhook
        int score = PlayerPrefs.GetInt("score_mask", 0);
        CallNPCWebhook(score);
    }

    public void CallNPCWebhook(int score)
    {
        StartCoroutine(GetNPCMessage(score));
    }

    IEnumerator GetNPCMessage(int score)
    {
        string url = $"https://n8n-1-rayu.onrender.com/webhook/npc-score-response?score={score}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string json = req.downloadHandler.text;

            // JSON là mảng nên dùng wrapper
            string wrappedJson = "{ \"list\": " + json + " }";
            NPCResponseList wrapper = JsonUtility.FromJson<NPCResponseList>(wrappedJson);

            string message = wrapper.list[0].output;
            Debug.Log("NPC says: " + message);
            if (myText != null && myText.gameObject != null)
            {
                myText.text = message;
            }
            bg_1.SetActive(true);
            playButton.SetActive(true);
            exitButton.SetActive(true);
            // Gọi NPC hiển thị câu này nếu cần
            // npcMessageDisplay.ShowMessage(message);
        }
        else
        {
            Debug.LogError("Failed to fetch NPC message: " + req.error);
        }
    }

    [System.Serializable]
    public class NPCResponseList
    {
        public List<NPCResponse> list;
    }

    [System.Serializable]
    public class NPCResponse
    {
        public string output;
    }

}
