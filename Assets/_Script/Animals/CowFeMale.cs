using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class CowFeMale : Cow
{
    public bool isBreedingNow = false;
    protected override void Start()
    {
        base.Start();
        gender = CowGender.Female;
    }


    public void TriggerBreed()
    {
        if (!isReadyToBreed || Time.time - lastBreedTime < breedCooldown)
            return;

        lastBreedTime = Time.time;
        isReadyToBreed = false;
        StartCoroutine(BreedRoutine());
    }

    private IEnumerator BreedRoutine()
    {
        isBreedingNow = true;
        isReadyToBreed = false;

        if (MoveRandom != null)
        {
            StopCoroutine(MoveRandom);
            MoveRandom = null;
        }

        SetIdleAnimation();
        rb.linearVelocity = Vector2.zero;
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(20f); 
        Vector3 spawnPos = transform.position + new Vector3(0.5f, 0.5f, 0);
        Instantiate(_babyCowPrefab, spawnPos, Quaternion.identity);

        isBreedingNow = false;
        transform.GetChild(0).gameObject.SetActive(false);
        if (MoveRandom == null)
            MoveRandom = StartCoroutine(MoveRandomly());
    }
}
