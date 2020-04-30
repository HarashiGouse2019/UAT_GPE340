using UnityEngine;

[CreateAssetMenu(fileName = "New Spawner", menuName = "Spawner")]
public class Spawner : ScriptableObject
{
    /*Spawn will be able to spawn anything onto world space. They have to be object
     that are spawnable though.*/

    public enum PositionType
    {
        RELATIVE,
        ABSOLUTE
    }

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

    //The initial position of the spawner object in world space.
    public Vector3 initialPosition { get; private set; }

    //This is not the position of the spawner, but the position where the spawner will pawn an object relative to it's position in world space.
    public Vector3 spawnPointLocation { get; private set; }

    /// <summary>
    /// Set the position of where the object is located.
    /// </summary>
    /// <param name="_startPosition"></param>
    public void SetInitialPosition(Vector3 _startPosition)
    {
        initialPosition = _startPosition;
    }

    /// <summary>
    /// Set the location that the spawner will spawn an object either relatively or absolutely.
    /// </summary>
    /// <param name="_spawnPosition"></param>
    /// <param name="_positionType"></param>
    public void SetSpawnLocation(Vector3 _spawnPosition, PositionType _positionType = PositionType.RELATIVE)
    {
        //Spawn relative to the spawn, or spawn on direct coordinates
        //This is especially handy having a "checkpoint" mechanic when each flag is collected
        //without the player spawning through the world after collecting them.
        switch (_positionType)
        {
            case PositionType.RELATIVE:
                //We'll take the intial position plus the location we want to spawn
                spawnPointLocation = initialPosition + _spawnPosition;
                return;

            case PositionType.ABSOLUTE:
                //We'll directly pass the value of our spawn position;
                spawnPointLocation = _spawnPosition;
                return;
        }
    }
}
