using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public float slowTime = 0.05f;
    public float amountsToBeSlowTime = 2f;

    private void Start()
    {
        GameEvents.Instance.DoSlowMo += DoSlowMotionEffect;
    }
    private void Update()
    {
        if(Time.timeScale < 1.0f)
        {
            SlowlyBackToNormal();
        }
    }

    private void SlowlyBackToNormal()
    {
        Time.timeScale += (1f / amountsToBeSlowTime) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
    }

    private void DoSlowMotionEffect()
    {
        Time.timeScale = slowTime;
        Time.fixedDeltaTime = Time.timeScale * .2f;
    }
}
