using UnityEngine;

public interface IAudioService
{
    public void PlayMenuMusic();
    public void PlayGameMusic();
    public void PlayPlayerShotSound(Vector3 position);
    public void PlayEnemyShotSound(Vector3 position);
    public void PlayButtonClick();

    public void StopCurrentMusic();
}
