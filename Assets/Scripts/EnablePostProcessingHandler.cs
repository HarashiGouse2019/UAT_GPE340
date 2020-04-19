using System;
using UnityEngine;
using UnityEngine.UI;

public class EnablePostProcessingHandler : MonoBehaviour
{
    public static EnablePostProcessingHandler Instance;

    [SerializeField]
    private Toggle postProcessingToggle;

    //Post Effect game object that handles the special effects
    [SerializeField]
    private GameObject postEffectVolumeObj;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdatePostProcessingToggle()
    {
        
        GameManager.Settings.SetPostProcessing(postProcessingToggle.isOn);
        postEffectVolumeObj.SetActive(GameManager.Settings.PostProcessingEnabled);
    }

    public void UpdatePostProcessingToggle(bool _isOn)
    {
        postProcessingToggle.isOn = _isOn;
        GameManager.Settings.SetPostProcessing(postProcessingToggle.isOn);
        postEffectVolumeObj.SetActive(GameManager.Settings.PostProcessingEnabled);
    }

    public static bool IsEnabled() => Instance.postProcessingToggle.isOn;
    public static bool NotNull() => (Instance != null);
}
