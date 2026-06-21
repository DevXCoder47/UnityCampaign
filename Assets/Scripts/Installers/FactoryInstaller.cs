using Enemies;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class FactoryInstaller : MonoInstaller
    {
        [SerializeField] private BlueBot _blueBotPrefab;
        public override void InstallBindings()
        {
            Container.BindFactory<BlueBot, BlueBot.Factory>().FromComponentInNewPrefab(_blueBotPrefab);
        }
    }
}