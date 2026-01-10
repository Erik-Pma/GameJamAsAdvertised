
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("InputMapping")]
        public InputActionAsset actionAsset;

        private InputAction _moveAction;
        [HideInInspector] public InputAction lookAction;

        private Vector2 _moveAmt;
        private float _xRotation;
        [Header("values")] public float moveSpeed = 5.0f;
        [Header("CharacterController")] public CharacterController controller;

        private void OnEnable()
        {
            actionAsset.FindActionMap("Movement").Enable();
        }

        private void OnDisable()
        {
            actionAsset.FindActionMap("Movement").Disable();
        }

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _moveAction = actionAsset.FindAction("Move");
            lookAction = actionAsset.FindAction("Look");
        }

        private void Update()
        {
            _moveAmt = _moveAction.ReadValue<Vector2>();
            Movement();
        }

        private void Movement()
        {
            //Debug.Log(_moveAmt);
            Vector3 movementVector = moveSpeed * Time.deltaTime * (transform.forward * _moveAmt.y + transform.right * _moveAmt.x);
            controller.Move(movementVector);
        }
    }
}
