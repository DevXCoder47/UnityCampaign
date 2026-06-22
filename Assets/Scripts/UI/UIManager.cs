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
        [SerializeField] private Image _screenFlash;

        [SerializeField] private float _flashDuration = 0.3f;
        [SerializeField] private float _maxAlpha = 0.5f;

        [Inject] private SignalBus _signalBus;

        private Coroutine _flashCoroutine;

        private void Start()
        {
            _healthText.color = Color.green;
            _healthText.text = "10000 HP";
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<DamageTakenSignal>(OnDamageTaken);
            _signalBus.Subscribe<CurrentAmmoChangedSignal>(OnCurrentAmmoChanged);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<DamageTakenSignal>(OnDamageTaken);
            _signalBus.Unsubscribe<CurrentAmmoChangedSignal>(OnCurrentAmmoChanged);
        }

        private void OnDamageTaken(DamageTakenSignal signal)
        {
            if(signal.CurrentHealth < 50 && signal.CurrentHealth >= 25) _healthText.color = Color.yellow;

            if (signal.CurrentHealth < 25) _healthText.color = Color.red;

            _healthText.text = $"{signal.CurrentHealth} HP";

            PerformScreenFlash(Color.red);
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


    }
}
