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
}
