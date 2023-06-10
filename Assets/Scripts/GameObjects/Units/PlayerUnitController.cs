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
        nextWallCountDownTimer = UnityEngine.Random.Range(0.5f, 3f);
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
        //start seeking
        SeekEnemy();
        //start Nav
        StartNav();

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
                targetFound = false;
                
                nextTargetCountDownTimer = UnityEngine.Random.Range(0.5f, 3f);
                // Debug.Log("next target");
            }
        }
    }
    private void DeathAfterAnimation(object sender, System.EventArgs e){
        isAttacking = false;
        isDeath = false;
        unitsSetting.ResetSetting();
        gameObject.SetActive(false);

    }

    //test the chasing area
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitsSetting.GetChaseRange());
    }

    public void Attack(){

        if(!currentTargetObject) {
            isAttacking = false;
            return;
        };
        //Debug.Log(unitsSetting.getAttackRange());
        //Debug.Log(Vector3.Distance(transform.position, currentTargetObject.transform.position));
        if(Vector3.Distance(transform.position, currentTargetObject.transform.position) < unitsSetting.getAttackRange()){
            setNavMeshSpeed(0f);
            isAttacking = true;
            IGameObjectStatus targetStatus = currentTargetObject.GetComponent<IGameObjectStatus>();
            targetStatus.takenDamage(unitsSetting.getAttackDamage());
            //Debug.Log("target damaged: " + targetStatus.GetHP());

            unitsSetting.SetHP(0f); //killed when reached the wall
        
        }
        else{
            setNavMeshSpeed(unitsSetting.getWalkingSpeed());
        }
    }

    //find the closest enemy
    public void SeekEnemy()
    {
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
                    if(status.GetUnitsType() == UnitsType.UnitType.ENEMY_UNIT){
                        if(Vector3.Distance(transform.position, collider.gameObject.transform.position) < nearestDistance){
                            nearestDistance = Vector3.Distance(transform.position, collider.gameObject.transform.position);
                            currentTargetObject = collider.gameObject;
                        }
                        //Debug.Log("target found!" + status.GetHP());
                        targetFound = true;
                    }
                }
            }
        }
        //if the object has been destory
        else if(currentTargetObject){
            if(currentTargetObject.activeInHierarchy == false){
                currentTargetObject = null;
                targetFound = false;
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
        if (NavMeshAgent.remainingDistance <= 5)
        {
            //set destination to a wall
            if (nextTargetWallObject)
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
                    BuildingSetting[] Walls = GameObject.FindObjectsOfType<BuildingSetting>();

                    if (Walls.Length == 0) return;

                    float nearestDistance = 9999999f;
                    foreach (BuildingSetting wall in Walls)
                    {
                        if (wall.GetUnitsType() == UnitsType.UnitType.ENEMY_UNIT)
                        {
                            if (Vector3.Distance(transform.position, wall.gameObject.transform.position) < nearestDistance)
                            {
                                nearestDistance = Vector3.Distance(transform.position, wall.gameObject.transform.position);
                                nextTargetWallObject = wall.gameObject;
                            }
                            // Debug.Log("next wall found!" + wall.GetHP());
                        }
                    }
                    nextWallCountDownTimer = UnityEngine.Random.Range(0.5f, 3f);
                }
            }
        }
    }
}
