using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Weapons;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Transform _playerTarget;
        [SerializeField] private Transform _playerHeadTransform;
        [SerializeField] private Transform _playerChestTransform;
        [SerializeField] private Transform _playerFeetTransform;

        [SerializeField] private List<Transform> _navPoints;

        public override void InstallBindings()
        {
            Container.Bind<Transform>().WithId("Target").FromInstance(_playerTarget).AsSingle();

            Container.Bind<Transform>().WithId("Head").FromInstance(_playerHeadTransform).AsCached();
            Container.Bind<Transform>().WithId("Chest").FromInstance(_playerChestTransform).AsCached();
            Container.Bind<Transform>().WithId("Feet").FromInstance(_playerFeetTransform).AsCached();

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