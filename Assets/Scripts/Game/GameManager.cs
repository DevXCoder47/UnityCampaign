using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEditorInternal;

namespace Game
{
    public class GameManager
    {
        private readonly List<string> _scenes;

        public int CurrentSceneIndex { get; private set; }
        public bool GameFinished { get; set; } = false;

        public GameManager(List<string> scenes)
        {
            _scenes = scenes;
        }

        public void CompleteLevel()
        {
            KillAllTweens();

            if (CurrentSceneIndex + 1 >= _scenes.Count)
            {
                GameFinished = true;

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;

                LoadMainMenuScene();
                return;
            }

            CurrentSceneIndex++;
            LoadCurrentScene();
        }

        public void RestartLevel()
        {
            KillAllTweens();
            LoadCurrentScene();
        }

        public void StartGame()
        {
            KillAllTweens();
            CurrentSceneIndex = 0;
            LoadCurrentScene();
        }

        private void LoadCurrentScene()
        {
            SceneManager.LoadScene(_scenes[CurrentSceneIndex]);
        }

        private void LoadMainMenuScene()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void KillAllTweens()
        {
            DOTween.KillAll();
        }
    }
}
