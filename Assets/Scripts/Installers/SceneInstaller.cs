using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Weapons;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private List<Transform> _navPoints;

        public override void InstallBindings()
        {
            Container.Bind<Transform>().WithId("Target").FromInstance(_targetTransform).AsSingle();

            foreach (var point in _navPoints)
            {
                Container.Bind<Transform>()
                    .WithId("NavPoint")
                    .FromInstance(point)
                    .AsCached();
            }
        }
    }
}