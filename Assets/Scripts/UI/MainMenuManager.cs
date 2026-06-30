using Game;
using Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        [Header("Panels")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _controlsPanel;
        [SerializeField] private GameObject _endingPanel;

        [Header("Settings")]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _sfxToggle;

        [Inject] private GameManager _gameManager;
        [Inject] private IAudioService _audioService;

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
            _musicSlider.SetValueWithoutNotify(_audioService.MusicVolume);
            _sfxSlider.SetValueWithoutNotify(_audioService.SfxVolume);

            _musicToggle.SetIsOnWithoutNotify(_audioService.MusicEnabled);
            _sfxToggle.SetIsOnWithoutNotify(_audioService.SfxEnabled);

            _audioService.StopCurrentMusic();
            _audioService.PlayMenuMusic();
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
            _audioService.PlayButtonClick();
            _audioService.StopCurrentMusic();
            _audioService.PlayGameMusic();
            _gameManager.StartGame();
        }

        public void OnSettingsButtonClick()
        {
            _audioService.PlayButtonClick();
            Navigate(Navigation.Settings);
        }

        public void OnControlsButtonClick()
        {
            _audioService.PlayButtonClick();
            Navigate(Navigation.Controls);
        }

        public void OnMainMenuButtonClick()
        {
            _audioService.PlayButtonClick();
            Navigate(Navigation.MainMenu);
        }

        public void OnExitButtonClick()
        {
            _audioService.PlayButtonClick();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }

        public void OnMusicVolumeChanged(float value)
        {
            _audioService.MusicVolume = value;
        }

        public void OnSfxVolumeChanged(float value)
        {
            _audioService.SfxVolume = value;
        }

        public void OnMusicToggleChanged(bool value)
        {
            _audioService.MusicEnabled = value;
        }

        public void OnSfxToggleChanged(bool value)
        {
            _audioService.SfxEnabled = value;
        }
    }
}
