using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLIfeTime : MonoBehaviour
{
    // Start is called before the first frame update
    public float LifeTime;
    void Start()
    {
        LifeTime = 15f;
    }
    public void Update(){
        LifeTime-= Time.deltaTime;
        if(LifeTime < 0f){
            LifeTime = 15f;
            gameObject.SetActive(false);
        }
    }
}
