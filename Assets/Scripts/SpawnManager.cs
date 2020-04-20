using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager:MonoBehaviour
{
    private static SpawnManager _Instance;
    public static SpawnManager Instance
    {
        get
        {
            return _Instance;
        }
    }
    //Keep track of all spawn points
    public List<SpawnerHandler> spawnerPoints;

    public SpawnerHandler GetSpawnerHandler(int _index) => spawnerPoints[_index];
    void Awake()
    {
        #region SINGLETON
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(_Instance);
        }
        else
        {
            Destroy(gameObject);
        } 
        #endregion
    }
    public static void Init()
    {
        SpawnerHandler[] activeSpawners = GameObject.FindObjectsOfType<SpawnerHandler>();
        foreach(SpawnerHandler spawner in activeSpawners)
        {
            Instance.spawnerPoints.Add(spawner);
            spawner.OnGameStart();
        }
    }
}