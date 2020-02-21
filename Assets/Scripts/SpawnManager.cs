using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    //Keep track of all spawn points
    public List<Spawner> spawnerPoint;

    public Spawner GetSpawner(int _index) => spawnerPoint[_index];
}
