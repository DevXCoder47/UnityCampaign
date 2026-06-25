using UnityEngine;
using Zenject;

namespace Enemies
{
    public class YellowTurret : Enemy
    {
        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (_isDead) return;

            RotateToTarget();

            if (IsInConus() && IsInPlaneView()) Shoot();

            if (_weapon.CurrentAmmo == 0) _weapon.Reload();
        }

        protected override void Shoot()
        {
            if (!_weapon.IsReloading) _weapon.Shoot();
        }

        public class Factory : PlaceholderFactory<YellowTurret>
        {

        }
    }
}
