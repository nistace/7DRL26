using SDRL26.Battles.Equipments;
using SDRL26.GameControllers.GameStates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SDRL26.Rendering.Equipments
{
   public class EquipmentDragAndDrop : MonoBehaviour
   {
      [SerializeField] private Image _draggingEquipmentVisual;
      private DraggableEquipmentSlotUi _draggedEquipmentOrigin;
      private Equipment _draggedEquipment;

      private void Start()
      {
         _draggingEquipmentVisual.gameObject.SetActive(false);
         GameState.OnStateChanged.AddListener(HandleStateChanged);
         RefreshEnabled();
      }

      private void OnDestroy()
      {
         GameState.OnStateChanged.RemoveListener(HandleStateChanged);
      }

      private void HandleStateChanged(GameState arg0) => RefreshEnabled();

      private void RefreshEnabled() => enabled = GameState.CurrentState is PrepareBattleGameState;

      private void OnEnable()
      {
         DraggableEquipmentSlotUi.OnDrag.AddListener(HandleEquipmentDragged);
         DraggableEquipmentSlotUi.OnDrop.AddListener(HandleEquipmentDropped);
      }

      private void OnDisable()
      {
         Drop(_draggedEquipmentOrigin);

         DraggableEquipmentSlotUi.OnDrag.RemoveListener(HandleEquipmentDragged);
         DraggableEquipmentSlotUi.OnDrop.RemoveListener(HandleEquipmentDropped);
      }

      private void HandleEquipmentDropped(DraggableEquipmentSlotUi droppedSlot)
      {
         var destination = droppedSlot;

         if (EventSystem.current.currentSelectedGameObject)
         {
            var hoveredSlot = EventSystem.current.currentSelectedGameObject.GetComponentInParent<DraggableEquipmentSlotUi>();

            if (hoveredSlot)
            {
               destination = hoveredSlot;
            }
         }

         Drop(destination);
      }

      private void Drop(DraggableEquipmentSlotUi destination)
      {
         if (!_draggedEquipment) return;

         _draggedEquipmentOrigin.Slot.SetEquipment(_draggedEquipment);

         if (!destination.Slot.Equipment && destination != _draggedEquipmentOrigin)
         {
            _draggedEquipmentOrigin.Slot.RequestRemoval();
            destination.Slot.RequestSet(_draggedEquipment);
         }

         _draggingEquipmentVisual.gameObject.SetActive(false);
         _draggedEquipment = null;
         _draggedEquipmentOrigin = null;
      }

      private void HandleEquipmentDragged(DraggableEquipmentSlotUi equipmentSlot)
      {
         if (_draggedEquipmentOrigin) return;
         if (!equipmentSlot.Slot.Equipment) return;

         _draggedEquipmentOrigin = equipmentSlot;
         _draggedEquipment = equipmentSlot.Slot.Equipment;
         _draggingEquipmentVisual.sprite = _draggedEquipment.Icon;
         _draggingEquipmentVisual.gameObject.SetActive(true);
         _draggedEquipmentOrigin.Slot.SetEquipment(null);
      }

      private void Update()
      {
         _draggingEquipmentVisual.transform.position = Mouse.current.position.ReadValue();
      }
   }
}