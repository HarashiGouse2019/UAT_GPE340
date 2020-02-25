﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler Instance;

    public bool spawnInParent = false;

    [System.Serializable]
    public class ObjectPoolItem
    {
        public string name;
        public int size;
        public GameObject prefab;
        public bool expandPool;
    }

    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;

    public int poolIndex;

    void Awake()
    {
        Instance = this;
        InitObjectPooler(spawnInParent);
    }

    void InitObjectPooler(bool _spawnInParent = false)
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.size; i++)
            {
                GameObject newMember;
                if (_spawnInParent)
                    newMember = Instantiate(item.prefab, gameObject.transform);
                else
                    newMember = Instantiate(item.prefab);

                newMember.SetActive(false);
                item.prefab.name = item.name;
                pooledObjects.Add(newMember);
            }

        }
    }

    public GameObject GetMember(string name)
    {
        #region Iteration
        for (int i = 0; i < pooledObjects.Count; i++)
        {

            if (!pooledObjects[i].activeInHierarchy && (name + "(Clone)") == pooledObjects[i].name)
            {
                poolIndex = i;
                return pooledObjects[i];
            }
        }
        #endregion

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (name == item.prefab.name)
            {
                if (item.expandPool)
                {
                    GameObject newMember = Instantiate(item.prefab);
                    newMember.SetActive(false);
                    pooledObjects.Add(newMember);
                    poolIndex = pooledObjects.Count - 1;
                    return newMember;
                }
            }
        }
        Debug.LogWarning("We couldn't find a prefab of this name");
        return null;
    }
}
