using FlyingAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class PinkDrone : Enemy
    {
        [SerializeField] private float _flyingSpeed;

        [Inject] private WaypointPathfinder _waypointPathfinder;
        [Inject] private WaypointManager _waypointManager;

        private List<Waypoint> _currentPath;
        private int _currentWaypointIndex;

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

            MoveAlongPath();

            if (_currentPath == null)
            {
                SwitchMovementTarget();
            }
        }

        protected override void Shoot()
        {
            if (!_weapon.IsReloading) _weapon.Shoot();
        }

        private void SwitchMovementTarget()
        {
            Waypoint start = _waypointManager.GetClosest(transform.position);

            Waypoint goal = _waypointManager.GetRandom();

            _currentPath = _waypointPathfinder.FindPath(start, goal);

            _currentWaypointIndex = 0;
        }

        private void MoveAlongPath()
        {
            if (_currentPath == null ||
                _currentPath.Count == 0)
            {
                return;
            }

            Waypoint targetWaypoint =
                _currentPath[_currentWaypointIndex];

            Vector3 targetPosition =
                targetWaypoint.transform.position;

            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    _flyingSpeed * Time.deltaTime);

            if (Vector3.Distance(
                    transform.position,
                    targetPosition) < 0.2f)
            {
                _currentWaypointIndex++;

                if (_currentWaypointIndex >= _currentPath.Count)
                {
                    _currentPath = null;
                }
            }
        }

        public class Factory : PlaceholderFactory<PinkDrone>
        {

        }
    }
}
