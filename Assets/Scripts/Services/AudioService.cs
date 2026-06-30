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

        private List<AudioSource> _sfxSourcePool;

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
            } 
        }

        public float SfxVolume 
        { 
            get => _sfxVolume; 
            set
            {
                _sfxVolume = Mathf.Clamp01(value);
                ApplySfxVolume();
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
            } 
        }
        public bool SfxEnabled 
        { 
            get => _sfxEnabled; 
            set
            {
                _sfxEnabled = value;
                ApplySfxVolume();
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

            ApplyAll();
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
    }
}
