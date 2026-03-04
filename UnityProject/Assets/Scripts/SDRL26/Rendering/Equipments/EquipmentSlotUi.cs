using System;
using SDRL26.Battles.Equipments;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDRL26.Rendering.Equipments
{
   public class EquipmentSlotUi : MonoBehaviour
   {
      [SerializeField] private Image _equipmentImage;
      [SerializeField] private Equipment _equipment;

      public Equipment Equipment
      {
         get => _equipment;
         private set => _equipment = value;
      }

      public uint Index => (uint)transform.GetSiblingIndex();

      public UnityEvent<EquipmentSlotUi, Equipment> OnSetRequested { get; } = new();
      public UnityEvent<EquipmentSlotUi> OnRemovalRequested { get; } = new();

      private void Start()
      {
         SetEquipment(_equipment);
      }

      public void SetEquipment(Equipment equipment)
      {
         Equipment = equipment;
         _equipmentImage.enabled = equipment;
         _equipmentImage.sprite = equipment ? equipment.Icon : null;
      }

      public void RequestRemoval()
      {
         if (!Equipment) return;

         OnRemovalRequested.Invoke(this);
      }

      public void RequestSet(Equipment equipment)
      {
         if (Equipment) return;

         OnSetRequested.Invoke(this, equipment);
      }
   }
}