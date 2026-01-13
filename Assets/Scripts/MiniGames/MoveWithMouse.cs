using UnityEngine;
using UnityEngine.InputSystem;

public class MoveWithMouse : MonoBehaviour
{
    public InputActionAsset actionAsset;
    private InputAction _moveAction;
    private InputAction _pickupAction;
    private Vector2 _moveAmt;
    private bool _pickupAmt;
    [SerializeField]private Vector3 _offset = new Vector3(0, 0, 3);
    private Vector3 _clampedPosition = Vector3.zero;
    [SerializeField] private Vector2 clampX;
    [SerializeField] private Vector2 clampY;
    [ReadOnly]
    [SerializeField]private DropAndRespawn _dropAndRespawn;
    
    // We don't strictly need _held anymore, checking if _item != null is enough
    private Transform _item; 

    private void OnEnable()
    {
        _moveAction = actionAsset.FindAction("MoveCursor");
        _pickupAction = actionAsset.FindAction("PickUp");
        
        _moveAction.Enable(); 
        _pickupAction.Enable();
    }

    private void Awake()
    {
        transform.position = _offset;
        
    }

    private void Update()
    {
        _moveAmt = _moveAction.ReadValue<Vector2>();
        _pickupAmt = _pickupAction.IsPressed();

        // Calculate Position
        _clampedPosition = new Vector3(_moveAmt.x / 1000 + transform.position.x, _moveAmt.y / 1000 + transform.position.y, _offset.z);
        _clampedPosition.x = Mathf.Clamp(_clampedPosition.x, clampX.x, clampX.y);
        _clampedPosition.y = Mathf.Clamp(_clampedPosition.y, clampY.x - 7f, clampY.y - 7f);
        
        transform.position = _clampedPosition;
        
        // Handle Interaction
        PickUp();
    }

    public void PickUp()
    {
        // If the button is NOT pressed, drop everything.
        if (!_pickupAmt)
        {
            if (_item != null)
            {
                _dropAndRespawn.RigidbodyActive();
                _item = null;
                _dropAndRespawn = null;
            }

            return;
        }

        // If the button IS pressed, but we aren't holding anything yet, try to find an object.
        if (_item == null)
        {
            Ray ray = new Ray(transform.position, Vector3.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("moveable"))
                {
                    Debug.Log("Picked up: " + hit.transform.name);
                    _item = hit.transform;
                    _dropAndRespawn = _item.gameObject.GetComponent<DropAndRespawn>();
                }
            }
        }

        // If we have an item (either just found or holding from before), move it.
        // This runs even if the raycast misses this frame, preventing the "drop".
        if (_item != null)
        {
             _item.position = new Vector3(transform.position.x, transform.position.y, _item.position.z);
        }
    }
}
