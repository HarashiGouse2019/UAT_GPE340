using UnityEngine;

public class SquareMight : Weapons
{
    protected override void OnTriggerEnter(Collider collision)
    {
        if(claimed == false) OnPickUp(collision.gameObject);
    }
}
