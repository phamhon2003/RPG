using System;
using UnityEngine;
using TMPro;

public class WorldTimeDisplay : MonoBehaviour
{
    [SerializeField] private WorldTime _WorldTime;
    private TextMeshProUGUI _Text;
    private void Awake()
    {
        _Text = GetComponent<TextMeshProUGUI>();
        _WorldTime.WorldTimeChange += OnWorldTimeChange;
    }
    private void OnDestroy()
    {
        _WorldTime.WorldTimeChange -= OnWorldTimeChange;
    }
    private void OnWorldTimeChange(object sender, TimeSpan newTime)
    {
        _Text.SetText(newTime.ToString(@"hh\:mm"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
