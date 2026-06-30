using Game;
using Services;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private List<string> _sceneNames;
    [SerializeField] private AudioService _audioService;

    public override void InstallBindings()
    {
        Container.Bind<GameManager>().AsSingle().WithArguments(_sceneNames).NonLazy();
        Container.Bind<IAudioService>().To<AudioService>().FromComponentInNewPrefab(_audioService).AsSingle().NonLazy();
    }
}