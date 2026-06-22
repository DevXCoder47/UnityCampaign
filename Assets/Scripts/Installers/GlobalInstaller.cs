using Game;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private List<string> _sceneNames;

    public override void InstallBindings()
    {
        Container.Bind<GameManager>().AsSingle().WithArguments(_sceneNames).NonLazy();
    }
}