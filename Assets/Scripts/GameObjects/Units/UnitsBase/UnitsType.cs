using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitsType
{
    //define the type of an Units

    public enum UnitType
    {
        PLAYER_UNIT,
        ENEMY_UNIT,
        NONE
    }
    public enum AttackType
    {
        MELEE,
        RANGE
    }
}
