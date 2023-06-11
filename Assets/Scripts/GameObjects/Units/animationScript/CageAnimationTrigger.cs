using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CageAnimationTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    public event EventHandler OnSBAnimationStart;
    public UnitsSetting unitsSetting;

    private bool isAnimationStart;
    void Start()
    {
        isAnimationStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(unitsSetting.GetHP() < 0 && !isAnimationStart){
            isAnimationStart = true;
            OnSBAnimationStart?.Invoke(this, EventArgs.Empty);
        }
    }
}
