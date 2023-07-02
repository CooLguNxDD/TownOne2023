using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public event EventHandler SpawnCountEvent;
    public int MinionCount;
    public int SwordCount;
    public int ShieldCount;

    
    // Start is called before the first frame update


    private int deathCount;
    public int homeBaseCount;
    public UnityEvent OnGameDeath;

    public float FB_Buff = 0f;


    public void Awake(){
        if (Instance == null)
        {
            Debug.Log("only one GameManager instance available");
        }
        MinionCount = 0;
        SwordCount = 0;
        ShieldCount = 0;
        Instance = this;
    }
    void Start()
    {
       deathCount = 0;
    }

    void Update()
    {
        if (deathCount >= homeBaseCount)
        {
            OnGameDeath.Invoke();
        }
    }

    public void increDeathCount()
    {
        deathCount++;
    }
}
