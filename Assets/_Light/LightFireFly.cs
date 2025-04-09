using System.Collections.Generic;
using UnityEngine;

public class LightFireFly : MonoBehaviour
{
    public GameObject lightPrefab; 
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private List<GameObject> lightPool = new List<GameObject>(); 

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
        Debug.Log(ps.main.maxParticles);
        for (int i = 0; i < 20; i++)
        {
            GameObject lightObj = Instantiate(lightPrefab, transform);
            lightObj.SetActive(false); // Tắt đi để chờ sử dụng
            lightPool.Add(lightObj);
        }
    }

    void LateUpdate()
    {
        int particleCount = ps.GetParticles(particles);

        for (int i = 0; i < lightPool.Count; i++)
        {
            if (i < particleCount)
            {
                lightPool[i].SetActive(true);
                lightPool[i].transform.position = ps.main.simulationSpace == ParticleSystemSimulationSpace.Local
                    ? transform.TransformPoint(particles[i].position)
                    : particles[i].position;
            }
            else
            {
                lightPool[i].SetActive(false);
            }
        }
    }
}
