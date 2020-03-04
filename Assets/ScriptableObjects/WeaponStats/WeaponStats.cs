using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stats", menuName = "Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    [Header("Weapon Damage")]
    public float weaponMinDamage;
    public float weaponMaxDamage;

    [Header("Critical Probability (in %)")]
    public float weaponCritChance;

    [Header("Weapon Critical Damage")]
    public float weaponCritMinDamage;
    public float weaponCritMaxDamage;

    [Header("Ammo Capacity")]
    public int weaponAmmoCapacity;

    [Header("Weapon Firing Rate")]
    public float weaponRoundsPer;

    [Header("Weapon Range")]
    public float weaponRange;

    [Header("Takes What Kind of Ammunition?")]
    public string ammoKind;
}
