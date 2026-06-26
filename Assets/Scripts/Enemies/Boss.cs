using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;
using Zenject;

namespace Enemies
{
    public class Boss : Enemy
    {
        private Transform _movementTarget;

        [SerializeField] private int _minSeconds;
        [SerializeField] private int _maxSeconds;

        [SerializeField] private Weapon _machineGun;
        [SerializeField] private Weapon _turretGun;
        [SerializeField] private Weapon _firstTwinMachineGun;
        [SerializeField] private Weapon _secondTwinMachineGun;

        [Inject(Id = "NavPoint")] private List<Transform> _navPoints;

        protected override void Start()
        {
            base.Start();

            _agent.updateRotation = false;

            StartCoroutine(MovementLoop());
        }

        private void Update()
        {
            if (_isDead) return;

            RotateToTarget();

            if (IsInConus() && IsInPlaneView()) Shoot();

            if (_weapon.CurrentAmmo == 0) _weapon.Reload();
            if (_machineGun.CurrentAmmo == 0) _machineGun.Reload();
            if (_turretGun.CurrentAmmo == 0) _turretGun.Reload();
            if (_firstTwinMachineGun.CurrentAmmo == 0) _firstTwinMachineGun.Reload();
            if (_secondTwinMachineGun.CurrentAmmo == 0) _secondTwinMachineGun.Reload();
        }

        private IEnumerator MovementLoop()
        {
            while (true)
            {
                if (_isDead) yield break;

                SwitchMovementTarget();

                float waitTime = Random.Range(_minSeconds, _maxSeconds);
                yield return new WaitForSeconds(waitTime);
            }
        }

        private void SwitchMovementTarget()
        {
            if (_navPoints == null || _navPoints.Count == 0)
                return;

            int index = Random.Range(0, _navPoints.Count);
            _movementTarget = _navPoints[index];

            _agent.SetDestination(_movementTarget.position);
        }

        protected override void Shoot()
        {
            if (!_weapon.IsReloading) _weapon.Shoot();
            if (!_machineGun.IsReloading) _machineGun.Shoot();
            if (!_turretGun.IsReloading) _turretGun.Shoot();
            if (!_firstTwinMachineGun.IsReloading) _firstTwinMachineGun.Shoot();
            if (!_secondTwinMachineGun.IsReloading) _secondTwinMachineGun.Shoot();
        }

        public class Factory : PlaceholderFactory<Boss>
        {

        }
    }
}
