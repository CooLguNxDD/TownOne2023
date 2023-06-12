using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class DemonKingUnitController : MonoBehaviour, IUnitBehavior{

    //public class
    public UnitsSetting unitsSetting;
    //public NavMeshAgent NavMeshAgent;

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
    public event EventHandler DemonKingAttackAnimationEvent;
    public event EventHandler DemonKingStopAttackAnimationEvent;
    [SerializeField]
    private dissolverController animationController;

    //logic
    [SerializeField]
    private bool isDeath;

    [SerializeField]
    private bool isAttacking;

    public float AreaEffect = 10f;
    public float reachingDistance = 5f;

    // Unity Events
    public UnityEvent onEndGame;
    
    void Start()
    {
        isDeath = false;
        nextTargetCountDownTimer = UnityEngine.Random.Range(0.5f, 3f);
        nextWallCountDownTimer = UnityEngine.Random.Range(5f, 10f);
        nextAttackCountDownTimer = 0f;
        animationController.OnDeathAnimationEnded += DeathAfterAnimation;
        targetFound = false;
        isAttacking = false;
        //NavMeshAgent.SetDestination(MouseController.Instance.mouseClickPosition);
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
        onEndGame.Invoke();
        gameObject.SetActive(false);
    }

    //test the chasing area
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitsSetting.GetChaseRange());

        Gizmos.color=Color.yellow;
        
        Gizmos.DrawWireSphere(transform.position-transform.forward*reachingDistance, AreaEffect);

        // Physics.SphereCast(transform.position,radius,-transform.up,out hit,maxDistance,~layerMask)
    }
    //start attack checking with count down
    public void Attack(){

        if(!currentTargetObject) {
            isAttacking = false;
            
            // DemonKingStopAttackAnimationEvent?.Invoke(this, EventArgs.Empty);
            return;
        };
        transform.LookAt(new Vector3(currentTargetObject.transform.position.x, transform.position.y, currentTargetObject.transform.position.z));

        nextAttackCountDownTimer -= Time.deltaTime;
        
        if(nextAttackCountDownTimer < 0){
            if(Vector3.Distance(transform.position, currentTargetObject.transform.position) < unitsSetting.getAttackRange()){
                //NavMeshAgent.isStopped = true;
                StartAttack();
                DemonKingAttackAnimationEvent?.Invoke(this, EventArgs.Empty);
                nextAttackCountDownTimer = unitsSetting.GetAttackSpeed();
            }
            else{
                isAttacking = false;
                //NavMeshAgent.isStopped = false;
                setNavMeshSpeed(unitsSetting.getWalkingSpeed());
            }
        }
    }
    //deal one attack
    public void StartAttack(){
        //Debug.Log(unitsSetting.getAttackRange());
        //Debug.Log(Vector3.Distance(transform.position, currentTargetObject.transform.position));
        
        IGameObjectStatus targetStatus = currentTargetObject.GetComponent<IGameObjectStatus>();


        // Debug.Log(targetStatus)
        setNavMeshSpeed(0f);
        isAttacking = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position-transform.forward*reachingDistance, AreaEffect);
        foreach (Collider collider in colliders){
            if(collider.TryGetComponent(out IGameObjectStatus status)){
                if(status.GetUnitsType() == WillAttackUnitType){
                    status.takenDamage(unitsSetting.getAttackDamage());

                }
            }
        }
        if(targetStatus.GetHP() < 0f){
            currentTargetObject = null;
            targetFound = false;
        }

        //Debug.Log("target damaged: " + targetStatus.GetHP());
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
        //NavMeshAgent.speed = speed;
    }

    public void StartNav(){
        if(isAttacking) return;

        //set destination to a enemy
        if(targetFound){
            //NavMeshAgent.SetDestination(currentTargetObject.transform.position);
            setNavMeshSpeed(unitsSetting.getWalkingSpeed());
        }

        //set destination to a wall
        if (nextTargetWallObject && !currentTargetObject)
        {
            //NavMeshAgent.SetDestination(nextTargetWallObject.transform.position);
            setNavMeshSpeed(unitsSetting.getWalkingSpeed());
        }
        //otherwise find the next wall if there is no target
        else
        {
            nextWallCountDownTimer -= Time.deltaTime;
            if (nextWallCountDownTimer < 0f)
            {
                BuildingSetting[] Walls = GameObject.FindObjectsOfType<BuildingSetting>();

                if (Walls.Length == 0) return;

                float nearestDistance = 9999999f;
                foreach (BuildingSetting wall in Walls)
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
