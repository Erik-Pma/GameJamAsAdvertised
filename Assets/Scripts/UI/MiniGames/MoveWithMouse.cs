using UnityEngine;
using UnityEngine.InputSystem;

public class MoveWithMouse : MonoBehaviour
{
    public InputActionAsset actionAsset;
    private InputAction _moveAction;
    private Vector2 _moveAmt;
    private Vector3 _offset = new Vector3(0, 0, 3);
    [SerializeField] private Camera camera;

    
    private void Awake()
    {
        //_moveAction = actionAsset.FindAction("Move");
    }

    private void Update()
    {
        //_moveAmt = _moveAction.ReadValue<Vector2>();
        transform.position = camera.ScreenToWorldPoint(Input.mousePosition) + _offset;
    }
}
