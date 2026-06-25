using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager
    {
        private readonly List<string> _scenes;

        public int CurrentSceneIndex { get; private set; }

        public GameManager(List<string> scenes)
        {
            _scenes = scenes;
        }

        public void CompleteLevel()
        {
            KillAllTweens();

            if (CurrentSceneIndex + 1 >= _scenes.Count)
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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

        private void KillAllTweens()
        {
            DOTween.KillAll();
        }
    }
}
