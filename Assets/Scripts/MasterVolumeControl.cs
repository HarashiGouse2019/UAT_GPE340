using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeControl : MonoBehaviour
{
    private static MasterVolumeControl Instance;

    [SerializeField] private int controlValue = 0;
    [SerializeField] private Slider S_MASTERVOLUME;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateMasterValue()
    {
        AudioManager.Instance.MasterMixerGroup.audioMixer.SetFloat("MasterVolumeParam", S_MASTERVOLUME.value);
        GameManager.Settings.SetMasterVolume(S_MASTERVOLUME.value, S_MASTERVOLUME);
    }

    public Slider GetS_MASTERVOLUME() => S_MASTERVOLUME;
}
