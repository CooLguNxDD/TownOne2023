using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DroppedShield : MonoBehaviour
{

    public PlayerUnitController controller;

    

    public void Start(){
        controller.OnDropShieldEvent += DropShield;
    }
    // Start is called before the first frame update

    public void DropShield(object sender, EventArgs args){

        ObjectPool.Instance.SpawnFromPool("dropped", transform.position, Quaternion.identity);
    }

}
