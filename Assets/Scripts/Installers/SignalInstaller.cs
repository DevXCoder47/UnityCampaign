using Signals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SignalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<DamageTakenSignal>();
            Container.DeclareSignal<HealthReceivedSignal>();
            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<EnemyDiedSignal>();
            Container.DeclareSignal<CurrentAmmoChangedSignal>();
        }
    }
}