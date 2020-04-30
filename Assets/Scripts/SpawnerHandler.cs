using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerHandler : MonoBehaviour
{
    public static SpawnerHandler Instance;

    //Tag enumerator
    public enum Tag
    {
        PLAYER,
        ENEMY,
        ITEM,
        WEAPON
    }

    [SerializeField, Tooltip("Our spawner scriptable object that hands what spawns, how often it spawns, and the limit.")]
    private Spawner spawner;

    [SerializeField, Tooltip("Check a tag of a given object to know what category an game entity belongs to.")]
    private Tag tagAs;

    //Spawning coroutine
    private IEnumerator spawningRoutine;

    //Time value
    public float time;

    //Reset constant
    private const uint reset = 0;

    //To break out of spawning loop
    public bool breakOut = false;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// On the start of the game
    /// </summary>
    public void OnGameStart()
    {
        //Variable representation of coroutine
        spawningRoutine = SpawningRoutine();

        //Don't break out of spawning loop.
        breakOut = false;

        //Set the spawner's intial position;
        spawner.SetInitialPosition(transform.position);
        spawner.SetSpawnLocation(spawner.initialPosition);

        //Begin spawning loop
        StartCoroutine(spawningRoutine);
    }

    /// <summary>
    /// Get the number of total spawnees.
    /// </summary>
    /// <returns></returns>
    public int FindTotalObjectsSpawned() => GetComponentsInChildren<Pawn>().Length;

    /// <summary>
    /// Spawn a spawnee at a desired coordinate
    /// </summary>
    /// <param name="_onlyOnce"></param>
    public void SpawnObj(bool _onlyOnce = true)
    {
        //If it's time to spawn an object, and the object to be spawned implements ISpawnable
        if (spawner.ableToSpawn && (spawner.objectToSpawn is ISpawnable))
        {
            //If the object is tagged as being a player, enable UI
            if (tagAs == Tag.PLAYER)
                GameManager.EnableUI();

            //Spawn a new spawnee
            Pawn newSpawnee = Instantiate(spawner.objectToSpawn, transform);
            newSpawnee.transform.position = spawner.spawnPointLocation;

            //Execute OnSpawn code
            newSpawnee.OnSpawn();

            //If the spawner can only spawn one object, stop spawning.
            if (_onlyOnce) spawner.ableToSpawn = false;
        }
    }
    /// <summary>
    /// The duration between each object being spawned.
    /// </summary>
    /// <param name="_duration"></param>
    public void SetSpawningInterval(float _duration)
    {
        //Keep track on time if not at the duration mark, and if spawnLimit hasn't been met.
        if (time < _duration && FindTotalObjectsSpawned() < spawner.spawnLimit)
            time += Time.deltaTime;

        //If time is up and still haven't met spawnLimit, we don't 
        else if (time > _duration && FindTotalObjectsSpawned() < spawner.spawnLimit)
        {
            SpawnObj(false);
            ResetTime();
        }

        else if (FindTotalObjectsSpawned() == spawner.spawnLimit)
            ResetTime();
    }

    /// <summary>
    /// Reset time duration
    /// </summary>
    public void ResetTime() { time = reset; }

    /// <summary>
    /// Returns the spawner scriptable object
    /// </summary>
    /// <returns></returns>
    public Spawner GetAssociatedSpawner() => spawner;

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
