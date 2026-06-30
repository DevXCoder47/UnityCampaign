using UnityEngine;

namespace Services
{
    public interface IAudioService
    {
        float MusicVolume { get; set; }
        float SfxVolume { get; set; }

        bool MusicEnabled { get; set; }
        bool SfxEnabled { get; set; }

        public void PlayMenuMusic();
        public void PlayGameMusic();
        public void PlayPlayerShotSound(Vector3 position);
        public void PlayEnemyShotSound(Vector3 position);
        public void PlayButtonClick();

        public void StopCurrentMusic();
    }
}
