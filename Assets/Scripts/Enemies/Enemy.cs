using Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using Zenject;
using static UnityEngine.GraphicsBuffer;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _maxHealth; 

        [SerializeField] private float _viewAngle = 60f;
        [SerializeField] private float _maxConusRadius = 20f;
        [SerializeField] private float _minConusRadius = 0f;

        [SerializeField] protected NavMeshAgent _agent;
        [SerializeField] protected Weapon _weapon;

        [Inject(Id = "Target")] private Transform _playerTarget;

        [Inject(Id = "Head")] private Transform _playerHead;
        [Inject(Id = "Chest")] private Transform _playerChest;
        [Inject(Id = "Feet")] private Transform _playerFeet;


        [Inject] private SignalBus _signalBus;

        protected bool _isDead = false;
        private float _currentHealth;
        private Rigidbody[] _ragdollBodies;

        protected virtual void Awake()
        {
            _ragdollBodies = GetComponentsInChildren<Rigidbody>(true);

            foreach (var rb in _ragdollBodies)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        protected virtual void Start()
        {
            _currentHealth = _maxHealth;
        }


        protected void RotateToTarget()
        {
            if (_playerTarget == null)
                return;

            Vector3 direction = _playerTarget.position - transform.position;

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
            if (_playerTarget == null)
                return false;

            Vector3 toTarget = _playerTarget.position - transform.position;

            float distance = toTarget.magnitude;

            if (distance < _minConusRadius || distance > _maxConusRadius)
                return false;

            float angle = Vector3.Angle(transform.forward, toTarget);

            return angle <= _viewAngle * 0.5f;
        }

        protected bool IsInPlaneView()
        {
            return CanSeePoint(_playerHead) || CanSeePoint(_playerChest) || CanSeePoint(_playerFeet);
        }

        private bool CanSeePoint(Transform point)
        {
            Vector3 origin = transform.position;
            Vector3 direction = point.position - origin;

            float distance = direction.magnitude;

            if (Physics.Raycast(origin, direction.normalized,
                out RaycastHit hit, distance))
            {
                return hit.collider.CompareTag("Player");
            }

            return false;
        }

        public void TakeDamage(float damage)
        {
            if (_isDead) return;

            _currentHealth -= damage;

            if(_currentHealth <= 0)
            {
                _signalBus.Fire<EnemyDiedSignal>();
                StartCoroutine(DieRoutine());
            }
        }

        private IEnumerator DieRoutine()
        {
            _isDead = true;

            EnableRagdoll();

            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }

        private void EnableRagdoll()
        {
            if (_agent != null)
                _agent.enabled = false;

            foreach (var rb in _ragdollBodies)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddForce(Random.insideUnitSphere * 10f, ForceMode.Impulse);
            }
        }
    }
}
