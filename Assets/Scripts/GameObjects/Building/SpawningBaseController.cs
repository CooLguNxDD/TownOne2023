using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningBaseController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private ObjectPool objectPool;
    [SerializeField]
    private string spawnObjectTag;

    [SerializeField]
    private BuildingSetting buildingSetting;

    private float SpawnCountDown = 0f;

    
    
    void Start()
    {
        if(!objectPool){
            objectPool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        }
        SpawnCountDown = buildingSetting.getSpawnRate();
    }
    // Update is called once per frame
    void Update()
    {
        SpawnCountDown -= Time.deltaTime;
        if(SpawnCountDown <= 0){
            SpawnCountDown = buildingSetting.getSpawnRate();
            for(int i = 0 ; i < buildingSetting.getNumberOfSpawn(); i ++){
                SpawnUnitsFromPool();
            }
        }
    }

    void SpawnUnitsFromPool(){
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-2f, 2f),
        transform.position.y + Random.Range(-2f, 2f), transform.position.z);
        objectPool.SpawnFromPool(spawnObjectTag, transform.position, Quaternion.identity);
    }
}
