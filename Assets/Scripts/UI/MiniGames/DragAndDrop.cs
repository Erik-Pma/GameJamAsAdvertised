using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.MiniGames
{
    public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
    {
        [SerializeField] private Canvas canvas;
        
        private Transform _transform;
        //private CanvasGroup _canvasGroup;
        private void Awake()
        {
            _transform = GetComponent<Transform>();
            //_canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log("OnBeginDrag");
            //_canvasGroup.blocksRaycasts = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log("OnEndDrag");
            //_canvasGroup.blocksRaycasts = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("OnDrag");
            _transform.position += new Vector3(transform.position.x +eventData.delta.x, transform.position.y + eventData.delta.y, 0);
        }
    }
}