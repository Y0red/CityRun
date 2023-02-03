using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MenuSystem
{
    public class MainMenu : MonoBehaviour
    {
        public Button play, store, exit;
        void Start()
        {
            play.onClick.AddListener(OnPlay);
            exit.onClick.AddListener(OnExit);
            GameEvents.Instance.MainMenuLoaded();
        }

        private void OnExit()
        {
            Application.Quit();
        }

        private void OnPlay()
        {
            GameEvents.Instance.OnPlatformGenerate();
        }
    }
}
