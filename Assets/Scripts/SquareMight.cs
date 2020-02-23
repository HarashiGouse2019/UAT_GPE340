using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMight : Weapons
{
    //We what to know what kind of bullets this thing shoots out
    public GameObject bulletPrefab;
    public Transform ammoSource;

    //How much ammo it can hold
    public int ammoAmount = 10;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            SetNextRound();
    }

    public override void OnShoot()
    {
        if (canShoot)
        {
            Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            bullet.transform.position = ammoSource.position;
            bullet.transform.rotation = ammoSource.rotation;
            bullet.Release(bullet.physicalProperty);
            canShoot = false;
        }
    }

    void SetNextRound()
    {
        canShoot = true;
    }
}
