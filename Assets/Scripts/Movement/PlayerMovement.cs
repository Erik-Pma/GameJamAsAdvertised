
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset actionAsset;
    
    private InputAction _moveAction;
    private InputAction _lookAction;

    private Vector2 _moveAmt;
    private Vector2 _lookAmt;
    private float _xRotation;
        
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 5.0f;
    
    private Transform _cameraTransform;
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
        _lookAction = actionAsset.FindAction("Look");
        _cameraTransform = GetComponentInChildren<Transform>();
    }
    private void Update()
    {
        _moveAmt = _moveAction.ReadValue<Vector2>();
        _lookAmt = _lookAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Movement();
        Rotating();
    }

    private void Movement()
    {
        //Debug.Log(_moveAmt);
        Vector3 movementVector = (transform.forward * _moveAmt.y + transform.right * _moveAmt.x) *moveSpeed * Time.deltaTime;
        transform.position += movementVector;
    }

    private void Rotating()
    {
        //Debug.Log(_lookAmt);
        float mouseX = _lookAmt.x * rotationSpeed * Time.deltaTime;
        float mouseY = _lookAmt.y * rotationSpeed * Time.deltaTime;
        
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        //_cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);   
    }
}
