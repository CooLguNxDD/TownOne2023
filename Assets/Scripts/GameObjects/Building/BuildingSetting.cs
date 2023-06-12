using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingSetting : MonoBehaviour, IGameObjectStatus
{

    public BuildingScriptableObject buildingScriptableObject;

    private string BuildingTag;
    public UnitsType.UnitType UnitsType;
    private string BuildingName;

    //Building status

    private float HP;
    private int level;

    private float Damage;
    private float Defence;

    private float WalkingSpeed;
    private float ChaseRange;
    private float AttackSpeed;

    private GameObject SpawnObject;

    // spawn for every {SpawnRate} seconds
    private float SpawnRate;

    // how many mobs gonna spawn
    private int NumberOfSpawn;

    public UnityEvent OnDestroy;


    // Start is called before the first frame update
    void Awake()
    {
        ResetSetting();
        Debug.Log("Spawned Building: " + BuildingName);
    }

    public void ResetSetting() {
        BuildingTag = buildingScriptableObject.BuildingTag;
        BuildingName = buildingScriptableObject.BuildingName;
        UnitsType = buildingScriptableObject.UnitsType;
        HP = buildingScriptableObject.HP;
        level = buildingScriptableObject.level;
        Damage = buildingScriptableObject.Damage;
        Defence = buildingScriptableObject.Defence;
        WalkingSpeed = buildingScriptableObject.WalkingSpeed;
        ChaseRange = buildingScriptableObject.ChaseRange;
        AttackSpeed = buildingScriptableObject.AttackSpeed;
        SpawnRate = buildingScriptableObject.SpawnRate; // I think spawn rate should
        NumberOfSpawn = buildingScriptableObject.NumberOfSpawn;
        SpawnObject = buildingScriptableObject.SpawnObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0){
            OnDestroy.Invoke();
            gameObject.SetActive(false);
        }
    }

    public float getSpawnRate(){
        return SpawnRate;
    }
        public float getNumberOfSpawn(){
        return NumberOfSpawn;
    }

    public float GetHP()
    {
        return HP;
    }

    public void SetHP(float hp)
    {
        this.HP = hp;
    }

    public UnitsType.UnitType GetUnitsType()
    {
        return UnitsType;
    }

    public string getUnitsName()
    {
        return BuildingName;
    }

    public void takenDamage(float hp)
    {
        this.HP -= hp;
    }
}
