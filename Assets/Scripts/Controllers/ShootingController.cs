using UnityEngine;
using Weapons;
using Zenject;

namespace Controllers
{
    public class ShootingController : MonoBehaviour
    {
        [Inject(Id = "Rifle")]
        private Weapon _weapon;

        private void Update()
        {
            if(Input.GetMouseButton(0))
            {
                _weapon.Shoot();
            }
        }
    }
}
