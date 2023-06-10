using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsSetting : MonoBehaviour, IGameObjectStatus
{

    public UnitsScriptableObject UnitsScriptableObject;

    //UnitsScriptableObject -> Unit Setting
    private string UnitsTag;

    private UnitsType.UnitType UnitsType;
    private UnitsType.AttackType AttackType;
    private string UnitsName;

    private int level;
    private float HP;

    private float Damage;
    private float Defence;

    private float WalkingSpeed;
    private float ChaseRange;
    private float AttackSpeed;

    private float attackRange;

    void Awake()
    {
        ResetSetting();
        Debug.Log("Spawned Unit " + UnitsName);
    }
    public void ResetSetting()
    {
        UnitsTag = UnitsScriptableObject.UnitsTag;
        UnitsType = UnitsScriptableObject.UnitsType;
        AttackType = UnitsScriptableObject.AttackType;
        UnitsName = UnitsScriptableObject.UnitsName;
        HP = UnitsScriptableObject.HP;
        level = UnitsScriptableObject.level;
        Damage = UnitsScriptableObject.Damage;
        Defence = UnitsScriptableObject.Defence;
        WalkingSpeed = UnitsScriptableObject.WalkingSpeed;
        ChaseRange = UnitsScriptableObject.ChaseRange;
        AttackSpeed = UnitsScriptableObject.AttackSpeed;
        attackRange = UnitsScriptableObject.attackRange;
    }

    public float getWalkingSpeed(){
        return WalkingSpeed;
    }

    public float getAttackDamage(){
        return this.Damage;
    }
    //the unit will chase enmey unit in certein range
    public float getAttackRange(){
        return this.attackRange;
    }
    public string getUnitsName()
    {
        return this.UnitsName;
    }

    public UnitsType.UnitType GetUnitsType()
    {
        return this.UnitsType;
    }
    public float GetChaseRange() {
        return this.ChaseRange;
    }
    public void SetHP(float hp)
    {
        this.HP = hp;
    }

    public float GetHP()
    {
        return HP;
    }
}
