using SDRL26.Rendering.Shared;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SDRL26.Rendering.Battleground.DragAndDrop
{
   public class DraggableBattlerToken : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
   {
      [SerializeField] private BattlerTokenUi _token;
      [SerializeField] private SmoothMover _smoothMover;

      public BattlerTokenUi Token => _token;
      public SmoothMover SmoothMover => _smoothMover;

      public static UnityEvent<DraggableBattlerToken> OnDrag { get; } = new();
      public static UnityEvent<DraggableBattlerToken> OnDrop { get; } = new();

      public void OnPointerDown(PointerEventData eventData) => OnDrag.Invoke(this);
      public void OnPointerUp(PointerEventData eventData) => OnDrop.Invoke(this);
   }
}