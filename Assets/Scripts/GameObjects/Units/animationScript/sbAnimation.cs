using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class sbAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public CageAnimationTrigger cageAnimationTrigger;

    public Animator animator;
    void Start()
    {
        cageAnimationTrigger.OnSBAnimationStart += StartDancing;
    }

    public void StartDancing(object sender, EventArgs args){
        animator.SetBool("isDance", true);
    }


}
