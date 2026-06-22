using Signals;
using System.Collections.Generic;
using UnityEngine;
using Weapons;
using Zenject;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _maxHealth; 

        [SerializeField] private float _viewAngle = 60f;
        [SerializeField] private float _maxConusRadius = 20f;
        [SerializeField] private float _minConusRadius = 0f;

        [SerializeField] protected Weapon _weapon;

        [Inject(Id = "Target")] private Transform _target;
        [Inject] private SignalBus _signalBus; 

        private float _currentHealth;

        protected virtual void Start()
        {
            _currentHealth = _maxHealth;
        }

        protected void RotateToTarget()
        {
            if (_target == null)
                return;

            Vector3 direction = _target.position - transform.position;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                _turnSpeed * Time.deltaTime);
        }

        protected abstract void Shoot();
        
        protected bool IsInConus()
        {
            if (_target == null)
                return false;

            Vector3 toTarget = _target.position - transform.position;

            float distance = toTarget.magnitude;

            if (distance < _minConusRadius || distance > _maxConusRadius)
                return false;

            float angle = Vector3.Angle(transform.forward, toTarget);

            return angle <= _viewAngle * 0.5f;
        }

        protected bool IsInPlaneView()
        {
            if (_target == null)
                return false;

            Vector3 origin = transform.position;
            Vector3 direction = (_target.position - origin).normalized;

            float distance = Vector3.Distance(origin, _target.position);

            if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
            {
                return hit.transform == _target &&
                       hit.transform.CompareTag("Player");
            }

            return false;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;

            if(_currentHealth <= 0)
            {
                _signalBus.Fire<EnemyDiedSignal>();
                Destroy(gameObject);
            }
        }
    }
}
