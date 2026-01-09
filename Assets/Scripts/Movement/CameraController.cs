using UnityEngine;

namespace Movement
{
    public class CameraController : MonoBehaviour
    {
        public float minX = -60f;
        public float maxX = 60f;

        public float sensitivity;

        public Camera cam;

        private float _rotY;
        private float _rotX;

        PlayerMovement _move;

        private Vector2 _lookAmt;

        // Start is called before the first frame update
        void Start()
        {
            //lock cursor and set it to false so you can move the screen more easily
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _move = GetComponent<PlayerMovement>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _lookAmt = _move.lookAction.ReadValue<Vector2>();
            if (transform.position.y < -7)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            //get the camera and make it move to the mouse
            _rotY += _lookAmt.x * sensitivity * Time.deltaTime;
            _rotX += _lookAmt.y * sensitivity * Time.deltaTime;
            //clamp the rotation to it does over rotate
            _rotX = Mathf.Clamp(_rotX, minX, maxX);

            transform.localEulerAngles = new Vector3(0, _rotY, 0);
            cam.transform.localEulerAngles = new Vector3(-_rotX, 0, 0);
        }
    }
}
