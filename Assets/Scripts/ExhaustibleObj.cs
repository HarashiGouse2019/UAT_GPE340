using System.Collections;
using UnityEngine;

public class ExhaustibleObj : MonoBehaviour
{
    [SerializeField] private float objStamina;
    [SerializeField] private float maxObjStamina = 100f;

    //Know what object this script belongs to
    private GameObject attachedObj;

    [SerializeField] private bool isRecovering = false;

    IEnumerator staminaRecovery;

    private void Awake()
    {
        attachedObj = GetComponent<GameObject>();
        objStamina = maxObjStamina;
        staminaRecovery = StaminaRecovery(0.5f, 1f);


    }

    public void UseObjStamina(float _value)
    {
        objStamina -= _value;
    }

    public float GetObjStamina() => objStamina;

    IEnumerator StaminaRecovery(float _duration, float _recoveryIncrease)
    {
        while (isRecovering)
        {
            yield return new WaitForSeconds(_duration);
            objStamina += _recoveryIncrease;

            //Check if at maxStamina. We want to break out of this while loop.
            if (objStamina > maxObjStamina)
                isRecovering = false;
        }
    }

    public IEnumerator CheckForRecovery()
    {
        while (true)
        {
            if (objStamina < maxObjStamina && isRecovering == false)
                StartCoroutine(staminaRecovery);

            yield return new WaitForEndOfFrame();
        }
    }
}
