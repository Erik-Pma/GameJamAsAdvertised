using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.MiniGames
{
    public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
    {
        [SerializeField] private Canvas canvas;
        
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log("OnBeginDrag");
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log("OnEndDrag");
            _canvasGroup.blocksRaycasts = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("OnDrag");
            _rectTransform.anchoredPosition += eventData.delta;
        }
    }
}