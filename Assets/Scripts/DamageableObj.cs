using UnityEngine;
using UnityEngine.Events;

public class DamageableObj : MonoBehaviour
{
    /*DamagebalObj will be responsible for making an object to take damage.
     Variables like the health and strength values can be set through the inspector.
     Strength will be subtracted from the health, which will be the overall damage that it'll take.
     */
    [Header("Health Value")]
    [SerializeField] private float objHealth = 100f;

    [Header("Strength Value")]
    [SerializeField] private float objStrength = 1;

    [Header("Events")]
    [SerializeField] private UnityEvent onDie;
    [SerializeField] private UnityEvent onDestroy;

    float time;
    const float timeUntilDestroy = 5f;

    //We want to be able to know what object this script is attact to, and what it's affecting
    public GameObject attachedObj;

    private void Awake()
    {
        attachedObj = gameObject;
    }

    private void Update()
    {
        if (objHealth < 0)
        {
            onDie.Invoke();
            WaitForDestroy();
        }
    }

    public void TakeDamage(float _damageVal)
    {
        //When we take damage, we also what to consider the armor
        //And divide the damage value from that.
        objHealth -= _damageVal / objStrength;
    }

    public void Heal(float _healVal)
    {
        //Just heal the pawn
        objHealth += _healVal;
    }

    public void Die()
    {
        //Do a thing when you die.
    }

    //Get the health val
    public float GetObjHealthVal() => objHealth;

    //get the strength val
    public float GetObjStrengthVal() => objStrength;


    /*This will be to destroy an instance of an object
     or do something as objHealth is zero*/
    public void WaitForDestroy()
    {
        time += Time.deltaTime;
        if (time >= timeUntilDestroy)
            onDestroy.Invoke();
    }
}
