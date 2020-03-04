using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Lives"), SerializeField] private int playerLives;
    public int playerCurrentLives { get; private set; }

    [Header("Player Lives TMP"), SerializeField]
    private TMPro.TextMeshProUGUI TMP_LIVES;

    public static bool IsGamePaused { get; private set; }

    public delegate void Main();

    public Main core;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

        playerCurrentLives += playerLives;
        UpdateTMPLIVES(playerCurrentLives);

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
        playerCurrentLives--;
        UpdateTMPLIVES(playerCurrentLives);
    }

    public int GetPlayerLives() => playerCurrentLives;

    public void PlayerRespawn()
    {

    }

    public static void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0;
    }

    public static void UnPauseGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1;
    }
}
