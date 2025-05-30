using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CowMale : Cow
{
    float distance=5f;
    CowFeMale _female;
    Coroutine MoveToFemale;
    protected override void Start()
    {
        base.Start();
        gender = CowGender.Male;
    }

    protected override void Update()
    {
        base.Update();
        if(isReadyToBreed) CheckForMate();
        if(_female != null && Grown >=1 && _female.Grown>=1)
        {
            if (MoveToFemale == null)
                MoveToFemale = StartCoroutine(MoveToFemaleAndBreed());
            distance=Vector2.Distance(transform.position, _female.transform.position);
        }else
        { distance = 5f; }
    }

    private void CheckForMate()
    {
        
        if (!isReadyToBreed || Time.time - lastBreedTime < breedCooldown)
            return;
        _female=null;
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (var col in nearby)
        {
            CowFeMale female = col.GetComponent<CowFeMale>();
            if (female != null && female.isReadyToBreed)
            {
                _female = female;
                break;
            }
        }
    }
    private IEnumerator MoveToFemaleAndBreed()
    {   
        if(_female == null) yield break;
        isReadyToBreed = false;
        if (MoveRandom != null)
        {
            StopCoroutine(MoveRandom);
            MoveRandom = null;
        }
        List<Node> path = Pathfinding.FindPath((Vector2)transform.position, (Vector2)_female.transform.position);
        if (path == null || path.Count == 0)
        {
            yield break;
        }
        foreach (Node node in path)
        {
            if ( _female == null || distance < 1.3f)
            {
                break;
            }
            Vector2 pos = GridSystem.Instance.GridToWorld(node.gridPos);
            yield return StartCoroutine(MoveToPosition(pos));
        }

       
        if (distance < 1.3f && _female.isReadyToBreed)
        {
            Debug.Log(distance);
            _female.isBreedingNow = true;
            _female.TriggerBreed();
            rb.linearVelocity = Vector2.zero;
            SetIdleAnimation();
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(20f);
            lastBreedTime = Time.time;
            if (MoveRandom == null)
                MoveRandom = StartCoroutine(MoveRandomly());
        }
        else
        {
            Debug.Log("Bò cái không còn trong phạm vi hoặc không sẵn sàng sinh");
            isReadyToBreed = true; // Cho phép bò đực thử lại sau
            if (MoveRandom == null)
                MoveRandom = StartCoroutine(MoveRandomly());
        }
        transform.GetChild(0).gameObject.SetActive(false);
        _female = null;
        MoveToFemale = null;
    }
}
