using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnitController : MonoBehaviour, IUnitBehavior
{

    //public class
    public UnitsSetting unitsSetting;
    public NavMeshAgent NavMeshAgent;

    public UnitsType.UnitType WillAttackUnitType;

    //Targets
    [SerializeField]
    private bool targetFound;
    private bool IsMouseLocation;
    [SerializeField]
    private GameObject currentTargetObject;
    [SerializeField]
    private GameObject nextTargetWallObject;

    //Counters
    [SerializeField]
    private float nextTargetCountDownTimer;
    [SerializeField]
    private float nextWallCountDownTimer;

    [SerializeField]
    private float nextAttackCountDownTimer;


    //Animation part
    public event EventHandler OnDeathAnimation;
    [SerializeField]
    private dissolverController animationController;

    //logic
    [SerializeField]
    private bool isDeath;

    [SerializeField]
    private bool isAttacking;
    
    void Start()
    {
        isDeath = false;
        nextTargetCountDownTimer = UnityEngine.Random.Range(0.5f, 3f);
        nextWallCountDownTimer = UnityEngine.Random.Range(5f, 10f);
        nextAttackCountDownTimer = 0;
        animationController.OnDeathAnimationEnded += DeathAfterAnimation;
        targetFound = false;
        isAttacking = false;
        NavMeshAgent.SetDestination(MouseController.Instance.mouseClickPosition);
        setNavMeshSpeed(unitsSetting.getWalkingSpeed());
    }
    // Update is called once per frame
    void Update()
    {
        // Debug.Log(unitsSetting.GetHP());
        if(isDeath) return;

        //start Nav
        StartNav();
        //start seeking
        SeekEnemy();

        Attack();

        if (unitsSetting.GetHP() <= 0)
        {
            // reach 0 HP, set it to inactive
            isDeath = true;
            OnDeathAnimation?.Invoke(this, EventArgs.Empty);
        }

        // seek the next closer enemy after countdown
        // the units should not stick with one object in all time
        if(targetFound){
            nextTargetCountDownTimer -= Time.deltaTime;
            if(nextTargetCountDownTimer < 0f){
                nextTargetCountDownTimer = UnityEngine.Random.Range(0.5f, 3f);
                targetFound = false;
                // Debug.Log("next target");
            }
        }
    }
    private void DeathAfterAnimation(object sender, System.EventArgs e){
        isAttacking = false;
        isDeath = false;
        nextWallCountDownTimer = UnityEngine.Random.Range(5f, 10f);
        unitsSetting.ResetSetting();
        gameObject.SetActive(false);

    }

    //test the chasing area
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitsSetting.GetChaseRange());
    }
    //start attack checking with count down
    public void Attack(){

        if(!currentTargetObject) {
            isAttacking = false;
            return;
        };

        nextAttackCountDownTimer -= Time.deltaTime;
        
        if(nextAttackCountDownTimer < 0){
            StartAttack();
            nextAttackCountDownTimer = unitsSetting.GetAttackSpeed();
        }
    }
    //deal one attack
    public void StartAttack(){
        //Debug.Log(unitsSetting.getAttackRange());
        //Debug.Log(Vector3.Distance(transform.position, currentTargetObject.transform.position));
        if(Vector3.Distance(transform.position, currentTargetObject.transform.position) < unitsSetting.getAttackRange()){
            setNavMeshSpeed(0f);
            isAttacking = true;
            IGameObjectStatus targetStatus = currentTargetObject.GetComponent<IGameObjectStatus>();
            targetStatus.takenDamage(unitsSetting.getAttackDamage());
            isAttacking = false;
            //Debug.Log("target damaged: " + targetStatus.GetHP());
            //unitsSetting.SetHP(0f); //killed when reached the wall
        
        }
        else{
            isAttacking = false;
            setNavMeshSpeed(unitsSetting.getWalkingSpeed());
        }
    }

    //find the closest enemy
    public void SeekEnemy()
    {
        //if the object has been destory
        if(currentTargetObject){
            if(currentTargetObject.activeInHierarchy == false){
                currentTargetObject = null;
                targetFound = false;
            }
        }
        if(nextTargetWallObject){
            if(nextTargetWallObject.activeInHierarchy == false){
                nextTargetWallObject = null;
            }
        }
        //TODO: need performance check right here
        if(isAttacking) return;
        
        //if not target found yet
        if(!targetFound){
            Collider[] colliders = Physics.OverlapSphere(transform.position, unitsSetting.GetChaseRange());
            if(colliders.Length == 0){
                return;
            }
            //find the closest target
            float nearestDistance = 9999999f;
            foreach (Collider collider in colliders){
                if(collider.TryGetComponent(out IGameObjectStatus status)){
                    if(status.GetUnitsType() == WillAttackUnitType){
                        float currentDistance = Vector3.Distance(transform.position, collider.gameObject.transform.position);
                        if(currentDistance < nearestDistance){
                            nearestDistance = currentDistance;
                            currentTargetObject = collider.gameObject;
                        }
                        //Debug.Log("target found!" + status.GetHP());
                        targetFound = true;
                    }
                }
            }
        }

    }

        public void setNavMeshSpeed(float speed){
        // if(speed <= 0f){
        //     NavMeshAgent.enabled = false;
        // }
        // else{
        //     NavMeshAgent.enabled = true;
        // }
        NavMeshAgent.speed = speed;
    }

    public void StartNav(){
        if(isAttacking) return;

        //set destination to a enemy
        if(targetFound){
            NavMeshAgent.SetDestination(currentTargetObject.transform.position);
            setNavMeshSpeed(unitsSetting.getWalkingSpeed());
        }

        //set destination to a wall
        if (nextTargetWallObject && !currentTargetObject)
        {
            NavMeshAgent.SetDestination(nextTargetWallObject.transform.position);
            setNavMeshSpeed(unitsSetting.getWalkingSpeed());
        }
        //otherwise find the next wall if there is no target
        else
        {
            nextWallCountDownTimer -= Time.deltaTime;
            if (nextWallCountDownTimer < 0f)
            {
                UnitsSetting[] Walls = GameObject.FindObjectsOfType<UnitsSetting>();

                if (Walls.Length == 0) return;

                float nearestDistance = 9999999f;
                foreach (UnitsSetting wall in Walls)
                {
                    if (wall.GetUnitsType() == WillAttackUnitType)
                    {
                        if (Vector3.Distance(transform.position, wall.gameObject.transform.position) < nearestDistance)
                        {
                            nearestDistance = Vector3.Distance(transform.position, wall.gameObject.transform.position);
                            nextTargetWallObject = wall.gameObject;
                        }
                        // Debug.Log("next wall found!" + wall.GetHP());
                    }
                }
                nextWallCountDownTimer = UnityEngine.Random.Range(5f, 10f);
            }
        }
        
    }
}
