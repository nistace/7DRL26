using System;
using System.Linq;
using UnityEngine.Events;
using Utilities;

namespace SDRL26.Battles.Equipments
{
   [Serializable]
   public class Inventory
   {
      private int _gold;
      private Equipment[] _equipments = Array.Empty<Equipment>();

      public Equipment this[uint index]
      {
         get => index < 0 || index >= _equipments.Length ? null : _equipments[index];
         set
         {
            if (index >= _equipments.Length)
            {
               Array.Resize(ref _equipments, (int)index + 1);
            }

            _equipments[index] = value;
            OnSlotChanged.Invoke(index);
         }
      }

      public int Gold
      {
         get => _gold;
         set
         {
            _gold = value;
            OnGoldChanged.Invoke(_gold);
         }
      }

      public Equipment[] AllEquipments => _equipments.Where(t => t != null).ToArray();

      public UnityEvent<uint> OnSlotChanged { get; } = new();
      public UnityEvent<int> OnGoldChanged { get; } = new();

      public uint FirstEmptySlotIndex
      {
         get
         {
            var index = _equipments.IndexOf(t => !t);

            return index >= 0 ? (uint)index : (uint)_equipments.Length;
         }
      }

      public bool TryGetSlotIndex(Equipment equipment, out uint index)
      {
         for (index = 0; index < _equipments.Length; index++)
         {
            if (_equipments[index] == equipment)
            {
               return true;
            }
         }

         return false;
      }
   }
}