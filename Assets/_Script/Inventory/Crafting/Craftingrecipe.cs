using UnityEngine;

public class  lightFireFly  : MonoBehaviour
{
    public GameObject lightPrefab; // Gán Prefab FireflyLight vào ðây
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private GameObject[] lights;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
        lights = new GameObject[ps.main.maxParticles];

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i] = Instantiate(lightPrefab, transform);
            lights[i].SetActive(false);
        }
    }

    void LateUpdate()
    {
        int particleCount = ps.GetParticles(particles);

        for (int i = 0; i < lights.Length; i++)
        {
            if (i < particleCount)
            {
                lights[i].SetActive(true);
                lights[i].transform.position = particles[i].position;
            }
            else
            {
                lights[i].SetActive(false);
            }
        }
    }
}
