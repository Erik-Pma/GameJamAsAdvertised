using UnityEngine;
using UnityEngine.InputSystem;

public class MoveWithMouse : MonoBehaviour
{
    public InputActionAsset actionAsset;
    private InputAction _moveAction;
    private Vector2 _moveAmt;
    private Vector3 _offset = new Vector3(0, 0, 3);
    [SerializeField] private Camera camera;

    private void OnEnable()
    {
        _moveAction = actionAsset.FindAction("MoveCursor");
        transform.position = _offset;
    }

    private void Update()
    {
        _moveAmt = _moveAction.ReadValue<Vector2>();
        transform.position = new Vector3(_moveAmt.x/1000 + transform.position.x, _moveAmt.y/1000 + transform.position.y, _offset.z) ;
    }
}
