using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Services
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioData _audioLibrary;
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private AudioSource _musicSource;

        [SerializeField] private int _poolSize;
        [SerializeField] private AudioSource _audioSourcePrefab;

        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SfxVolume";
        private const string MUSIC_ENABLED_KEY = "MusicEnabled";
        private const string SFX_ENABLED_KEY = "SfxEnabled";

        private List<AudioSource> _sfxSourcePool;
        private Coroutine _saveRoutine;

        private int _nextIndex;

        private float _musicVolume = 1f;
        private float _sfxVolume = 1f;
        private bool _musicEnabled = true;
        private bool _sfxEnabled = true;

        public float MusicVolume 
        {
            get => _musicVolume;
            set 
            {
                _musicVolume = Mathf.Clamp01(value);
                ApplyMusicVolume();

                if (_saveRoutine != null)
                {
                    StopCoroutine(_saveRoutine);
                    _saveRoutine = null;
                }
                _saveRoutine = StartCoroutine(SaveRoutine());
            } 
        }

        public float SfxVolume 
        { 
            get => _sfxVolume; 
            set
            {
                _sfxVolume = Mathf.Clamp01(value);
                ApplySfxVolume();

                if (_saveRoutine != null)
                {
                    StopCoroutine(_saveRoutine);
                    _saveRoutine = null;
                }
                _saveRoutine = StartCoroutine(SaveRoutine());
            } 
        }

        public bool MusicEnabled 
        { 
            get => _musicEnabled; 
            set
            {
                _musicEnabled = value;
                ApplyMusicVolume();

                if (_musicEnabled)
                    PlayMenuMusic();
                else
                    StopCurrentMusic();
                SaveSettings();
            } 
        }
        public bool SfxEnabled 
        { 
            get => _sfxEnabled; 
            set
            {
                _sfxEnabled = value;
                ApplySfxVolume();
                SaveSettings();
            } 
        }

        private void Awake()
        {
            _sfxSourcePool = new List<AudioSource>(_poolSize);

            for (int i = 0; i < _poolSize; i++)
            {
                AudioSource source = Instantiate(_audioSourcePrefab, transform);
                _sfxSourcePool.Add(source);
            }

            LoadSettings();
            ApplyAll();
        }

        private void OnApplicationQuit()
        {
            SaveSettings();
        }

        public void PlayButtonClick()
        {
            if (!_sfxEnabled)
                return;

            PlayClipAtPosition(_audioLibrary.buttonClick, Vector3.zero, false);
        }

        public void PlayEnemyShotSound(Vector3 position)
        {
            if (!_sfxEnabled)
                return;

            PlayClipAtPosition(_audioLibrary.enemyShot, position, true);
        }

        public void PlayGameMusic()
        {
            if (!_musicEnabled)
                return;

            if (_musicSource.clip == _audioLibrary.gameMusic && _musicSource.isPlaying)
                return;

            _musicSource.Stop();

            _musicSource.clip = _audioLibrary.gameMusic;
            _musicSource.loop = true;

            _musicSource.Play();
        }

        public void PlayMenuMusic()
        {
            if (!_musicEnabled)
                return;

            if (_musicSource.clip == _audioLibrary.menuMusic && _musicSource.isPlaying)
                return;

            _musicSource.Stop();

            _musicSource.clip = _audioLibrary.menuMusic;
            _musicSource.loop = true;

            _musicSource.Play();
        }

        public void PlayPlayerShotSound(Vector3 position)
        {
            if (!_sfxEnabled)
                return;

            PlayClipAtPosition(_audioLibrary.playerShot, position, true);
        }

        public void StopCurrentMusic()
        {
            _musicSource.Stop();
        }

        private AudioSource GetFreeSource()
        {
            foreach (var source in _sfxSourcePool)
            {
                if (!source.isPlaying)
                    return source;
            }

            AudioSource result = _sfxSourcePool[_nextIndex];

            _nextIndex++;

            if (_nextIndex >= _sfxSourcePool.Count)
                _nextIndex = 0;

            return result;
        }

        private void PlayClipAtPosition(AudioClip clip, Vector3 position, bool spatial)
        {
            AudioSource source = GetFreeSource();

            source.transform.position = position;
            source.spatialBlend = spatial ? 1f : 0f;

            source.PlayOneShot(clip);
        }

        private float LinearToDb(float value)
        {
            return value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
        }

        private void ApplyMusicVolume()
        {
            float volumeDb = _musicEnabled ? LinearToDb(_musicVolume) : -80f;
            _mixer.SetFloat("MusicVolume", volumeDb);
        }

        private void ApplySfxVolume()
        {
            float volumeDb = _sfxEnabled ? LinearToDb(_sfxVolume) : -80f;
            _mixer.SetFloat("SFXVolume", volumeDb);
        }

        private void ApplyAll()
        {
            ApplyMusicVolume();
            ApplySfxVolume();
        }

        private void SaveSettings()
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, _musicVolume);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, _sfxVolume);
            PlayerPrefs.SetInt(MUSIC_ENABLED_KEY, _musicEnabled ? 1 : 0);
            PlayerPrefs.SetInt(SFX_ENABLED_KEY, _sfxEnabled ? 1 : 0);

            PlayerPrefs.Save();
        }

        private void LoadSettings()
        {
            _musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
            _sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
            _musicEnabled = PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1) == 1;
            _sfxEnabled = PlayerPrefs.GetInt(SFX_ENABLED_KEY, 1) == 1;
        }

        private IEnumerator SaveRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            SaveSettings();
        }
    }
}
