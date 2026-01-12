using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveWithMouse : MonoBehaviour
{
    public InputActionAsset actionAsset;
    private InputAction _moveAction;
    private InputAction _pickupAction;
    private Vector2 _moveAmt;
    private bool _pickupAmt;
    private Vector3 _offset = new Vector3(0, 0, 3);
    private Vector3 _clampedPosition = Vector3.zero;
    [SerializeField] private Vector2 clampX;
    [SerializeField] private Vector2 clampY;
    private bool _held = false;
    Transform _item = null;

    private void OnEnable()
    {
        _moveAction = actionAsset.FindAction("MoveCursor");
        _pickupAction = actionAsset.FindAction("PickUp");
        
    }

    private void Awake()
    {
        transform.position = _offset;
    }

    private void Update()
    {
        _moveAmt = _moveAction.ReadValue<Vector2>();
        _pickupAmt =  _pickupAction.IsPressed();
        _clampedPosition = new Vector3(_moveAmt.x / 1000 + transform.position.x, _moveAmt.y / 1000 + transform.position.y, _offset.z);
        _clampedPosition.x = Mathf.Clamp(_clampedPosition.x, clampX.x, clampX.y);
        _clampedPosition.y = Mathf.Clamp(_clampedPosition.y,clampY.x - 7f, clampY.y - 7f);
        transform.position = _clampedPosition ;
        PickUp();
    }

    public void PickUp()
    {
        if (_pickupAmt)
        {
            Debug.Log("Picked up");
            Ray ray = new Ray(transform.position, Vector3.forward);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("moveable"))
                {
                    Debug.Log("Hit object: " + hit.transform.name);
                    Debug.Log("Hit point: " + hit.point);
                    _item = hit.transform;
                    _held = true;
                }
            }
            if (_held && _item != null)
                 _item.transform.position = new Vector3(transform.position.x, transform.position.y, _item.transform.position.z);
        }
        _held = false;
    }
}
