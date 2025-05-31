using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMessageData
{
    public List<string> jumpMessages;
    public List<string> slideMessages;
}

public static class NPCMessageStore
{
    private static NPCMessageData cachedData;

    public static string GetRandomMessage(string type)
    {
        if (cachedData == null)
        {
            string json = PlayerPrefs.GetString("npcMessages", "");
            if (!string.IsNullOrEmpty(json))
                cachedData = JsonUtility.FromJson<NPCMessageData>(json);
        }

        List<string> list = (type == "jump") ? cachedData.jumpMessages : cachedData.slideMessages;
        if (list != null && list.Count > 0)
            return list[Random.Range(0, list.Count)];

        return "[Missing message]";
    }
}
