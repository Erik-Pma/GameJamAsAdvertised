using UnityEngine;

[RequireComponent(typeof( Rigidbody))]
public class DropAndRespawn : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField]private Vector3 respawnPosition;
    [SerializeField]private float minHeight = -3;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        respawnPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (transform.localPosition.y < minHeight)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        _rigidbody.isKinematic = true;
        transform.position = respawnPosition;
    }

    public void RigidbodyActive()
    {
        _rigidbody.isKinematic = false;
    }
}
