using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Settings Class
    [System.Serializable]
    public struct Settings
    {
        public static float SFXVolume { get; private set; }
        public static float BGMVolume { get; private set; }
        public static float MasterVolume { get; private set; }
        public enum Quality
        {
            VERY_LOW,
            LOW,
            MEDIUM,
            HIGH,
            VERY_HIGH,
            ULTRA
        }

        public enum FrameRate 
        {
            FRAMERATE_30,
            FRAMERATE_60,
            FRAMERATE_120,
            UNLIMITED
        }

        public static Quality GameQuality { get; private set; }
        public static ShadowQuality ShadowQuality { get; private set; }
        public static Resolution Resolution { get; private set; }
        public static int ResValue { get; private set; }            //This is especially used to update the DropDown for the resolution
        public static FrameRate TargetFrameRate { get; private set; }
        public static Screen ScreenDisplay { get; private set; }
        public static bool WindowMode { get; private set; }

        //These Set Methods can be accessed with UI objects using events
        #region Set Methods
        public static void SetSFXVolume(float _value, Slider _sfxVolume)
        {
            SFXVolume = _value;
            _sfxVolume.value = SFXVolume;
        }

        public static void SetBGMVolume(float _value, Slider _bgmVolume)
        {
            BGMVolume = _value;
            _bgmVolume.value = BGMVolume;
        }

        public static void SetMasterVolume(float _value, Slider _masterVolume)
        {
            MasterVolume = _value;
            _masterVolume.value = MasterVolume;
        }

        public static void SetGameQuality(Quality _quality)
        {
            GameQuality = _quality;
        }

        public static void SetShadowQuality(ShadowQuality _quality)
        {
            ShadowQuality = _quality;
        }

        public static void SetGameResolution(Resolution _resolution, int _value)
        {
            Resolution = _resolution;
            ResValue = _value;
            Screen.SetResolution(Resolution.width, Resolution.height, Screen.fullScreenMode);
        }

        public static void SetTargetFrameRate(FrameRate _frameRate)
        {
            TargetFrameRate = _frameRate;
            switch (_frameRate)
            {
                case FrameRate.FRAMERATE_30: Application.targetFrameRate = 30;
                    break;
                case FrameRate.FRAMERATE_60: Application.targetFrameRate = 60;
                    break;
                case FrameRate.FRAMERATE_120: Application.targetFrameRate = 120;
                    break;
                case FrameRate.UNLIMITED: Application.targetFrameRate = 300;
                    break;
            }
        }

        public static void SetWindowMode(bool _isON)
        {
            WindowMode = _isON;

            if (WindowMode) Screen.fullScreenMode = FullScreenMode.Windowed;
            else Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }

        public static void ApplyChanges()
        {

        }

        #endregion

        public static void Save()
        {
            PlayerPrefs.SetFloat("SFX Volume", SFXVolume);
            PlayerPrefs.SetFloat("BGM Volume", BGMVolume);
            PlayerPrefs.SetFloat("Master Volume", MasterVolume);

            PlayerPrefs.SetInt("Game Quality", (int)GameQuality);
            PlayerPrefs.SetInt("Shadow Quality", (int)ShadowQuality);
            PlayerPrefs.SetInt("Resolution Dropdown Value", ResValue);

            PlayerPrefs.SetString("Resolution", Resolution.ToString());

            PlayerPrefs.SetInt("Window Mode", WindowMode ? 1 : 0);

            PlayerPrefs.Save();
        }

        public static void Load()
        { 
            Instance.sfxVolumeControl.value = PlayerPrefs.GetFloat("SFX Volume");
            Instance.bgmVolumeControl.value = PlayerPrefs.GetFloat("BGM Volume");
            Instance.masterVolumeControl.value = PlayerPrefs.GetFloat("Master Volume");

            /*Resolution is already saved, as well as the set window move, but the UI will not update so easily... sooo */
            Instance.dropDownResolutionHandler.GetComponent<DropDownResolutionHandler>().UpdateResolutionList();
            Instance.dropDownResolutionHandler.value = PlayerPrefs.GetInt("Resolution Dropdown Value");

            Instance.windowModeToggleHandler.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Window Mode"));
        }
    }
    #endregion

    public static GameManager Instance { get; private set; }

    //The player's current lives
    [Header("Player Lives"), SerializeField] private int playerLives;
    public static int PlayerCurrentLives { get; private set; }

    //The text that displays the player's lives
    [Header("Player Lives TMP"), SerializeField]
    private TextMeshProUGUI TMP_LIVES;

    //GUI that for setting
    [Header("UI Settings")]
    public TMP_Dropdown dropDownResolutionHandler;
    public Toggle windowModeToggleHandler;
    public Slider sfxVolumeControl;
    public Slider bgmVolumeControl;
    public Slider masterVolumeControl;
    
    //Check if the game is paused
    public static bool IsGamePaused { get; private set; }

    private delegate void Main();

    private Main core;

    void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        PlayerCurrentLives += playerLives;

        //Update lives from start; the amount of lives that we start with
        UpdateTMPLIVES(PlayerCurrentLives);

        core += Run;
        core.Invoke();

        //Update on play
        LoadPreviousChanges();
    }

    void Run()
    {
        StartCoroutine(RunGameManagerControls());
    }


    /// <summary>
    /// Runs the Game Controls like the key for pausing
    /// </summary>
    /// <returns></returns>
    IEnumerator RunGameManagerControls()
    {
        while (true)
        {

            //If the key for pause is pressed, toggle on if the game is pause or not;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!IsGamePaused)
                    PauseGame();
                else
                    UnPauseGame();
            }

            yield return null;
        }
    }

    /// <summary>
    /// To update TextMeshPro that draws our lives
    /// </summary>
    /// <param name="_value"></param>
    void UpdateTMPLIVES(int _value)
    {
        //Since text is a string, take our integar, and turn it to a string
        TMP_LIVES.text = _value.ToString();
    }

    /// <summary>
    /// Decreases the player's lives.
    /// </summary>
    public void DecrementPlayerLives()
    {
        PlayerCurrentLives--;

        //We want to update our GUI
        UpdateTMPLIVES(PlayerCurrentLives);
    }

    /// <summary>
    /// Returns the player's current live.
    /// </summary>
    /// <returns></returns>
    public static int GetPlayerLives() => PlayerCurrentLives;

    public void PlayerRespawn()
    {
       
    }

    /// <summary>
    /// Invoke to pause the game; Main purpose was to use for Unity Events
    /// </summary>
    public void InvokePauseGame()
    {
        PauseGame();
    }

    /// <summary>
    /// INvoke to unpause the game; Main purpose was to use for Unity Events
    /// </summary>
    public void InvokeUnPauseGame()
    {
        UnPauseGame();
    }

    /// <summary>
    /// Invoke the end of the application (works on a build)
    /// </summary>
    public void InvokeApplicationEnd()
    {
        ApplicationEnd();
    }

    /// <summary>
    /// Apply changes to our settings
    /// </summary>
    public void ApplyChanges()
    {
        Settings.Save();
    }


    /// <summary>
    /// Load our previously changed settings
    /// </summary>
    public void LoadPreviousChanges()
    {
        Settings.Load();
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    private static void PauseGame()
    {
        PauseMenu.Instance.SetTo(EnableState.ENABLED);
        IsGamePaused = true;
        Time.timeScale = 0;
        
    }

    /// <summary>
    /// Unpause the game
    /// </summary>
    private static void UnPauseGame()
    {
        PauseMenu.Instance.SetTo(EnableState.DISABLED);
        IsGamePaused = false;
        Time.timeScale = 1;

    }

    /// <summary>
    /// Ends the application
    /// </summary>
    public static void ApplicationEnd()
    {
        Application.Quit();
    }
}
