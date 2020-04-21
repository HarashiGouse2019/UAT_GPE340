using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerHandler : MonoBehaviour
{
    public static SpawnerHandler Instance;
    public enum Tag
    {
        PLAYER,
        ENEMY,
        ITEM,
        WEAPON
    }

    [SerializeField]
    private Spawner spawner;

    [SerializeField]
    private Tag tagAs;

    private IEnumerator spawningRoutine;

    public float time;

    private const uint reset = 0;

    public bool breakOut = false;

    void Awake()
    {
        Instance = this;
    }

    public void OnGameStart()
    {
        spawningRoutine = SpawningRoutine();
        breakOut = false;
        StartCoroutine(spawningRoutine);
    }

    public int FindTotalObjectsSpawned() => GetComponentsInChildren<Pawn>().Length;
    public void SpawnObj(bool _onlyOnce = true)
    {
        if (spawner.ableToSpawn && (spawner.objectToSpawn is ISpawnable))
        {
            if (tagAs == Tag.PLAYER)
                GameManager.EnableUI();

            Pawn newSpawnee = Instantiate(spawner.objectToSpawn, transform);

            newSpawnee.OnSpawn();

            if (_onlyOnce) spawner.ableToSpawn = false;
        }
    }
    public void SetSpawningInterval(float _duration)
    {
        if (time < _duration && FindTotalObjectsSpawned() < spawner.spawnLimit)
            time += Time.deltaTime;

        else if (time > _duration && FindTotalObjectsSpawned() < spawner.spawnLimit)
        {
            SpawnObj(false);
            ResetTime();
        }

        else if (FindTotalObjectsSpawned() == spawner.spawnLimit)
            ResetTime();
    }
    public void ResetTime() { time = reset; }
    public IEnumerator SpawningRoutine()
    {
        while (breakOut == false)
        {
            if (spawner.repeatSpawning)
                SetSpawningInterval(spawner.repeatDuration);
            else
                SpawnObj();

            yield return new WaitForEndOfFrame();
        }
    }

    public void End()
    {
        Debug.Log("Spawning has ended...");
        breakOut = true;
        GameManager.EndGame();
        GameManager.SetResultsValue(BaseFlagCase.FlagsLeft);
    }
}
