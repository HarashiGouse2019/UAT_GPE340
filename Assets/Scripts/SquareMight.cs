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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnShoot()
    {
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = ammoSource.position;
        bullet.transform.rotation = ammoSource.rotation;
        bullet.Release(bullet.physicalProperty);
    }
}
