using Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;

        [Inject] private SignalBus _signalBus;

        private void Start()
        {
            _healthText.color = Color.green;
            _healthText.text = "100 HP";
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<DamageTakenSignal>(OnDamageTaken);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<DamageTakenSignal>(OnDamageTaken);
        }

        private void OnDamageTaken(DamageTakenSignal signal)
        {
            if(signal.CurrentHealth < 50 && signal.CurrentHealth >= 25) _healthText.color = Color.yellow;

            if (signal.CurrentHealth < 25) _healthText.color = Color.red;

            _healthText.text = $"{signal.CurrentHealth} HP";
        }
    }
}
