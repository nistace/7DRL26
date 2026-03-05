using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SDRL26.Rendering.Equipments
{
   public class DraggableEquipmentSlotUi : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
   {
      [SerializeField] private EquipmentSlotUi _equipmentSlot;

      public static UnityEvent<DraggableEquipmentSlotUi> OnDrag { get; } = new();
      public static UnityEvent<DraggableEquipmentSlotUi> OnDrop { get; } = new();
      public static UnityEvent<DraggableEquipmentSlotUi> OnHoverStarted { get; } = new();
      public static UnityEvent<DraggableEquipmentSlotUi> OnHoverStopped { get; } = new();
      public EquipmentSlotUi Slot => _equipmentSlot;

      public void OnPointerDown(PointerEventData eventData) => OnDrag.Invoke(this);
      public void OnPointerUp(PointerEventData eventData) => OnDrop.Invoke(this);
      public void OnPointerEnter(PointerEventData eventData) => OnHoverStarted.Invoke(this);
      public void OnPointerExit(PointerEventData eventData) => OnHoverStopped.Invoke(this);
   }
}