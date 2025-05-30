using UnityEngine;
using System.Collections;
public class TreeDrop : MonoBehaviour,CanGethit
{
    [Header("Cấu hình cây")]
    public int maxHits = 5;
    public GameObject woodDropPrefab, ChopDropPrefab;
    public int woodDropCount = 3;
    public float dropForce = 4f;

    [Header("Hiệu ứng")]
    public float shakeAngle = 5f;
    public float shakeDuration = 0.1f;
    public float fallDistance = 1f;
    public float fallDuration = 3f;
    public Transform player;

    private int currentHits = 0;
    private bool isFalling = false, isShaking = false;
    private Collider2D col;

    public string id => transform.position.ToString();
    void Start()
    {
        col = GetComponent<Collider2D>();
        if (SaveManager.Instance != null && SaveManager.Instance.IsCollected(id))
        {   
            GetComponentInParent<CapsuleCollider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }

    public void Gethit()
    {        
        if (isFalling || player == null) return;
        currentHits++;
        Shake();
        if (currentHits >= maxHits)
        {         
            StartCoroutine(Fall());
            transform.parent.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
    public void Shake()
    {
        if (isFalling || isShaking)
            return;

        StartCoroutine(ShakeEffect());
    }
    IEnumerator ShakeEffect()
    {
        yield return new WaitForSeconds(0.4f);
        ChopDrop();
        AudioManager.Instance.PlaySFX(AudioManager.Instance._Cutdowntrees);
        Quaternion originalRotation = transform.localRotation;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float z = Random.Range(-shakeAngle, shakeAngle);
            transform.localRotation = Quaternion.Euler(0f, 0f, z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = originalRotation;
        
    }

    IEnumerator Fall()
    {
       
        isFalling = true;
        col.enabled = false;
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySFX(AudioManager.Instance._TreeFalling);
        float targetZ = (player.position.x < transform.position.x) ? -90f : 90f;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, 0, targetZ);

        float t = 0f;
        while (t < fallDuration)
        {
            float progress = t / fallDuration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, progress);
            t += Time.deltaTime;
            yield return null;
        }
        DropWood();
        gameObject.SetActive(false);
        SaveManager.Instance.RegisterCollected(id);
    }

    void DropWood()
    {
        Vector2 Droppos = (player.position.x < transform.position.x) ? new Vector2(2, 0) : new Vector2(-2, 0);
        for (int i = 0; i < woodDropCount; i++)
        {
            
            Vector2 pos = (Vector2)transform.position + Droppos + Random.insideUnitCircle * 0.5f;
            GameObject drop = Instantiate(woodDropPrefab, pos, Quaternion.identity);
            Rigidbody2D rb = drop.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 force = Random.insideUnitCircle.normalized * Random.Range(1f, dropForce);
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
    void ChopDrop()
    {
        for (int i = 0; i < 4; i++)     
        {
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * 0.5f;
            GameObject drop = Instantiate(ChopDropPrefab, pos, Quaternion.identity);
        }
    }
}   


