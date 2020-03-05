using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum EnableState
{
    DISABLED,
    ENABLED
}

public class PauseMenu : MonoBehaviour, IPointerEnterHandler
{
    public static PauseMenu Instance;

    //Contains all elemented in Pause Menu to enable complete control
    [Header("Parent"), SerializeField]
    private RectTransform parent;

    [Header("Panel"), SerializeField]
    private Image panel;

    [Header("Buttons"), SerializeField]
    private List<Button> buttons = new List<Button>();


    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTo(EnableState _state)
    {
        //Our flag for enabling and disabling
        bool enabled = false;

        //Validate what data's been passed
        switch (_state)
        {
            case EnableState.DISABLED: enabled = false; break;
            case EnableState.ENABLED: enabled = true; break;
            default: return;
        }

        //Activate the gameObject that associates with Pausing
        parent.gameObject.SetActive(enabled);
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {

    }
}

