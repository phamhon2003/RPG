using System.Collections;
using UnityEngine;

public class Seeds : MonoBehaviour
{
    float Grown = 0;
    SpriteRenderer _Sprite;
    public bool isReadyToHarvest=false,_IsHarvest=false;
    public Sprite _Stage, _Stage1, _Stage2, _Stage3, _Stage4;
    public float moveDuration = 0.5f;
    public float jumpHeight = 1f;
    [SerializeField] Item _Item;
    private Vector3 _StartPos;

    void Start()
    {
        _Sprite = GetComponent<SpriteRenderer>();
        _StartPos=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_IsHarvest) return;

        if (Grown <= 15)
        {
            Grown = Grown + 0.2f * Time.deltaTime;
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
        
        StartCoroutine(HarvestRoutine(Pos));
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
}
