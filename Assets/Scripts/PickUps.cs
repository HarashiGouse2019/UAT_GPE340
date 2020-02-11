using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PickUps : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent onPickUp;

    public virtual void OnPickUp()
    {
        //Do a thing
    }
}
