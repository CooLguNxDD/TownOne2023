using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameUnits", menuName = "Buildings")]
public class BuildingScriptableObject : ScriptableObject
{
    public int BuildingUID;

    //Building discriptions
    //Building tag should match the tag of the objectPool
    public string BuildingTag;

    public UnitsType.UnitType UnitsType;
    public string BuildingName;

    

    //Building status

    public float HP;
    public int level;

    public float Damage;
    public float Defence;

    public float WalkingSpeed;
    public float ChaseRange;
    public float AttackSpeed;

    //SpawnOptions
    public GameObject SpawnObject;

    // spawn for every {SpawnRate} seconds
    public float SpawnRate;

    // how many mobs gonna spawn
    public int NumberOfSpawn;

}
