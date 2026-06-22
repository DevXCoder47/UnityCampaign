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

        public override void InstallBindings()
        {
            Container.BindFactory<BlueBot, BlueBot.Factory>().FromComponentInNewPrefab(_blueBotPrefab);
            Container.BindFactory<GreenBot, GreenBot.Factory>().FromComponentInNewPrefab(_greenBotPrefab);
            Container.BindFactory<RedBot, RedBot.Factory>().FromComponentInNewPrefab(_redBotPrefab);
        }
    }
}