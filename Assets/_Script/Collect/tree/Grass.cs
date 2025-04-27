using Unity.VisualScripting;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] Item _Grass;
    private Quaternion originalRotation;
    private bool isWiggling = false,isplayer=false;
    [SerializeField] public bool _IsPlayerPlaced;
    [SerializeField] private float maxWiggleAngle = 10f;
    [SerializeField] private float wiggleSpeed = 30f;
    [SerializeField] private float wiggleDuration = 0.5f;
    [SerializeField] private GameObject GrassChipPrefab;

    void Start()
    {
        originalRotation = transform.rotation;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isplayer)
            {
                //InventoryManager.instance.addItem(_Grass, 1);
                Consume();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isplayer = true;
            if (!isWiggling)
            {          
                float direction = Mathf.Sign(other.transform.position.x - transform.position.x);
                StartCoroutine(Wiggle(direction));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isplayer = false;
        }
    }

    System.Collections.IEnumerator Wiggle(float direction)
    {
        isWiggling = true;
        float time = 0f;

        while (time < wiggleDuration)
        {
            float damping = 1f - (time / wiggleDuration);
            float angle = Mathf.Sin(time * wiggleSpeed) * maxWiggleAngle * damping;

            transform.rotation = originalRotation * Quaternion.Euler(0f, 0f, angle * direction);

            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = originalRotation;
        isWiggling = false;
    }
    public void Consume() {
        for (int i = 0; i < 5; i++)
        {
            GameObject drop = Instantiate(GrassChipPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
