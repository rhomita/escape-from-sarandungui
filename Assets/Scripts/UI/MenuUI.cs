using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private GameObject _difficultySelector;
        [SerializeField] private GameObject _credits;

        private string MAIN_LEVEL = "MainGame";

        private void Start()
        {
            ToggleDifficulty(false);
            ToggleCredits(false);
            _sceneLoader.gameObject.SetActive(true);
        }

        public void Play()
        {
            _sceneLoader.GoToScene(MAIN_LEVEL);
        }

        public void ToggleDifficulty(bool show)
        {
            _difficultySelector.SetActive(show);
        }
        
        public void ToggleCredits(bool show)
        {
            _credits.SetActive(show);
        }
        
        public void Exit()
        {
            Application.Quit();
        }
        
        public void SelectEasy()
        {
            DifficultyManager.Instance.SelectEasy();
        }

        public void SelectNormal()
        {
            DifficultyManager.Instance.SelectNormal();
        }

        public void SelectHard()
        {
            DifficultyManager.Instance.SelectHard();
        }
    }
}