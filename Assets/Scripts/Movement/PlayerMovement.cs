using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput _playerInput;
    
    InputAction _moveAction;

    [SerializeField] float _moveSpeed = 5.0f;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
    }

    private void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Debug.Log(_moveAction.ReadValue<Vector2>());
        Vector2 direction = _moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * Time.deltaTime * _moveSpeed;
    }
}
