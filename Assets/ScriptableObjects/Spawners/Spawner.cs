using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawner", menuName = "Spawner")]
public class Spawner : ScriptableObject
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
}
