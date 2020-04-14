using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropDownResolutionHandler : MonoBehaviour
{
    /*I should really be making like a IHandler or a Handler abstract class or something, because
     this is pretty ridiculous.
     */
    public static DropDownResolutionHandler Instance;

    [SerializeField] private TMP_Dropdown DDTMP_RESOLUTION;



    void Awake()
    {
        Instance = this;
    }

    public void UpdateResolutionList()
    {
        Resolution[] resolutions = Screen.resolutions;

        //We'll update on the avaliable resolutions...
        List<TMP_Dropdown.OptionData> resOptions = new List<TMP_Dropdown.OptionData>();
        foreach (Resolution res in resolutions)
        {
            resOptions.Add(new TMP_Dropdown.OptionData(ResToString(res)));

        }

        DDTMP_RESOLUTION.options.Clear();
        DDTMP_RESOLUTION.AddOptions(resOptions);
    }

    public string ResToString(Resolution _resolution) => "(" + _resolution.width + " x " + _resolution.height + ")";

    public void UpdateResolution()
    {
        Resolution[] resolutions = Screen.resolutions;
        GameManager.Settings.SetGameResolution(resolutions[DDTMP_RESOLUTION.value], DDTMP_RESOLUTION.value);
    }

    public TMP_Dropdown GetDDTMP_RESOLUTION() => DDTMP_RESOLUTION;
}
