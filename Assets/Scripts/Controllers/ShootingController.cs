using Signals;
using UnityEngine;
using Weapons;
using Zenject;

namespace Controllers
{
    public class ShootingController : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        
        [Inject] private SignalBus _signalBus;

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void Update()
        {
            if(Input.GetMouseButton(0))
            {
                _weapon.Shoot();
            }
        }

        private void OnPlayerDied()
        {
            enabled = false;
        }
    }
}
