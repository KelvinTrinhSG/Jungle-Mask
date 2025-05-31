using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NPCWebhookTester : MonoBehaviour
{
    [Header("Webhook URL - use production, not /webhook-test/")]
    public string webhookUrl = "https://n8n-1-rayu.onrender.com/webhook/npc-init";

    void Start()
    {
        StartCoroutine(TestWebhook());
    }

    IEnumerator TestWebhook()
    {
        Debug.Log("🛰 Sending GET to: " + webhookUrl);

        UnityWebRequest request = UnityWebRequest.Get(webhookUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("✅ Webhook response received:\n" + json);

            // Save to PlayerPrefs for later use
            PlayerPrefs.SetString("npcMessages", json);
            PlayerPrefs.Save();
            Debug.Log("💾 NPC messages saved to PlayerPrefs.");
        }
        else
        {
            Debug.LogError("❌ Webhook request failed: " + request.error);
        }
    }
}
