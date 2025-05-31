using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class ObstacleRaycaster : MonoBehaviour
{
    public Transform raycastTop;
    public Transform raycastBottom;
    public float rayDistance = 10f;
    public LayerMask obstacleLayer;
    public TextMeshProUGUI messageText;
    public GameObject dialogPanel; // Gắn cả Panel (cha của text)

    private float cooldown = 0.5f;
    private float lastAlertTime = -999f;

    void Update()
    {
        if (Time.time - lastAlertTime < cooldown)
            return;

        if (raycastTop != null && Physics2D.Raycast(raycastTop.position, Vector2.right, rayDistance, obstacleLayer))
        {
            ShowMessageFrom("slide");
            Debug.Log("slide");
        }
        else if (raycastBottom != null && Physics2D.Raycast(raycastBottom.position, Vector2.right, rayDistance, obstacleLayer))
        {
            Debug.Log("jump");
            ShowMessageFrom("jump");
        }
    }

    private Image panelImage;
    private Color defaultColor;
    public GameSpeedFetcher speedFetcher; // Gán trong Unity Inspector
    void Start()
    {
        int savedScore = PlayerPrefs.GetInt("score", 0);
        speedFetcher.FetchSpeedFromScore(savedScore);

        // Lấy panel chứa messageText
        dialogPanel = messageText.transform.parent.gameObject;

        // Lấy component Image từ panel để đổi màu
        panelImage = dialogPanel.GetComponent<Image>();
        if (panelImage != null)
            defaultColor = new Color(0f, 1f, 0.66f);
    }

    Coroutine hideCoroutine;

    void ShowMessageFrom(string type)
    {
        string msg = NPCMessageStore.GetRandomMessage(type);
        messageText.text = msg;
        messageText.transform.parent.gameObject.SetActive(true);
        lastAlertTime = Time.time;

        // Đổi màu panel nếu là "slide"
        if (panelImage != null)
        {
            if (type == "slide")
                panelImage.color = new Color32(0, 11, 255, 255);
            else
                panelImage.color = defaultColor;
        }

        // Nếu đang có coroutine hide cũ → stop trước
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        // Bắt đầu coroutine mới để ẩn sau 1 giây
        hideCoroutine = StartCoroutine(HideMessageAfterDelay(1f));
    }

    IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.transform.parent.gameObject.SetActive(false);
        hideCoroutine = null; // reset
    }


    private void OnDrawGizmos()
    {
        if (raycastTop != null)
            Gizmos.DrawLine(raycastTop.position, raycastTop.position + Vector3.right * rayDistance);
        if (raycastBottom != null)
            Gizmos.DrawLine(raycastBottom.position, raycastBottom.position + Vector3.right * rayDistance);
    }
}
