using System;
using UnityEngine;
using System.Collections;


public class WorldTime : MonoBehaviour
{
    public event EventHandler<TimeSpan> WorldTimeChange;
    [SerializeField] private float _daylength;
    
    private TimeSpan _currentTime;
    private float _minutelength => (_daylength/WorldTimeConstants.MinutesInDay)*7;

    private void Start()
    {
        StartCoroutine(Addminute());
    }
    private IEnumerator Addminute()
    {
        _currentTime += TimeSpan.FromMinutes(1);
        WorldTimeChange?.Invoke(this, _currentTime);    
        yield return new WaitForSeconds(_minutelength);
        StartCoroutine(Addminute());
    }
}
