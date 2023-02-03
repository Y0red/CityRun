using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : Manager<GameEvents>
{
    public event Action<Platform> onRoadWayTriggerEnter;
    public event Action<Platform> onRoadWayTriggerExit;

    public event Action<Transform> OnPlatformCreated, OnDessable;

    public event Action OnGeneratePlatform, OnLoadMainMenu, OnLoadMainMenuDone;
    public event Action DoSlowMo;

    public void MainMenuLoaded()
    {
        if(OnLoadMainMenu != null)
        {
            OnLoadMainMenuDone();
        }
    }

    public void DoorWayTriggerEnter(Platform id)
    {
        if(onRoadWayTriggerEnter != null)
        {
            onRoadWayTriggerEnter(id);
        }
    }

    public void DoorWayTriggerExit(Platform id)
    {
        if(onRoadWayTriggerExit != null)
        {
            onRoadWayTriggerExit(id); 
        }
    }

    internal void LoadMainMenu()
    {
        if(OnLoadMainMenu != null)
        {
            OnLoadMainMenu();
        }
    }

    public void OnPlatformGenerated(Transform id)
    {
        if (OnPlatformCreated != null)
        {
            OnPlatformCreated(id);
        }
    }
    public void OnPlatformGenerate()
    {
        if (OnGeneratePlatform != null)
        {
            OnGeneratePlatform();
        }
    }
    public void OnDoSlowMO()
    {
        if(DoSlowMo != null)
        {
            DoSlowMo();
        }
    }
}
