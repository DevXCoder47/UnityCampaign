using UnityEngine;
using Zenject;

namespace Controllers
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float gravity = 9.81f;
        [Inject(Id = "PlayerCamera")] private Transform cameraTransform;

        private CharacterController controller;
        private float verticalVelocity;

        void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 forward = cameraTransform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = cameraTransform.right;
            right.y = 0f;
            right.Normalize();

            Vector3 moveDirection = (forward * vertical + right * horizontal).normalized * speed;

            if (controller.isGrounded)
            {
                verticalVelocity = -1f;
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }

            moveDirection.y = verticalVelocity;

            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}
