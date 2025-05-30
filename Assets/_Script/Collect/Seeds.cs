using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Seeds : MonoBehaviour
{
    public string Nameprefab;

    public float Grown = 0;
    SpriteRenderer _Sprite;
    public bool isReadyToHarvest=false,_IsHarvest=false;
    public Sprite _Stage, _Stage1, _Stage2, _Stage3, _Stage4;
    public float moveDuration = 0.5f;
    public float jumpHeight = 1f;
    [SerializeField] Item _Item;
    private Vector3 _StartPos;


    private Quaternion originalRotation;
    private bool isWiggling = false;
    [SerializeField] private float maxWiggleAngle = 10f;
    [SerializeField] private float wiggleSpeed = 30f;
    [SerializeField] private float wiggleDuration = 0.3f;
    PlacedObjectData data;
    void Start()
    {
        originalRotation = transform.rotation;
        _Sprite = GetComponent<SpriteRenderer>();
        _StartPos=transform.position;
    }

    // Update is called once per frame
    void Update()
    {    
        if (_IsHarvest) return;

        if (Grown <= 15)
        {
            if (TimapsManager.instance.Iswatering(TimapsManager.instance.getpostile(transform.position))) Grown = Grown + 0.4f * Time.deltaTime;
            else Grown = Grown + 0.2f * Time.deltaTime; 
        }
        if (Grown > 3 && Grown <= 6)
        {
            _Sprite.sprite = _Stage;
            return;
        }
        if (Grown > 6 && Grown <= 9)
        {
            _Sprite.sprite = _Stage1;
            return;
        }
        if (Grown > 9 && Grown <= 12)
        {
            _Sprite.sprite = _Stage2;
            return;
        }
        if (Grown > 12 && Grown <= 15)
        {
            _Sprite.sprite = _Stage3;
            return;
        }
        if (Grown > 15 )
        {
            _Sprite.sprite = _Stage4;
            isReadyToHarvest=true;
        }
        
    }
    public void Harvest(Vector3 Pos)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance._Claim);
        StartCoroutine(HarvestRoutine(Pos));
        SaveManager.Instance.RemovePlacedObjectByPosition(transform.position);
        SaveManager.Instance.SaveDataInstatiate();
    }
    private IEnumerator HarvestRoutine(Vector3 Pos)
    {   
        _IsHarvest = true;
        _Sprite.sprite = _Item.image ;
        Vector3 startPos = transform.position;
        Vector3 endPos = Pos;
        Vector3 peakPos = startPos + Vector3.up * jumpHeight;

        float elapsed = 0f;
        Vector3 startScale = transform.localScale;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            Vector3 currentPos = Vector3.Lerp(Vector3.Lerp(startPos, peakPos, t), Vector3.Lerp(peakPos, endPos, t), t);
            transform.position = currentPos;

            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            yield return null;
        }
        TimapsManager.instance.HS.Remove(_StartPos);
        InventoryManager.instance.addItem(_Item, 1);
        Destroy(gameObject); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isWiggling && isReadyToHarvest)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance._SoundGrass);
                float direction = Mathf.Sign(other.transform.position.x - transform.position.x);
                StartCoroutine(Wiggle(direction));
            }
        }
    }
    
    IEnumerator Wiggle(float direction)
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
    void save()
    {
        foreach (PlacedObjectData objdata in SaveManager.Instance.placedObjects)
        {
            if (objdata.position == (Vector2)transform.position)
            {
                data = objdata;
            }
        }
        if (data == null)
        {
            data = new PlacedObjectData
            {
                prefabName = Nameprefab,
                position = transform.position,
                uniqueID = System.Guid.NewGuid().ToString(),
                grown = Grown,
            };
            SaveManager.Instance.placedObjects.Add(data);

        }
        else
        {
            Debug.Log("saveSeed");
            data.grown = Grown;
            SaveManager.Instance.UpdateSeedData(data);
        }
        SaveManager.Instance.SaveDataInstatiate();
    }
    void OnDisable()
    {
        save();
    }
    void OnApplicationQuit()
    {
        save();
    }
}
