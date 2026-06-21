using Signals;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapons;
using Zenject;

namespace Controllers
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _restartDelay;

        [Inject] private SignalBus _signalBus;

        private float _currentHealth;
        private bool _isDead = false;

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (_isDead) return;
  
            _currentHealth -= damage;
            _signalBus.Fire(new DamageTakenSignal() { CurrentHealth = _currentHealth });

            if (_currentHealth <= 0)
            {
                Debug.Log("Player Died");
                _isDead = true;
                _signalBus.Fire<PlayerDiedSignal>();
                StartCoroutine(DieRoutine());
            }
        }

        private IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(_restartDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
