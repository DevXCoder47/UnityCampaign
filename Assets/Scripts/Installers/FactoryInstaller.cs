using Enemies;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class FactoryInstaller : MonoInstaller
    {
        [SerializeField] private BlueBot _blueBotPrefab;
        [SerializeField] private GreenBot _greenBotPrefab;
        [SerializeField] private RedBot _redBotPrefab;
        [SerializeField] private YellowTurret _yellowTurretPrefab;
        [SerializeField] private PinkDrone _pinkDronePrefab;
        [SerializeField] private Boss _bossPrefab;

        public override void InstallBindings()
        {
            Container.BindFactory<BlueBot, BlueBot.Factory>().FromComponentInNewPrefab(_blueBotPrefab);
            Container.BindFactory<GreenBot, GreenBot.Factory>().FromComponentInNewPrefab(_greenBotPrefab);
            Container.BindFactory<RedBot, RedBot.Factory>().FromComponentInNewPrefab(_redBotPrefab);
            Container.BindFactory<YellowTurret, YellowTurret.Factory>().FromComponentInNewPrefab(_yellowTurretPrefab);
            Container.BindFactory<PinkDrone, PinkDrone.Factory>().FromComponentInNewPrefab(_pinkDronePrefab);
            Container.BindFactory<Boss, Boss.Factory>().FromComponentInNewPrefab(_bossPrefab);
        }
    }
}