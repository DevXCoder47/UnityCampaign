using System.Runtime.CompilerServices;
using UnityEngine;
using Weapons;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Rifle _rifle;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform cameraTransform;
    public override void InstallBindings()
    {
        Container.Bind<Weapon>().WithId("Rifle").FromInstance(_rifle).AsSingle();
        Container.Bind<Camera>().FromInstance(_camera).AsSingle();
        Container.Bind<Transform>().WithId("PlayerCamera").FromInstance(cameraTransform).AsSingle();
    }
}