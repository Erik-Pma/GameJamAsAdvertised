using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput _playerInput;
    
    InputAction _moveAction;
    InputAction _cameraAction;
    
    Transform _cameraTransform;

    private float _xRotation;

    [SerializeField] float _moveSpeed = 5.0f;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
        _cameraAction = _playerInput.actions.FindAction("Camera");
        _cameraTransform = GetComponentInChildren<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MovePlayer();
        MoveCamera();
    }

    void MovePlayer()
    {
        Debug.Log(_moveAction.ReadValue<Vector2>());
        Vector2 direction = _moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * Time.deltaTime * _moveSpeed;
    }

    void MoveCamera()
    {
        Debug.Log(_cameraAction.ReadValue<Vector2>());
        
        float mouseX = _cameraAction.ReadValue<Vector2>().x * 10* Time.deltaTime;
        float mouseY = _cameraAction.ReadValue<Vector2>().y * 50* Time.deltaTime;
        
        _xRotation -= mouseY;
        _xRotation =  Mathf.Clamp(_xRotation, -80f, 90f);
        
        _cameraTransform.localRotation = Quaternion.Euler( _xRotation,0 , 0);
    }
}
