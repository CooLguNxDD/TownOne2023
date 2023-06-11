using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    public DemonKingUnitController demonKingUnitController;
    public UnitsSetting unitsSetting;

    void Start()
    {
        demonKingUnitController.DemonKingAttackAnimationEvent += StartAttackAnimation;
        demonKingUnitController.DemonKingStopAttackAnimationEvent += StopAttackAnimation;
    }

    public void StartAttackAnimation(object sender, EventArgs args){
        animator.SetTrigger("isAttack");
    }


    public void StopAttackAnimation(object sender, EventArgs args){
        animator.SetBool("stopAttack" , false);
    }
}
