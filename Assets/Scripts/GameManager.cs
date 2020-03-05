using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Settings Class
    [System.Serializable]
    public static class Settings
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
        public static Shadow Shadows { get; private set; }
        public static Resolution[] Resolution { get; private set; } = Screen.resolutions;
        public static FrameRate TargetFrameRate { get; private set; }
        public static Screen ScreenDisplay { get; private set; }


        #region Set Methods
        public static void SetSFXVolume(float _value)
        {

        }

        public static void SetBGMVolume(float _value)
        {

        }

        public static void SetMasterVolume(float _value)
        {

        }

        public static void SetGameQuality(Quality _quality)
        {

        }

        public static void SetShadowQuality(ShadowQuality _quality)
        {

        }

        public static void SetGameResolution(Resolution _resolution)
        {

        }

        public static void SetTargetFrameRate(FrameRate _frameRate)
        {

        }
        #endregion
    }
    #endregion

    public static GameManager Instance;

    public enum ChangeSettingsFor
    {
        SFX_VOLUME,
        BGM_VOLUME,
        MASTER_VOLUME,
        GAME_QUALITY,
        SHADOW_QUALITY,
        GAME_RESOLUTION,
        TARGET_REFRESH_RATE
    }

    [Header("Player Lives"), SerializeField] private int playerLives;
    public static int PlayerCurrentLives { get; private set; }

    [Header("Player Lives TMP"), SerializeField]
    private TMPro.TextMeshProUGUI TMP_LIVES;

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
        UpdateTMPLIVES(PlayerCurrentLives);

        core += Run;
        core.Invoke();
    }

    void Run()
    {
        StartCoroutine(RunGameManagerControls());
    }

    IEnumerator RunGameManagerControls()
    {
        while (true)
        {
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


    void UpdateTMPLIVES(int _value)
    {
        TMP_LIVES.text = _value.ToString();
    }

    public void DecrementPlayerLives()
    {
        PlayerCurrentLives--;
        UpdateTMPLIVES(PlayerCurrentLives);
    }

    public static int GetPlayerLives() => PlayerCurrentLives;

    public void PlayerRespawn()
    {
       
    }

    public void InvokePauseGame()
    {
        PauseGame();
    }

    public void InvokeUnPauseGame()
    {
        UnPauseGame();
    }

    public static void PauseGame()
    {
        PauseMenu.Instance.SetTo(EnableState.ENABLED);
        IsGamePaused = true;
        Time.timeScale = 0;
        
    }

    public static void UnPauseGame()
    {
        PauseMenu.Instance.SetTo(EnableState.DISABLED);
        IsGamePaused = false;
        Time.timeScale = 1;
        
    }

    void OnSettingsUpdateCall<T>(ChangeSettingsFor _changeFor, object _value)
    {
        switch (_changeFor)
        {
            case ChangeSettingsFor.SFX_VOLUME:
                if (_value.GetType().IsAssignableFrom(typeof(float)))
                {
                    float convertedValue = (float)_value;
                    Settings.SetSFXVolume(convertedValue);
                }
                break;

            case ChangeSettingsFor.BGM_VOLUME:
                if (_value.GetType().IsAssignableFrom(typeof(float)))
                {
                    float convertedValue = (float)_value;
                    Settings.SetBGMVolume(convertedValue);
                }
                break;

            case ChangeSettingsFor.MASTER_VOLUME:
                if (_value.GetType().IsAssignableFrom(typeof(float)))
                {
                    float convertedValue = (float)_value;
                    Settings.SetMasterVolume(convertedValue);
                }
                break;

            case ChangeSettingsFor.GAME_QUALITY:
                if (_value.GetType().IsAssignableFrom(typeof(Settings.Quality)))
                {
                    Settings.Quality convertedValue = (Settings.Quality)_value;
                    Settings.SetGameQuality(convertedValue);
                }
                break;

            case ChangeSettingsFor.SHADOW_QUALITY:
                if (_value.GetType().IsAssignableFrom(typeof(ShadowQuality)))
                {
                    ShadowQuality convertedValue = (ShadowQuality)_value;
                    Settings.SetShadowQuality(convertedValue);
                }
                break;

            case ChangeSettingsFor.GAME_RESOLUTION:
                if (_value.GetType().IsAssignableFrom(typeof(Resolution)))
                {
                    Resolution convertedValue = (Resolution)_value;
                    Settings.SetGameResolution(convertedValue);
                }
                break;

            case ChangeSettingsFor.TARGET_REFRESH_RATE:
                if (_value.GetType().IsAssignableFrom(typeof(Settings.FrameRate)))
                {
                    Settings.FrameRate convertedValue = (Settings.FrameRate)_value;
                    Settings.SetTargetFrameRate(convertedValue);
                }
                break;
            default:
                break;
        }
    }
}
