using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public int adjustSpeed = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: giữ nguyên qua scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
