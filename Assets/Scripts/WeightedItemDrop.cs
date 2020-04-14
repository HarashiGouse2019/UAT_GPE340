using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedItemDrop : MonoBehaviour
{
    static System.Random rnd = new System.Random();

    static System.Array cdfArray;

    [System.Serializable]
    public class WeightedObject
    {
        [SerializeField, Tooltip("The object selected by this choice.")]
        private Object value;

        [SerializeField, Tooltip("The chance to select the value.")]
        private double chance = 1.0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int FindCumulativeDensity()
    {
        int selectedIndex = System.Array.BinarySearch(cdfArray, rnd.NextDouble() * cdfArray.Last());
        if (selectedIndex < 0)
            selectedIndex = ~selectedIndex;
        return choices[selectedIndex].value;
    }
}
