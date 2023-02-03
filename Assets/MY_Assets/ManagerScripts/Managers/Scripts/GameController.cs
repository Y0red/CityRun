using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using MenuSystem;

public class GameController : MonoBehaviour
{
    public enum GameState { PREGAME, PLAYING, PAUSE, MENU, LogIn };

    public GameState currentGameState = GameState.PREGAME;

    public EventGameState OnGameStateChanged;

    public GameObject player;
   // public TPUC tPCU;


    void Start()
    {
        GameEvents.Instance.OnPlatformCreated += OnPlatformCreated;
        GameEvents.Instance.OnLoadMainMenuDone += DoneLoadingMainMenu;
    }

    private void DoneLoadingMainMenu()
    {
        player.SetActive(true);
    }

    private void OnPlatformCreated(Transform obj)
    {
        UiManager.Instance.LoadMenu("Game_Play_Menu");
        player.gameObject.SetActive(false);
        if (obj != null)
        {
            player.transform.position = obj.position;
            player.gameObject.SetActive(true);

        }

       // tPCU.v = 1;
    }

    
}
[Serializable] public class EventGameState : UnityEvent<GameController.GameState, GameController.GameState> { }