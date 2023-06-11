using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public event EventHandler SpawnCountEvent;
    public int SpawnCount;
    // Start is called before the first frame update

    public void Awake(){
        if (Instance == null)
        {
            Debug.Log("only one GameManager instance available");
        }
        SpawnCount = 0;
        Instance = this;
    }
    void Start()
    {
       
    }


}
