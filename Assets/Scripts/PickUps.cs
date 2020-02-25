using System.Collections;
using UnityEngine;

public abstract class PickUps : MonoBehaviour, IPickable
{
    protected uint reset = 0;
    protected uint full360 = 360;
    public float rotationVal = 0f;
    protected float rotationRate = 1f;

    public virtual void Update()
    {
        Rotate();
    }

    public virtual void Rotate()
    {
        gameObject.transform.Rotate(new Vector3(0, rotationRate, 0f));

        if (rotationVal >= full360)
            ResetRotation();
    }

    public virtual void OnPickUp(GameObject _source)
    {
        //Do a thing
    }

    public virtual void ResetRotation()
    {
        rotationVal = reset;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        OnPickUp(other.gameObject);
    }
}
