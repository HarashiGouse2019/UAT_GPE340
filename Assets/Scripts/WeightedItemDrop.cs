using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeightedItemDrop : MonoBehaviour
{
    public List<WeightedObject> itemsToDrop = new List<WeightedObject>();
    int randomNum;
    List<int> CDFArray = new List<int>();

   [System.Serializable]
    public class WeightedObject
    {
        public PickUps pickUp;
        public int weight;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop()
    {
        
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


        //Remove if BinarySearch works!!!
        ////Wealk through CDF array to find where our random number would fall on "our" array
        //for(int index = 0; index < CDFArray.Count; index++)
        //{
        //    if(randomNum <= CDFArray[index])
        //    {
        //        //Return the same location from itemToDrop
        //        return itemsToDrop[index].pickUp;
        //    }
        //}



        //Binary Search
        int selectedIndex = Array.BinarySearch(CDFArray.ToArray(), randomNum);

        if (selectedIndex < 0)
            selectedIndex =  ~selectedIndex;

        return itemsToDrop[selectedIndex].pickUp;
    }
}
