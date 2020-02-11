using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageableObj : MonoBehaviour
{
    /*DamagebalObj will be responsible for making an object to take damage.
     Variables like the health and strength values can be set through the inspector.
     Strength will be subtracted from the health, which will be the overall damage that it'll take.
     */
    [Header("Health Value")]
    public float objHealth = 100f;

    [Header("Strength Value")]
    public float objStrength = 1;

    [Header("Events")]
    [SerializeField] private UnityEvent onHeal;
    [SerializeField] private UnityEvent onDamage;
    [SerializeField] private UnityEvent onDie;

    //We want to be able to know what object this script is attact to, and what it's affecting
    private GameObject attachedObj;

    private void Awake()
    {
        attachedObj = GetComponent<GameObject>();
    }

    public void TakeDamage(float _damageVal)
    {
        objHealth = _damageVal / objStrength;
    }

    public void Heal(float _healVal)
    {
        objHealth += _healVal;
    }

    public void Die()
    {
        //Do a thing when you die.
    }

    public float GetObjHealthVal() => objHealth;

    public float GetObjStrengthVal() => objStrength;
}
