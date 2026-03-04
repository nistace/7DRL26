using SDRL26.Battles.Equipments;
using SDRL26.GameControllers;
using UnityEngine;

namespace SDRL26.Rendering.Equipments
{
   public class InventoryUi : MonoBehaviour
   {
      [SerializeField] private EquipmentSlotUi[] _slots;

      private Inventory _inventory;

      private void Start()
      {
         RefreshInventory();
         GameData.OnInventoryReset.AddListener(HandleInventoryReset);

         foreach (var slot in _slots)
         {
            slot.OnRemovalRequested.AddListener(HandleRemovalRequested);
            slot.OnSetRequested.AddListener(HandleSetRequested);
         }
      }

      private static void HandleSetRequested(EquipmentSlotUi slot, Equipment equipment) => GameData.Inventory[slot.Index] = equipment;
      private static void HandleRemovalRequested(EquipmentSlotUi slot) => GameData.Inventory[slot.Index] = null;

      private void OnDestroy()
      {
         GameData.OnInventoryReset.RemoveListener(HandleInventoryReset);

         foreach (var slot in _slots)
         {
            slot.OnRemovalRequested.RemoveListener(HandleRemovalRequested);
            slot.OnSetRequested.RemoveListener(HandleSetRequested);
         }
      }

      private void HandleInventoryReset() => RefreshInventory();

      private void RefreshInventory()
      {
         _inventory?.OnSlotChanged.RemoveListener(HandleInventoryChanged);

         _inventory = GameData.Inventory;

         if (_inventory != null)
         {
            _inventory.OnSlotChanged.AddListener(HandleInventoryChanged);

            for (uint i = 0; i < _slots.Length; i++)
            {
               _slots[i].SetEquipment(_inventory[i]);
            }
         }
      }

      private void HandleInventoryChanged(uint slotIndex) => _slots[slotIndex].SetEquipment(_inventory[slotIndex]);
   }
}