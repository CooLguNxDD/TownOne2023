using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class CounterUI : MonoBehaviour
{
    // Start is called before the first frame update


    public TextMeshProUGUI TextMeshPro;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshPro.text = "Spawned: "+GameManager.Instance.SpawnCount;
    }
}
