using System;
using UnityEngine;

public class HealthPack : PickUps
{
    public override void Update()
    {
        base.Update();
    }

    public override void OnPickUp(GameObject _source)
    {
        try
        {
            //We simply heal whoever picks this item up.
            if (_source != null) _source.GetComponent<DamageableObj>().Heal(10f);
            Destroy(gameObject);

        }
        catch { }
    }
}
