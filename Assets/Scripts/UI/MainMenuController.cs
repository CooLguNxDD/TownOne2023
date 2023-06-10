using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{

    public Transform mainMenuBtn;

    public string GameSceneTag;

    int in_game;

    public void startNewGame() {
        //PanelManager.instance.loadCustomDifficulty();

        SceneManager.LoadScene(GameSceneTag);
        Debug.Log("doing shit");
        //PanelManager.instance.loadPanel("\nPlease choose your difficulty:", delegate {startNewGame((int)DIFFICULTY.EASY);}, delegate {startNewGame((int)DIFFICULTY.NORMAL);}, "Easy", "Normal", addButtons);
    }
    public void Start(){

    }
}
