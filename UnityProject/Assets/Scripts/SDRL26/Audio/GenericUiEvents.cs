using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SDRL26.Audio
{
   public class GenericUiEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
   {
      [SerializeField] private UnityEvent _pointerEnter = new();
      [SerializeField] private UnityEvent _pointerExit = new();
      [SerializeField] private UnityEvent _pointerDown = new();
      [SerializeField] private UnityEvent _pointerUp = new();

      public void OnPointerEnter(PointerEventData eventData) => _pointerEnter.Invoke();
      public void OnPointerExit(PointerEventData eventData) => _pointerExit.Invoke();
      public void OnPointerDown(PointerEventData eventData) => _pointerDown.Invoke();
      public void OnPointerUp(PointerEventData eventData) => _pointerUp.Invoke();
   }
}