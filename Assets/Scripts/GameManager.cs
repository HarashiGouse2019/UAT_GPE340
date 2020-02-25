using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Handling")]
    public Slider S_HEALTH;
    public Slider S_STAMINA;
    public TMPro.TextMeshProUGUI TMP_AMMO;

    //Main function
    public delegate void Main();
    public Main mainFunc;

    // Start is called before the first frame update
    void Start()
    {
        mainFunc += () => RUNUI();
        mainFunc.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RUNUI()
    {

    }
}
