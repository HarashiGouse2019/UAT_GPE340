using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeControl : MonoBehaviour
{
    private static SFXVolumeControl Instance;

    [SerializeField] private int controlValue = 0;
    [SerializeField] private Slider S_SFXVOLUME;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateSFXValue()
    {
        AudioManager.Instance.GetSFXAudioMixer().audioMixer.SetFloat("SFXVolumeParam", S_SFXVOLUME.value);
        GameManager.Settings.SetSFXVolume(S_SFXVOLUME.value, S_SFXVOLUME);
    }

    public  Slider GetS_SFXVOLUME() => S_SFXVOLUME;
}
