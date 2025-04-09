using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight : MonoBehaviour
{
    private Light2D _light;
    [SerializeField] WorldTime _WorldTime;
    [SerializeField] Gradient _Gradient;
    private void Start()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        mat.shader = Shader.Find("Custom/VoronoiWater");

        if (mat.shader == null)
        {
            Debug.LogError("Shader Custom/VoronoiWater không tìm thấy!");
        }
    }
    //private void Awake()
    //{
    //    _light = GetComponent<Light2D>();
    //    _WorldTime.WorldTimeChange += OnWorldTimeChange;
    //}
    //private void OnDestroy()
    //{
    //    _WorldTime.WorldTimeChange -= OnWorldTimeChange;
    //}
    //private void OnWorldTimeChange(object sender, TimeSpan newTime)
    //{
    //    _light.color = _Gradient.Evaluate(PercentOfDay(newTime));
    //}
    //private float PercentOfDay(TimeSpan timeSpan)
    //{
    //    return (float)timeSpan.TotalMinutes % WorldTimeConstants.MinutesInDay / WorldTimeConstants.MinutesInDay;
    //}
    //void Update()
    //{

    //}
}
