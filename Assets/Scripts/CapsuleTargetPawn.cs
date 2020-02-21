using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CapsuleTargetPawn : Pawn
{
    public override void OnSpawn()
    {
        GetComponent<CapsuleTargetController>().target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public GameObject FindGameObjectOnLayer(string _layerName)
    {
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject obj in allGameObjects)
        {
            if (obj.layer == SortingLayer.GetLayerValueFromName(_layerName))
                return obj;
        }
        return null;
    }
}
