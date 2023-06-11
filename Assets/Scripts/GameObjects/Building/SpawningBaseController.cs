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

    void Start()
    {
        if (!objectPool)
        {
            objectPool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        }
        MouseController.Instance.MouseOnClickEvent += SpawnOnClick;
    }

    private void SpawnOnClick(object sender, System.EventArgs args)
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f), transform.position.z);
        objectPool.SpawnFromPool(spawnObjectTag, pos, Quaternion.identity);
    }

}
