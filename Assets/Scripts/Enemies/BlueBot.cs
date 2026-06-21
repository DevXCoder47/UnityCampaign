using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using Zenject;

namespace Enemies
{
    public class BlueBot : Enemy
    {
        private Transform _movementTarget;

        [SerializeField] private int _minSeconds;
        [SerializeField] private int _maxSeconds;
        
        [Inject(Id = "NavPoint")] private List<Transform> _navPoints;

        private NavMeshAgent _agent;

        protected override void Start()
        {
            base.Start();

            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;

            StartCoroutine(MovementLoop());
        }

        private void Update()
        {
            RotateToTarget();

            if(IsInConus() && IsInPlaneView()) Shoot();
        }

        private IEnumerator MovementLoop()
        {
            while (true)
            {
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

        public class Factory : PlaceholderFactory<BlueBot>
        {

        }
    }
}
