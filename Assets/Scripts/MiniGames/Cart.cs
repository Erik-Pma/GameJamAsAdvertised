using UnityEngine;

public class Cart : MonoBehaviour
{
    private InventoryController _inventory;
    void Start()
    {
        _inventory = FindObjectOfType<InventoryController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("moveable"))
        {
            _inventory.AddItem(other.collider.GetComponent<Items>().item);
            other.collider.GetComponent<DropAndRespawn>().Respawn();
        }
    }
}
