using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

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
        public static bool PostProcessingEnabled { get; private set; }
        public static bool ColorGradingEnabled { get; private set; }
        public static bool MotionBlurEnabled { get; private set; }
        public static bool AutoExposureEnabled { get; private set; }
        public static bool DepthOfFieldEnabled { get; private set; }

        //These Set Methods can be accessed with UI objects using events
        #region Set Methods
        /// <summary>
        /// Set the volume for only Sound Effects
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_sfxVolume"></param>
        public static void SetSFXVolume(float _value, Slider _sfxVolume)
        {
            SFXVolume = _value;
            _sfxVolume.value = SFXVolume;
        }

        /// <summary>
        /// Set the volume for only background music
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_bgmVolume"></param>
        public static void SetBGMVolume(float _value, Slider _bgmVolume)
        {
            BGMVolume = _value;
            _bgmVolume.value = BGMVolume;
        }

        /// <summary>
        /// Set the overall volume of all sounds and music
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_masterVolume"></param>
        public static void SetMasterVolume(float _value, Slider _masterVolume)
        {
            MasterVolume = _value;
            _masterVolume.value = MasterVolume;
        }

        /// <summary>
        /// Set overall quality of the game
        /// </summary>
        /// <param name="_quality"></param>
        public static void SetGameQuality(Quality _quality)
        {
            GameQuality = _quality;
            QualitySettings.SetQualityLevel((int)GameQuality);
        }

        /// <summary>
        /// Set the Shadow Quality of the game
        /// </summary>
        /// <param name="_quality"></param>
        public static void SetShadowQuality(ShadowQuality _quality)
        {
            ShadowQuality = _quality;
        }

        /// <summary>
        /// Set application's resolution
        /// </summary>
        /// <param name="_resolution"></param>
        /// <param name="_value"></param>
        public static void SetGameResolution(Resolution _resolution, int _value)
        {
            Resolution = _resolution;
            ResValue = _value;
            Screen.SetResolution(Resolution.width, Resolution.height, Screen.fullScreenMode);
        }

        /// <summary>
        /// Set the application's target framerate
        /// </summary>
        /// <param name="_frameRate"></param>
        public static void SetTargetFrameRate(FrameRate _frameRate)
        {
            TargetFrameRate = _frameRate;
            switch (_frameRate)
            {
                case FrameRate.FRAMERATE_30:
                    Application.targetFrameRate = 30;
                    break;
                case FrameRate.FRAMERATE_60:
                    Application.targetFrameRate = 60;
                    break;
                case FrameRate.FRAMERATE_120:
                    Application.targetFrameRate = 120;
                    break;
                case FrameRate.UNLIMITED:
                    Application.targetFrameRate = 300;
                    break;
            }
        }

        /// <summary>
        /// Set Window Mode on or off
        /// </summary>
        /// <param name="_isON"></param>
        public static void SetWindowMode(bool _isON)
        {
            WindowMode = _isON;

            if (WindowMode) Screen.fullScreenMode = FullScreenMode.Windowed;
            else Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }

        /// <summary>
        /// Set enablement of Post-Processing
        /// </summary>
        /// <param name="_enabled"></param>
        public static void SetPostProcessing(bool _enabled)
        {
            PostProcessingEnabled = _enabled;

        }

        /// <summary>
        /// Set enablement of Color Grading
        /// </summary>
        /// <param name="_enabled"></param>
        public static void SetColorGrading(bool _enabled)
        {
            ColorGradingEnabled = _enabled;
        }

        /// <summary>
        /// Set enablement of Motion Blur
        /// </summary>
        /// <param name="_enabled"></param>
        public static void SetMotionBlur(bool _enabled)
        {
            MotionBlurEnabled = _enabled;
        }

        /// <summary>
        /// Set enablement of Auto Exposure
        /// </summary>
        /// <param name="_enabled"></param>
        public static void SetAutoExposure(bool _enabled)
        {
            AutoExposureEnabled = _enabled;
        }

        /// <summary>
        /// Set enablement of Depth of Field
        /// </summary>
        /// <param name="_enabled"></param>
        public static void SetDepthOfField(bool _enabled)
        {
            DepthOfFieldEnabled = _enabled;
        }
        #endregion

        /// <summary>
        /// Save desire setting into registry
        /// </summary>
        public static void Save()
        {
            //Save all volume values
            PlayerPrefs.SetFloat("SFX Volume", SFXVolume);
            PlayerPrefs.SetFloat("BGM Volume", BGMVolume);
            PlayerPrefs.SetFloat("Master Volume", MasterVolume);

            //Save quality and resolution values
            PlayerPrefs.SetInt("Game Quality", (int)GameQuality);
            PlayerPrefs.SetInt("Shadow Quality", (int)ShadowQuality);
            PlayerPrefs.SetInt("Resolution Dropdown Value", ResValue);

            //Save current resolution that's set
            PlayerPrefs.SetString("Resolution", Resolution.ToString());

            //Save Window mode
            PlayerPrefs.SetInt("Window Mode", WindowMode ? 1 : 0);

            //Save all post-processing options
            PlayerPrefs.SetInt("Post Processing Enabled", PostProcessingEnabled ? 1 : 0);
            PlayerPrefs.SetInt("Color Grading Enabled", ColorGradingEnabled ? 1 : 0);
            PlayerPrefs.SetInt("Motion Blur Enabled", MotionBlurEnabled ? 1 : 0);
            PlayerPrefs.SetInt("Auto Exposure Enabled", AutoExposureEnabled ? 1 : 0);
            PlayerPrefs.SetInt("Depth Of Field Enabled", DepthOfFieldEnabled ? 1 : 0);

            //Save all keys
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Load key values from registry, and assign them to respective objects
        /// </summary>
        public static void Load()
        {
            //Update all volume values
            Instance.sfxVolumeControl.value = PlayerPrefs.GetFloat("SFX Volume");
            Instance.bgmVolumeControl.value = PlayerPrefs.GetFloat("BGM Volume");
            Instance.masterVolumeControl.value = PlayerPrefs.GetFloat("Master Volume");
            Instance.qualityControlHandler.value = PlayerPrefs.GetInt("Game Quality");

            /*Resolution is already saved, as well as the set window move, but the UI will not update so easily... sooo */
            Instance.dropDownResolutionHandler.GetComponent<DropDownResolutionHandler>().UpdateResolutionList();
            Instance.dropDownResolutionHandler.value = PlayerPrefs.GetInt("Resolution Dropdown Value");

            //Update WindowMode
            Instance.windowModeToggleHandler.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Window Mode"));

            //Update enablement of Post-Processing
            Instance.postProcessingToggleHandler.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Post Processing Enabled"));

            //Update post processing effects from registry
            Instance.postProcessingGroup.configurationToggle[0].isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Color Grading Enabled"));
            Instance.postProcessingGroup.configurationToggle[1].isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Motion Blur Enabled"));
            Instance.postProcessingGroup.configurationToggle[2].isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Auto Exposure Enabled"));
            Instance.postProcessingGroup.configurationToggle[3].isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Depth Of Field Enabled"));
        }
    }
    #endregion

    [SerializeField]
    private SpawnerHandler PlayerSpawnHandler;
    public static GameManager Instance { get; private set; }

    //The player's current lives
    [Header("Player Lives"), SerializeField] private int playerLives;
    public static int PlayerCurrentLives { get; private set; }

    //The text that displays the player's lives
    [Header("Player Lives TMP"), SerializeField, Tooltip("The text that displays the number of lives the player has.")]
    private TextMeshProUGUI TMP_LIVES;

    //GUI that for setting
    [Header("UI Settings"), Tooltip("Handlers that helps alter settings.")]
    public TMP_Dropdown dropDownResolutionHandler;
    public Toggle windowModeToggleHandler;
    public Toggle postProcessingToggleHandler;
    public Slider qualityControlHandler;
    public Slider sfxVolumeControl;
    public Slider bgmVolumeControl;
    public Slider masterVolumeControl;

    [Header("Post Processing Effects"), Tooltip("The Post Processing Volume that's causing the effects, which is altered by the PostProcessingGroup")]
    public PostProcessVolume postEffect;
    public PostProcessingGroup postProcessingGroup;

    [Header("Player UI"), SerializeField, Tooltip("The GameObject that holds all Player Status (HP, STAMINA, AMMO, ETC)")]
    private GameObject GroupStats;

    [Header("Information Log"), SerializeField]
    private TextMeshProUGUI TMP_INFOMATIONLOG;

    [Header("Results Text"), SerializeField]
    private TextMeshProUGUI TMP_RESULTS, TMP_NOTES;

    //Is Player UI Enabled
    public static bool UIEnabled { get; private set; }

    //Check if the game is paused
    public static bool IsGamePaused { get; private set; }

    //Check if game has started
    public static bool IsGameInitialized { get; private set; }

    //String null
    private const string STRINGNULL = "";

    //Time value
    private float time = 0;

    //If delaying clear
    private bool clearDelay = false;

    //The PlayerPawn
    private PlayerPawn player;

    //Main Script Cycle
    private delegate void Main();

    private Main core;

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

    public static void LogPlayer(PlayerPawn _player)
    {
        Instance.player = _player;
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

    /// <summary>
    /// Return post processing volume to allow modifications
    /// </summary>
    public static PostProcessVolume GetPostEffect() => Instance.postEffect;

    public void StartGame()
    {
        if (PlayerCurrentLives == 0)
        {
            PlayerCurrentLives += playerLives;
            player.GetDamageableObj().Heal(100f);

            //Update lives from start; the amount of lives that we start with
            UpdateTMPLIVES(PlayerCurrentLives);

        }

        IsGameInitialized = true;
        SpawnManager.Init();
    }

    public static void EnableUI()
    {
        Instance.GroupStats.SetActive(true);
    }

    public static void PrintInfoLog(string _info)
    {
        Instance.TMP_INFOMATIONLOG.text = _info;
        Instance.ClearLog();
    }

    void ClearLog(float _duration = 5f)
    {
        StartCoroutine(ClearLogCycle(_duration));
    }

    IEnumerator ClearLogCycle(float _duration)
    {
        while (true)
        {
            time += Time.deltaTime;

            if (time >= _duration)
            {
                TMP_INFOMATIONLOG.text = STRINGNULL;
                Debug.Log("DAMMMMMMMMN!!!!");
                time = 0;
                StopCoroutine(ClearLogCycle(_duration));
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public static void EndGame()
    {
        IsGameInitialized = false;
        Instance.GroupStats.SetActive(false);
        SpawnManager.SetAllSpawnersOff(SpawnManager.Instance.spawnerPoints);
    }

    public static void SetResultsValue(int _value)
    {
        switch (_value)
        {
            case 0:
                Instance.TMP_RESULTS.text = "CONGRATULATIONS";
                Instance.TMP_NOTES.text = "It looks like that you managed to get all of the flags with no issues.";
                return;
            case 1:
                Instance.TMP_RESULTS.text = "Wow! You almost had it!";
                Instance.TMP_NOTES.text = "One step closer to glory!!!";
                return;
            case 2:
                Instance.TMP_RESULTS.text = "Oh! Not quite!";
                Instance.TMP_NOTES.text = "Better luck next time... I guess?";
                return;
            case 3:
                Instance.TMP_RESULTS.text = "Huh...";
                Instance.TMP_NOTES.text = "Seems like you're having a rough day.";
                return;
            case 4:
                Instance.TMP_RESULTS.text = "That... was Pathetic";
                Instance.TMP_NOTES.text = "No comment...";
                return;
            case 5:
                Instance.TMP_RESULTS.text = "...";
                Instance.TMP_NOTES.text = "I can't even look at you right now. Such a sad existence.";
                return;
        }

    }

    public static void UpdatePlayerSpawnerPosition(Vector3 _location)
    {
        Instance.PlayerSpawnHandler.gameObject.transform.position = _location;
    }
}
