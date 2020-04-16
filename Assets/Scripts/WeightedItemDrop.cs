using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeightedItemDrop : MonoBehaviour
{
    //List of items that we spawn based on the weight (or chance) of it occuring
    public List<WeightedObject> itemsToDrop = new List<WeightedObject>();

    //A random number that will be used to pick a random item from our list
    int randomNum;

    //This is an array of values called the Cummulative Defintive Function
    List<int> CDFArray = new List<int>();

    //We get the transform of our parent
    Transform parent;

    //An offset. This will be used to makes sure that the spawning item doesn't clip on the ground
    Vector3 spawnOffset = new Vector3(0f, 1f, 0f);

    //Our weighted objects
   [System.Serializable]
    public class WeightedObject
    {
        public PickUps pickUp;
        public int weight;
    }

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop()
    {
        //Spawn a pickup from our list
        Instantiate(RandomPickUpFromList(), parent.position + spawnOffset, Quaternion.identity);
    }

    PickUps RandomPickUpFromList()
    {

        //Create a new array - parallel to our weighted drops, but contains a cumulative value (CDF)
        CDFArray.Clear();

        int density = 0;
        //SO... go through my weighted drop list, and track cummulative
        foreach(WeightedObject drop in itemsToDrop)
        {
            int index = Array.IndexOf(itemsToDrop.ToArray(), drop);
            density += itemsToDrop[index].weight;
            CDFArray.Add(density);
        }

        //Choose a randome number between 0 and my maximum density
        randomNum = Random.Range(0, density);

        //Binary Search
        int selectedIndex = Array.BinarySearch(CDFArray.ToArray(), randomNum);

        if (selectedIndex < 0)
            selectedIndex =  ~selectedIndex;

        return itemsToDrop[selectedIndex].pickUp;
    }
}
