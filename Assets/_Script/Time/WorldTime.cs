using System;
using UnityEngine;
using System.Collections;


public class WorldTime : MonoBehaviour
{
    public event EventHandler<TimeSpan> WorldTimeChange;
    [SerializeField] private float _daylength;
    [SerializeField] float TimeSpeed=7;
    private TimeSpan _currentTime;
    private TimeSpan _previousTime;
    private float _minutelength => (_daylength/WorldTimeConstants.MinutesInDay)* TimeSpeed;
    private const string SaveKey = "WorldTime";
    private void Awake()
    {
        string savedTime = PlayerPrefs.GetString(SaveKey, "08:00");
        _currentTime = TimeSpan.Parse(savedTime);
    }

    private void Start()
    {
        StartCoroutine(Addminute());
    }
    private IEnumerator Addminute()
    {
        _previousTime = _currentTime;
        _currentTime += TimeSpan.FromMinutes(1);
        _currentTime = _currentTime.TotalMinutes >= WorldTimeConstants.MinutesInDay
                        ? TimeSpan.Zero : _currentTime; // Reset về 00:00 nếu hết ngày

        if (_previousTime.TotalMinutes > _currentTime.TotalMinutes)
        {   if(LoadSceneStatic.nextSceneName=="SampleScene")
            TimapsManager.instance.ResetTileWasWatering();
        }
        WorldTimeChange?.Invoke(this, _currentTime);
        
        yield return new WaitForSeconds(_minutelength);
        StartCoroutine(Addminute());
    }
    private void OnDisable()
    {
        SaveTime();
    }

    private void OnApplicationQuit()
    {
        SaveTime();
    }

    private void SaveTime()
    {
        PlayerPrefs.SetString("WorldTime", _currentTime.ToString(@"hh\:mm"));
        PlayerPrefs.Save();
    }
}
