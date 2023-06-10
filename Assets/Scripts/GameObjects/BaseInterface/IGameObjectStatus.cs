using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameObjectStatus
{
    // Start is called before the first frame update

    public void SetHP(float hp);
    public float GetHP();

    public void takenDamage(float hp);
    public UnitsType.UnitType GetUnitsType();
    public string getUnitsName();
    public void ResetSetting();
}
