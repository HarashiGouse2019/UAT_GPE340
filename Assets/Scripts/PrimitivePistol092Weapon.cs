using UnityEngine;

public class PrimitivePistol092Weapon : Weapons
{
    protected override void OnTriggerEnter(Collider collision)
    {
        if (claimed == false) OnPickUp(collision.gameObject);
    }
}
