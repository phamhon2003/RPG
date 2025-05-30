using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _MusicSlider;
    [SerializeField] private Slider _SFXSlider;
    void Start()
    {
        if (PlayerPrefs.HasKey("Musicvolume")&& PlayerPrefs.HasKey("SFXvolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = _MusicSlider.value; 
        _audioMixer.SetFloat("Music",Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("Musicvolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = _SFXSlider.value;
        _audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXvolume", volume);
    }
    private void LoadVolume()
    {
        _MusicSlider.value = PlayerPrefs.GetFloat("Musicvolume");
        _SFXSlider.value = PlayerPrefs.GetFloat("SFXvolume");
    }

}
