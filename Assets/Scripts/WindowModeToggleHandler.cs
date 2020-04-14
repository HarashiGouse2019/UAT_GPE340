using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowModeToggleHandler : MonoBehaviour
{
    private static WindowModeToggleHandler Instance;
    [SerializeField]private Toggle T_WINDOWMODE;

    void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        T_WINDOWMODE.isOn = GameManager.Settings.WindowMode;
    }

    public void UpdateWindowMode()
    {
        GameManager.Settings.SetWindowMode(T_WINDOWMODE.isOn);
    }

    public  Toggle GetT_WINDOWMODE() => T_WINDOWMODE;
}
