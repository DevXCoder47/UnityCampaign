using UnityEngine;
using Zenject;

namespace Enemies
{
    public class PinkDrone : Enemy
    {
        [SerializeField] private int _minSeconds;
        [SerializeField] private int _maxSeconds;

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

        public class Factory : PlaceholderFactory<PinkDrone>
        {

        }
    }
}
