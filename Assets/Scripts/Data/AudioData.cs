using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
public class AudioData : ScriptableObject
{
    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("SFX")]
    public AudioClip playerShot;
    public AudioClip enemyShot;

    [Header("UI")]
    public AudioClip buttonClick;
}
