using System.Collections;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    /*Spawn will be able to spawn anything onto world space. They have to be object
     that are spawnable though.*/

    [Header("Spawner Name")]
    public string spawnerName;

    [Header("Object To Spawn"), SerializeField]
    public Pawn objectToSpawn;

    [Header("Able to Spawn")]
    public bool ableToSpawn = false;

    [Header("Repeat Spawning")]
    public bool repeatSpawning;
    public float repeatDuration;

    [Header("Spawn Limit")]
    public int spawnLimit = 1;

    protected int totalSpawnInstances;
    
    protected Transform position;

    protected IEnumerator spawningRoutine;

    protected float time;

    protected const uint reset = 0;

    protected void Start()
    {
        position = GetComponent<Transform>();

        spawningRoutine = SpawningRoutine();
        StartCoroutine(spawningRoutine);
    }

    public virtual int FindTotalObjectsSpawned() { return -1; }
    public virtual void SpawnObj(bool _onlyOnce) { }
    public virtual void SetSpawningInterval(float _duration) { }
    public virtual void ResetTime() { time = reset; }
    public virtual IEnumerator SpawningRoutine() { yield return null; }
}
