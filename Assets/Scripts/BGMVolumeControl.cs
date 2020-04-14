using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMVolumeControl : MonoBehaviour
{
    private static BGMVolumeControl Instance;

    [SerializeField] private int controlValue = 0;
    [SerializeField] private Slider S_BGMVOLUME;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateBGMValue()
    {
        AudioManager.Instance.GetBGMAudioMixer().audioMixer.SetFloat("BGMVolumeParam", S_BGMVOLUME.value);
        GameManager.Settings.SetBGMVolume(S_BGMVOLUME.value, S_BGMVOLUME);
    }

    public Slider GetS_BGMVOLUME() => S_BGMVOLUME;
}
