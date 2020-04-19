using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QualitySliderHandler : MonoBehaviour
{
    public static QualitySliderHandler Instance;

    public Slider qualitySlider;
    public TextMeshProUGUI TMP_QUALITY;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateQualityLevel()
    {
        //We'll get the index value of the enumerator that we have set just to set the quality
        GameManager.Settings.SetGameQuality((GameManager.Settings.Quality)qualitySlider.value);

        string qualityLevelString = Enum.GetName(typeof(GameManager.Settings.Quality), GameManager.Settings.GameQuality).Replace("_", " ");

        TMP_QUALITY.text = "QUALITY LEVEL (" + qualityLevelString + ")";
    }
}