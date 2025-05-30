using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Header("Audio Source")]
    [SerializeField] AudioSource _AudioSourceMusic;
    [SerializeField] AudioSource _AudioSourceSFX;
    [Header("Audio Clip")]
    public AudioClip _Music;
    public AudioClip _Cutdowntrees;
    public AudioClip _TreeFalling;
    public AudioClip _Collect;
    public AudioClip _SoundGrass;
    public AudioClip _Splash;
    public AudioClip _PullFish;
    public AudioClip _FoodStep;
    public AudioClip _Claim;
    public AudioClip _PutSeed;
    public AudioClip _Put;
    public AudioClip _Dig;
    public AudioClip _HarVestTrigger;
    public AudioClip _OpenChest;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;           
        }     
    }
    private void Start()
    {
        _AudioSourceMusic.clip = _Music;
        _AudioSourceMusic.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        _AudioSourceSFX.PlayOneShot(clip) ;
    }
    public void PlayLoopingSFX(AudioClip clip)
    {
        _AudioSourceSFX.clip = clip;
        _AudioSourceSFX.loop = true;
        _AudioSourceSFX.Play();
    }

    public void StopLoopingSFX()
    {
        _AudioSourceSFX.Stop();
        _AudioSourceSFX.loop = false;
        _AudioSourceSFX.clip = null;
    }
}
