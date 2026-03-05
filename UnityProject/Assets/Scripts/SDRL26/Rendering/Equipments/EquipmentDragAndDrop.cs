using SDRL26.Battles.Equipments;
using SDRL26.GameControllers.GameStates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SDRL26.Rendering.Equipments
{
   public class EquipmentDragAndDrop : MonoBehaviour
   {
      [SerializeField] private GameStateTypes _enableDuringStates = GameStateTypes.PrepareBattle | GameStateTypes.BattleWon | GameStateTypes.NonBattleEncounter;
      [SerializeField] private Image _draggingEquipmentVisual;

      private DraggableEquipmentSlotUi _draggedEquipmentOrigin;
      private Equipment _draggedEquipment;
      private DraggableEquipmentSlotUi _hoveredSlot;

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

      private void RefreshEnabled() => enabled = GameState.CurrentState?.Is(_enableDuringStates) ?? false;

      private void OnEnable()
      {
         DraggableEquipmentSlotUi.OnDrag.AddListener(HandleEquipmentDragged);
         DraggableEquipmentSlotUi.OnDrop.AddListener(HandleEquipmentDropped);
         DraggableEquipmentSlotUi.OnHoverStarted.AddListener(HandleEquipmentHoverStarted);
         DraggableEquipmentSlotUi.OnHoverStopped.AddListener(HandleEquipmentHoverStopped);
      }

      private void HandleEquipmentHoverStopped(DraggableEquipmentSlotUi slot)
      {
         if (_hoveredSlot != slot) return;

         _hoveredSlot = null;
      }

      private void HandleEquipmentHoverStarted(DraggableEquipmentSlotUi slot) => _hoveredSlot = slot;

      private void OnDisable()
      {
         Drop(_draggedEquipmentOrigin);

         DraggableEquipmentSlotUi.OnDrag.RemoveListener(HandleEquipmentDragged);
         DraggableEquipmentSlotUi.OnDrop.RemoveListener(HandleEquipmentDropped);
      }

      private void HandleEquipmentDropped(DraggableEquipmentSlotUi droppedSlot)
      {
         Drop(_hoveredSlot);
      }

      private void Drop(DraggableEquipmentSlotUi destination)
      {
         if (!_draggedEquipment) return;

         _draggedEquipmentOrigin.Slot.SetEquipment(_draggedEquipment);

         if (destination && !destination.Slot.Equipment && destination != _draggedEquipmentOrigin)
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