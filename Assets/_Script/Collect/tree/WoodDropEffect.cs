using UnityEngine;
using System.Collections;
public class WoodDropEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float bounceHeight = 0.5f;
    public float dropTime = 0.4f;
    public float floatDistance = 0.5f; // Độ văng ngang
    public AnimationCurve dropCurve;   // Đường cong bật

    private Vector3 startPos;
    private Vector3 targetPos;
    private float elapsed = 0f;
    [SerializeField]Item Wood;
    void Start()
    {
        // Tính hướng ngẫu nhiên để văng ra
        Vector2 randomDir = Random.insideUnitCircle.normalized * floatDistance;
        startPos = transform.position;
        targetPos = startPos + new Vector3(randomDir.x, 0f, 0f); // chỉ văng ngang

        // Chạy hiệu ứng rơi
        StartCoroutine(DropEffect());
    }

    IEnumerator DropEffect()
    {
        while (elapsed < dropTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dropTime);

            // Lerp giữa vị trí
            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);

            // Thêm độ cao theo đường cong bounce
            float height = dropCurve.Evaluate(t) * bounceHeight;
            pos.y += height;

            transform.position = pos;

            yield return null;
        }

        transform.position = targetPos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.instance.addItem(Wood, 1);
            Destroy(gameObject);
        }
    }
}
