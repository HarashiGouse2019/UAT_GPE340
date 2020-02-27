using System.Collections;
using UnityEngine;

public abstract class PickUps : MonoBehaviour, IPickable
{
    //All this are for pickup rotating for style
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
        //We rotate based on the rate
        gameObject.transform.Rotate(new Vector3(0, rotationRate, 0f));

        //This prevent an overflow of the float value in our rotation y
        if (rotationVal >= full360)
            ResetRotation();
    }

    public virtual void OnPickUp(GameObject _source)
    {
        if(_source != null)
            _source.GetComponent<Pawn>().itemDrop.AddItem(gameObject);
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
