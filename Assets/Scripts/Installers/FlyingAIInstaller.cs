using FlyingAI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class FlyingAIInstaller : MonoInstaller
    {
        [SerializeField] private List<Waypoint> _waypoints;
        [SerializeField] private WaypointManager _waypointManager;

        public override void InstallBindings()
        {
            foreach (var waypoint in _waypoints)
            {
                Container.Bind<Waypoint>().WithId("Waypoint").FromInstance(waypoint).AsCached();
            }

            Container.Bind<WaypointPathfinder>().AsSingle().NonLazy();
            Container.Bind<WaypointManager>().FromComponentInNewPrefab(_waypointManager).AsSingle();
        }
    }
}
