using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    [Header("References")]
    public RectTransform catchBar; // Con cá di chuyển ngẫu nhiên
    public RectTransform fishIcon; // Thanh do người chơi điều khiển
    public Image progressBar;

    [Header("Settings")]
    public float fishControlSpeed = 100f;
    public float catchBarMoveSpeed = 80f;
    public float progressRate = 0.3f;
    public float changeTargetTime = 1.5f;

    private float progress = 0f;
    private float timer = 0f;
    private Vector2 catchTargetPos;

    [SerializeField] Item Fish;

    void Start()
    {
        SetNewCatchTarget();
        progress = 0.2f;
        progressBar.fillAmount = progress;
    }
    void OnEnable()
    {
        if (progressBar != null)
        {
            progress = 0.2f;
            progressBar.fillAmount = progress;
        }
    }

    void Update()
    {
        
        catchBar.anchoredPosition = Vector2.MoveTowards(
            catchBar.anchoredPosition,
            catchTargetPos,
            catchBarMoveSpeed * Time.deltaTime
        );
        timer += Time.deltaTime;
        if (timer >= changeTargetTime)
        {
            SetNewCatchTarget();
            timer = 0f;
        }

        if (Input.GetKey(KeyCode.Space))
            fishIcon.anchoredPosition += Vector2.up * fishControlSpeed * Time.deltaTime;
        else
            fishIcon.anchoredPosition -= Vector2.up * fishControlSpeed * Time.deltaTime;

        // Clamp vùng di chuyển
        fishIcon.anchoredPosition = new Vector2(
            fishIcon.anchoredPosition.x,
            Mathf.Clamp(fishIcon.anchoredPosition.y, -183f, 170f)
        );

        if (Mathf.Abs(fishIcon.anchoredPosition.y - catchBar.anchoredPosition.y) < 55f)
            progress += progressRate * Time.deltaTime;
        else
            progress -= progressRate * Time.deltaTime;

        progress = Mathf.Clamp01(progress);
        progressBar.fillAmount = progress;

        if (progress >= 1f) CatchSuccess();
        else if (progress <= 0f) CatchFail();
    }

    void SetNewCatchTarget()
    {
        float currentY = catchBar.anchoredPosition.y;
        float newY;

        int safety = 0; 

        do
        {
            newY = Random.Range(-183f, 170f);
            safety++;
        }
        while (Mathf.Abs(newY - currentY) < 30f && safety < 10); 

        catchTargetPos = new Vector2(catchBar.anchoredPosition.x, newY);
    }

    void CatchSuccess()
    {
        InventoryManager.instance.addItem(Fish, 1);
        Debug.Log("🎉 Bắt được cá!");
        UIManager.Instance.StopFishing();
    }

    void CatchFail()
    {
        Debug.Log("💥 Cá thoát mất!");
        UIManager.Instance.StopFishing();
    }
}
