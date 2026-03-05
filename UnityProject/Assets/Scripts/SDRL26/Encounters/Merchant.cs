using System;
using System.Collections.Generic;
using SDRL26.Battles.Equipments;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Encounters
{
   [Serializable]
   public class Merchant : IEncounter
   {
      public Sprite Portrait { get; }
      public string DisplayName { get; }
      public string Description { get; }

      public Equipment[] _equipmentPrefabs;
      public IReadOnlyList<Equipment> EquipmentPrefabs => _equipmentPrefabs;

      public UnityEvent OnChanged { get; } = new();

      public Merchant(Equipment[] equipmentPrefabs, Sprite portrait, string displayName, string description)
      {
         _equipmentPrefabs = equipmentPrefabs;
         Portrait = portrait;
         DisplayName = displayName;
         Description = description;
      }

      public bool TryGet(int index, out Equipment equipment)
      {
         equipment = null;

         if (index < 0 || index >= _equipmentPrefabs.Length) return false;
         if (_equipmentPrefabs[index] == null) return false;

         equipment = _equipmentPrefabs[index];

         return equipment;
      }

      public void Sell(int index)
      {
         if (index < 0 || index >= _equipmentPrefabs.Length) return;

         _equipmentPrefabs[index] = null;
         OnChanged.Invoke();
      }
   }
}