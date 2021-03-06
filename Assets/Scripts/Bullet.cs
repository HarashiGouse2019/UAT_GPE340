﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /*We can use this enumerate to give our bullets different functionality.
     For instance, if we made a grenade launcher, we'd give it a physical property
     of having Gravity. If you wanna get goofy, you can have your bullet bounce around.
     Other than that, the bullet will just launch straight!*/
    private Rigidbody rb;
    private Collider m_collider;
    public enum PhysicalProperty
    {
        NONE,
        GRAVITY,
        BUOYANCY,
        BOTH
    }
    public float bulletVelocity;
    public float bulletDamage;
    public PhysicalProperty physicalProperty;

    //Know who shoot it
    public Pawn shooterSource;
    public void Release(PhysicalProperty _physicalProperty, Pawn _source)
    {
        gameObject.tag = "bullet";

        shooterSource = _source;

        switch (_physicalProperty)
        {
            case PhysicalProperty.NONE:
                StartCoroutine(RUN_BULLET_PROPERTIES());
                break;
            case PhysicalProperty.GRAVITY:
                rb.useGravity = true;
                StartCoroutine(RUN_BULLET_PROPERTIES());
                break;
            case PhysicalProperty.BUOYANCY:
                m_collider.material = (PhysicMaterial)Resources.Load(Application.dataPath + @"/PhysicsMaterial/BuoyancyMat.mat");
                StartCoroutine(RUN_BULLET_PROPERTIES());
                break;
            case PhysicalProperty.BOTH:
                rb.useGravity = true;
                StartCoroutine(RUN_BULLET_PROPERTIES());
                break;
            default:
                break;
        }
    }

    IEnumerator RUN_BULLET_PROPERTIES()
    {
        rb = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        while(true)
        {
            rb.velocity = transform.right * bulletVelocity;
            yield return new WaitForEndOfFrame();
        }
    }
}
