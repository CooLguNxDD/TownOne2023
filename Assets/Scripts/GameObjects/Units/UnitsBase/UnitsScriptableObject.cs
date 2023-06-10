using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameUnits", menuName = "Units")]
public class UnitsScriptableObject : ScriptableObject
{
    //UID
    public int UnitsUID;

    //Unit discriptions
    //Unit tag should match the tag of the objectPool
    public string UnitsTag;

    public UnitsType.UnitType UnitsType;
    public UnitsType.AttackType AttackType;
    public string UnitsName;

    //Basic Unit status

    public int level;
    public int HP;

    public float Damage;
    public float Defence;

    public float WalkingSpeed;
    public float ChaseRange;
    public float AttackSpeed;

    public float attackRange;

}