using System;
using UnityEngine;
using UnityEngine.UI;

public class EnablePostProcessingHandler : MonoBehaviour
{
    /*EnablePostProcessingHandler hands the enablement of not only Post-Processing,
     but also the effects that it has.*/

    public static EnablePostProcessingHandler Instance;

    //Enable Post Processing Toggle
    [SerializeField]
    private Toggle postProcessingToggle;

    //Post Effect game object that handles the special effects
    [SerializeField]
    private GameObject postEffectVolumeObj;

    void Awake()
    {
        #region SINGLETON
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        } 
        #endregion
    }

    /// <summary>
    /// Update Post Processing in Settings
    /// </summary>
    public void UpdatePostProcessingToggle()
    {
        //Set enablement of Post Processing and write it to Settings
        GameManager.Settings.SetPostProcessing(postProcessingToggle.isOn);

        //Enable Post-Processing
        postEffectVolumeObj.SetActive(GameManager.Settings.PostProcessingEnabled);
    }

    /// <summary>
    /// Update Post Porcessing in Settings
    /// </summary>
    /// <param name="_isOn"></param>
    public void UpdatePostProcessingToggle(bool _isOn)
    {
        postProcessingToggle.isOn = _isOn;
        GameManager.Settings.SetPostProcessing(postProcessingToggle.isOn);
        postEffectVolumeObj.SetActive(GameManager.Settings.PostProcessingEnabled);
    }

    /// <summary>
    /// Check if Post-Processing is enabled
    /// </summary>
    /// <returns></returns>
    public static bool IsEnabled() => Instance.postProcessingToggle.isOn;

    /// <summary>
    /// Check if this class is not null
    /// </summary>
    /// <returns></returns>
    public static bool NotNull() => (Instance != null);
}
