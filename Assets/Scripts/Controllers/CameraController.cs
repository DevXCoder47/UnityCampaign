using Signals;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float speed;

        [Inject] private SignalBus _signalBus;

        private float rotationX;
        private float rotationY;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            rotationX += mouseX * speed * Time.deltaTime;
            rotationY -= mouseY * speed * Time.deltaTime;

            rotationY = Mathf.Clamp(rotationY, -80f, 80f);

            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }

        private void OnPlayerDied()
        {
            enabled = false;
        }
    }
}
