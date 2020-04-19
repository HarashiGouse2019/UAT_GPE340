using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUINaviIndex : MonoBehaviour
{
    //Udates the UIAssetIndex in the UINavigator
    [Header("Update UIAsset Index"), SerializeField] private uint index;

    void OnEnable()
    {
        UINavigator.SetAssetIndex(index);
    }
}
