﻿using UnityEngine;

public class PrimitivePistol092Weapon : Weapons
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnShoot()
    {
        base.OnShoot();
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
        if (claimed == false) OnPickUp(collision.gameObject);
    }
}
