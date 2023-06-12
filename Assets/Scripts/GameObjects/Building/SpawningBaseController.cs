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
    private string spawnObjectTag2;

    public bool isClickControl = false;

    public float spawnerCounter = 0f;

    [SerializeField]
    private BuildingSetting buildingSetting;

    void Start()
    {
        if (!objectPool)
        {
            objectPool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        }
        if(isClickControl){
            MouseController.Instance.MouseOnLeftClickEvent += SpawnOnLeftClick;
            MouseController.Instance.MouseOnRightClickEvent += SpawnOnRightClick;
        }

    }
    void Update(){
        if(!isClickControl){
            spawnerCounter -= Time.deltaTime;
            if(spawnerCounter < 0f){
                SpawnObject();
                spawnerCounter = buildingSetting.getSpawnRate();
            }
        }
    }
    private void SpawnObject()
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f), transform.position.z);
        objectPool.SpawnFromPool(spawnObjectTag, pos, Quaternion.identity);

        GameManager.Instance.MinionCount += 1;
    }

    private void SpawnOnLeftClick(object sender, System.EventArgs args)
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f), transform.position.z);
        objectPool.SpawnFromPool(spawnObjectTag, pos, Quaternion.identity);

        GameManager.Instance.SwordCount += 1;
    }
    private void SpawnOnRightClick(object sender, System.EventArgs args)
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f), transform.position.z);
        objectPool.SpawnFromPool(spawnObjectTag2, pos, Quaternion.identity);

        GameManager.Instance.ShieldCount += 1;
    }


}
