using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SDRL26.Rendering.Battleground.DragAndDrop
{
   public class DraggableBattlerToken : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
   {
      [SerializeField] private BattlerTokenUi _token;

      public BattlerTokenUi Token => _token;

      public static UnityEvent<DraggableBattlerToken> OnDrag { get; } = new();
      public static UnityEvent<DraggableBattlerToken> OnDrop { get; } = new();

      public void OnPointerDown(PointerEventData eventData) => OnDrag.Invoke(this);
      public void OnPointerUp(PointerEventData eventData) => OnDrop.Invoke(this);
   }
}