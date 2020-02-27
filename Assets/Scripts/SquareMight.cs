using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMight : Weapons
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Shoot()
    {
        base.Shoot();
    }

    public override void OnPickUp(GameObject _source)
    {
        base.OnPickUp(_source);
    }

    public override void SetNextRound()
    {
        base.SetNextRound();
    }

    public override bool OutOfAmmo() => base.OutOfAmmo();

    protected override void OnTriggerEnter(Collider collision)
    {
        if(claimed == false) OnPickUp(collision.gameObject);
    }
}
