using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput _playerInput;
    
    InputAction _moveAction;
    InputAction _cameraAction;
    
    Transform _cameraTransform;

    [SerializeField] float _moveSpeed = 5.0f;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
        _cameraAction = _playerInput.actions.FindAction("Camera");
        _cameraTransform = GetComponentInChildren<Transform>();

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
    }
}
