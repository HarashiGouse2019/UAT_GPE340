using UnityEngine;
using UnityEngine.UI;

public class QualitySliderHandler : MonoBehaviour
{
    public static QualitySliderHandler Instance;

    public Slider qualitySlider;

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
    }
}