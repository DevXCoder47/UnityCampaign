using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
            LoadCurrentScene();
        }

        public void StartGame()
        {
            CurrentSceneIndex = 0;
            LoadCurrentScene();
        }

        private void LoadCurrentScene()
        {
            SceneManager.LoadScene(_scenes[CurrentSceneIndex]);
        }
    }
}
