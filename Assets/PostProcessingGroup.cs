using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingGroup : MonoBehaviour
{
    public static PostProcessingGroup Instance;
    [SerializeField]
    private CanvasGroup postProcessingConfig;

    [SerializeField]
    public Toggle[] configurationToggle = new Toggle[4];


    ColorGrading colorGrading = null;
    MotionBlur motionBlur = null;
    AutoExposure autoExposure = null;
    DepthOfField depthOfField = null;

    const float ENABLE_TRANSPARENCY = 1f;
    const float DISABLE_TRANSPARENCY = 0.1f;

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

    void Start()
    {
        EnablePostProcessingOptions();
    }

    public void EnablePostProcessingOptions()
    {
        if (EnablePostProcessingHandler.NotNull())
        {
            switch (EnablePostProcessingHandler.IsEnabled())
            {
                case true: postProcessingConfig.alpha = ENABLE_TRANSPARENCY; return;
                case false: postProcessingConfig.alpha = DISABLE_TRANSPARENCY; return;
            }
        }
    }

    public void TurnOnPostProcessing()
    {

        foreach (Toggle toggle in configurationToggle)
        {
            try
            {
                if (toggle.isOn && !EnablePostProcessingHandler.IsEnabled())
                {
                    Debug.Log("Doing things");
                    Debug.Log("ConfigureToggle " + Array.IndexOf(configurationToggle, toggle));
                    EnablePostProcessingHandler.Instance.UpdatePostProcessingToggle(toggle.isOn);
                }
            }
            catch { }
        }
    }

    public void UpdateEffect()
    {
        foreach (Toggle toggle in configurationToggle)
            ModifyPostProcessingEffects(Array.IndexOf(configurationToggle, toggle), toggle.isOn);
    }

    void ModifyPostProcessingEffects(int _index, bool _enable)
    {
        switch (_index)
        {
            //Color Grading Effect
            case 0:
                GameManager.Settings.SetColorGrading(_enable);
                GameManager.GetPostEffect().profile.TryGetSettings(out colorGrading);
                colorGrading.active = _enable;
                return;

            //Motion Blur Effect
            case 1:
                GameManager.Settings.SetMotionBlur(_enable);
                GameManager.GetPostEffect().profile.TryGetSettings(out motionBlur);
                motionBlur.active = _enable;
                return;

            //Auto-Exposure Effect
            case 2:
                GameManager.Settings.SetAutoExposure(_enable);
                GameManager.GetPostEffect().profile.TryGetSettings(out autoExposure);
                autoExposure.active = _enable;
                return;

            //Depth of Field Effect
            case 3:
                GameManager.Settings.SetDepthOfField(_enable);
                GameManager.GetPostEffect().profile.TryGetSettings(out depthOfField);
                depthOfField.active = _enable;
                return;
        }
    }
}
