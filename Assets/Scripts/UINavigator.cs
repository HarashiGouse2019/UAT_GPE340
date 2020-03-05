using System;
using System.Collections.Generic;
using UnityEngine;

public class UINavigator : MonoBehaviour
{
    /*UI Navigator will be responsible for the 
     movement of different UI Assets. One major example of this
     is having the Pause Menu navigate to the settings menu.*/

    [System.Serializable]
    public class UiBundle
    { //Previous Index
        public string name;
        public GameObject uiBundle;
        public List<GameObject> subGroups;
    }


    [Header("UI Assets"), SerializeField]
    private List<UiBundle> uiObjects = new List<UiBundle>();
    private static List<UiBundle>  UIObjects; 

    //The index to turn on the asset
    public static uint UIAssetIndex { get; private set; } = 0;

    //Previous Bundle
    public static UiBundle PreviousBundle { get; private set; }

    private UINavigator()
    {
        UIObjects = uiObjects;
    }

    public void NextInIndex()
    {
        /*This will simply go to the next UIAsset that's been assigned
         into the array.*/

        //Assing the current UIBundle used in game
        PreviousBundle = UIObjects[(int)UIAssetIndex];

        //Then increment the index
        UIAssetIndex++;

        //We want to enable the targeted index
        UIObjects[(int)UIAssetIndex].uiBundle.SetActive(true);

        //And then we want to disable the previous index
        PreviousBundle.uiBundle.SetActive(false);
    }

    public void PreviousInIndex()
    {
        /*This will get the previous index in the list.
         Not the previous index */

        //Assing the current UIBundle used in game
        PreviousBundle = UIObjects[(int)UIAssetIndex];

        //Then decrement the index
        UIAssetIndex--;

        //We want to enable the targeted index
        UIObjects[(int)UIAssetIndex].uiBundle.SetActive(true);

        //And then we want to disable the previous index
        PreviousBundle.uiBundle.SetActive(false);
    }

    public void Goto(string _uiBundleName)
    {
        /*This will just jump to any desired position on the
         list based on the name.*/

        //First, we'll need to iterate through our list.
        foreach(UiBundle bundle in UIObjects)
        {
            //If we find the name that we are looking for
            if(bundle.name == _uiBundleName)
            {
                //Have the current equal previous
                PreviousBundle = UIObjects[(int)UIAssetIndex];

                //Get the index value of where we got our match
                UIAssetIndex = (uint)Array.IndexOf(UIObjects.ToArray(), bundle);

                //Set the target bundle to true
                bundle.uiBundle.SetActive(true);

                //And set the previous to false.
                PreviousBundle.uiBundle.SetActive(false);
            }
        }
    }

    public void Goto(uint _index)
    {
        /*This will just jump to any desired position on the list.*/

        //Have the current equal previous
        PreviousBundle = UIObjects[(int)UIAssetIndex];

        //Get the index value of where we got our match
        UIAssetIndex = _index;

        //Set the target bundle to true
        UIObjects[(int)UIAssetIndex].uiBundle.SetActive(true);

        //And set the previous to false.
        PreviousBundle.uiBundle.SetActive(false);
    }
}
