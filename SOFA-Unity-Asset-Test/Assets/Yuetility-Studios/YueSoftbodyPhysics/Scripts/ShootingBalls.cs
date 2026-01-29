using UnityEngine;
using UnityEngine.InputSystem;
using YuetilitySoftbody;

namespace YuetilitySoftbody
{
    public class ShootingBalls : MonoBehaviour
    {
        [SerializeField] private float mouseSpeed = 200f;
        [SerializeField] private float shootingSpeed = 25f;
        [SerializeField] private GameObject ballPrefab;

        private DefaultInputActions input;
        private float counter = 0f;

        private void Awake()
        {
            input = new DefaultInputActions();
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }

        private void Update()
        {
            HandleShooting();
            HandleLook();
            HandleCooldown();
        }

        private void HandleShooting()
        {
            if (input.Player.Fire.IsPressed() && counter <= 0f)
            {
                GameObject temp = Instantiate(
                    ballPrefab,
                    transform.position,
                    Quaternion.identity
                );

                temp.GetComponent<Rigidbody>().linearVelocity =
                    transform.forward * shootingSpeed;

                Destroy(temp, 5f);
                counter = 0.1f;
            }
        }

        private void HandleLook()
        {
            Vector2 mouseDelta = input.Player.Look.ReadValue<Vector2>();

            float yaw = mouseDelta.x * mouseSpeed * Time.deltaTime;
            float pitch = -mouseDelta.y * mouseSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up * yaw, Space.World);
            transform.Rotate(Vector3.right * pitch, Space.Self);

            // Lock roll
            Vector3 euler = transform.eulerAngles;
            transform.rotation = Quaternion.Euler(euler.x, euler.y, 0f);
        }

        private void HandleCooldown()
        {
            counter -= Time.deltaTime;
            if (counter < 0f)
                counter = 0f;
        }
    }
}