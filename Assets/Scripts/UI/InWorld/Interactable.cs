using UnityEngine;
using UnityEngine.InputSystem;
namespace UI.InWorld
{
    public abstract class Interactable : MonoBehaviour
    {
        [Header("References")] 
        
        public InputActionAsset actionAsset;
        private InputAction _interactionAction;
        protected bool interationAmt;
        private BoxCollider _collider;
        public GameObject game;
        public Renderer renderer;

        private void Start()
        {
            _interactionAction = actionAsset.FindAction("Interact");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                UpdateUI(Color.white);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            interationAmt = _interactionAction.IsPressed();
            Interact();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                UpdateUI(Color.gray);
                game.SetActive(false);
            }
        }

        private void UpdateUI(Color color)
        {
            if(renderer != null)
                renderer.material.SetColor("_BaseColor", color);
        }

        protected abstract void Interact();

        

    }
    
}