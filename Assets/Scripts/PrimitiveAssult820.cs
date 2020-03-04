using UnityEngine;

public class PrimitiveAssult820 : Weapons
{
    protected override void OnTriggerEnter(Collider collision)
    {
        if (claimed == false) OnPickUp(collision.gameObject);
    }
}
