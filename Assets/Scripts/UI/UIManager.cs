using Game;
using Signals;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _ammoText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Image _screenFlash;
        [SerializeField] private Image _deathScreen;

        [SerializeField] private float _flashDuration = 0.3f;
        [SerializeField] private float _maxAlpha = 0.5f;

        [Inject] private SignalBus _signalBus;
        [Inject] private GameManager _gameManager;

        private Coroutine _flashCoroutine;
        
        private void Start()
        {
            _healthText.color = Color.green;
            _healthText.text = "100 HP";
            StartCoroutine(ShowLevelNumberRoutine());
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<DamageTakenSignal>(OnDamageTaken);
            _signalBus.Subscribe<HealthReceivedSignal>(OnHealthReceived);
            _signalBus.Subscribe<CurrentAmmoChangedSignal>(OnCurrentAmmoChanged);
            _signalBus.Subscribe<PlayerDiedSignal>(ShowDeathScreen);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<DamageTakenSignal>(OnDamageTaken);
            _signalBus.Unsubscribe<HealthReceivedSignal>(OnHealthReceived);
            _signalBus.Unsubscribe<CurrentAmmoChangedSignal>(OnCurrentAmmoChanged);
            _signalBus.Unsubscribe<PlayerDiedSignal>(ShowDeathScreen);
        }

        private void OnDamageTaken(DamageTakenSignal signal)
        {
            if(signal.CurrentHealth < 50 && signal.CurrentHealth >= 25) _healthText.color = Color.yellow;

            if (signal.CurrentHealth < 25) _healthText.color = Color.red;

            _healthText.text = $"{signal.CurrentHealth} HP";

            PerformScreenFlash(Color.red);
        }

        private void OnHealthReceived(HealthReceivedSignal signal)
        {
            if(signal.CurrentHealth >= 50) _healthText.color = Color.green;

            if (signal.CurrentHealth < 50 && signal.CurrentHealth >= 25) _healthText.color = Color.yellow;

            if (signal.CurrentHealth < 25) _healthText.color = Color.red;

            _healthText.text = $"{signal.CurrentHealth} HP";

            PerformScreenFlash(Color.green);
        }

        private void OnCurrentAmmoChanged(CurrentAmmoChangedSignal signal)
        {
            _ammoText.text = $"{signal.CurrentAmmo} Ammo";
        }

        private void PerformScreenFlash(Color color)
        {
            if (_flashCoroutine != null)
                StopCoroutine(_flashCoroutine);

            _flashCoroutine = StartCoroutine(ScreenFlashRoutine(color));
        }

        private IEnumerator ScreenFlashRoutine(Color color)
        {
            _screenFlash.gameObject.SetActive(true);

            color.a = _maxAlpha;
            _screenFlash.color = color;

            float timer = 0f;

            while (timer < _flashDuration)
            {
                timer += Time.deltaTime;

                float alpha = Mathf.Lerp(
                    _maxAlpha,
                    0f,
                    timer / _flashDuration);

                Color currentColor = _screenFlash.color;
                currentColor.a = alpha;
                _screenFlash.color = currentColor;

                yield return null;
            }

            Color finalColor = _screenFlash.color;
            finalColor.a = 0f;
            _screenFlash.color = finalColor;

            _screenFlash.gameObject.SetActive(false);
            _flashCoroutine = null;
        }

        private IEnumerator ShowLevelNumberRoutine()
        {
            _levelText.text = $"Level {_gameManager.CurrentSceneIndex + 1}";
            yield return new WaitForSeconds(2f);
            _levelText.gameObject.SetActive(false);
        }

        private void ShowDeathScreen()
        {
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
                _flashCoroutine = null;
            }

            _deathScreen.gameObject.SetActive(true);
        }
    }
}
