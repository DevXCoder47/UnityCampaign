using Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
namespace UI
{
    public enum Navigation
    {
        MainMenu,
        Settings,
        Controls,
        Ending
    }

    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _controlsPanel;
        [SerializeField] private GameObject _endingPanel;

        [Inject] private GameManager _gameManager;

        private Dictionary<Navigation, GameObject> _panels;
        private Navigation _currentNavigation;

        private void Awake()
        {
            _panels = new()
            {
                { Navigation.MainMenu, _mainMenuPanel },
                { Navigation.Settings, _settingsPanel },
                { Navigation.Controls, _controlsPanel },
                { Navigation.Ending, _endingPanel }
            };
        }

        private void Start()
        {
            Navigate(_gameManager.GameFinished ? Navigation.Ending : Navigation.MainMenu);
        }

        private void ShowCurrentNavigation()
        {
            foreach (var panel in _panels.Values)
                panel.SetActive(false);

            _panels[_currentNavigation].SetActive(true);
        }

        private void Navigate(Navigation navigation)
        {
            _currentNavigation = navigation;
            ShowCurrentNavigation();
        }

        public void OnPlayButtonClick()
        {
            _gameManager.StartGame();
        }

        public void OnSettingsButtonClick()
        {
            Navigate(Navigation.Settings);
        }

        public void OnControlsButtonClick()
        {
            Navigate(Navigation.Controls);
        }

        public void OnMainMenuButtonClick()
        {
            Navigate(Navigation.MainMenu);
        }

        public void OnExitButtonClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }
}
