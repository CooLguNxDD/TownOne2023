using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnitController : MonoBehaviour, IUnitBehavior
{
    // Start is called before the first frame update

    public UnitsSetting unitsSetting;
    public NavMeshAgent NavMeshAgent;
    
    private bool targetFound;
    [SerializeField]
    private GameObject currentTargetObject;
    [SerializeField]
    private GameObject nextTargetWallObject;

    private float nextTargetCountDownTimer;
    private float nextWallCountDownTimer;

    public event EventHandler OnDeathAnimation;
    void Start()
    {
        nextTargetCountDownTimer = UnityEngine.Random.Range(0.5f, 3f);
        nextWallCountDownTimer = UnityEngine.Random.Range(0.5f, 3f);
        targetFound = false;
    }
    // Update is called once per frame
    void Update()
    {
        //start seeking
        SeekEnemy();
        //start Nav
        StartNav();

        Attack();

        if (unitsSetting.GetHP() <= 0)
        {
            // reach 0 HP, set it to inactive
            gameObject.SetActive(false);
        }

        // seek the next closer enemy after countdown
        // the units should not stick with one object in all time
        if(targetFound){
            nextTargetCountDownTimer -= Time.deltaTime;
            if(nextTargetCountDownTimer < 0f){
                targetFound = false;
                nextTargetCountDownTimer = UnityEngine.Random.Range(0.5f, 3f);
                //Debug.Log("next target");
            }
        }
    }

    //test the chasing area
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, unitsSetting.GetChaseRange());
    }

    public void setNavMeshSpeed(float speed){
        NavMeshAgent.speed = speed;
    }

    public void Attack(){

        if(!currentTargetObject) return;
        // Debug.Log(unitsSetting.getAttackRange());
        // Debug.Log(Vector3.Distance(transform.position, currentTargetObject.transform.position));

        if(Vector3.Distance(transform.position, currentTargetObject.transform.position) < unitsSetting.getAttackRange()){
            setNavMeshSpeed(0f);
            IGameObjectStatus targetStatus = currentTargetObject.GetComponent<IGameObjectStatus>();
            targetStatus.SetHP(targetStatus.GetHP() - unitsSetting.getAttackDamage());
            Debug.Log("target damaged: " + targetStatus.GetHP());
            unitsSetting.SetHP(0f); //killed when reached the wall
        }
        
        
    }

    //find the closest enemy
    public void SeekEnemy()
    {
        //TODO: need performance check right here

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
                        // Debug.Log("target found!" + status.GetHP());
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



    public void StartNav(){
        //set destination to a enemy
        if(targetFound){
            NavMeshAgent.SetDestination(currentTargetObject.transform.position);
            setNavMeshSpeed(unitsSetting.getWalkingSpeed());
        }
        //set destination to a wall
        else if(nextTargetWallObject){
            NavMeshAgent.SetDestination(nextTargetWallObject.transform.position);
            setNavMeshSpeed(unitsSetting.getWalkingSpeed());
        }
        //otherwise find the next wall if there is no target
        else{
            nextWallCountDownTimer -= Time.deltaTime;
            if(nextWallCountDownTimer < 0f){
                BuildingSetting[] Walls = GameObject.FindObjectsOfType<BuildingSetting>();

                if(Walls.Length == 0) return;

                float nearestDistance = 9999999f;
                foreach(BuildingSetting wall in Walls){
                    if(wall.GetUnitsType() == UnitsType.UnitType.ENEMY_UNIT){
                        if(Vector3.Distance(transform.position, wall.gameObject.transform.position) < nearestDistance){
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
