using System;
using SDRL26.Battles.Equipments;
using UnityEngine;

namespace SDRL26.Encounters
{
   [Serializable]
   public class Blacksmith : IEncounter
   {
      public Equipment GiveEquipment { get; }
      public Equipment ReceiveEquipmentPrefab { get; }
      public Sprite Portrait { get; }
      public string DisplayName { get; }
      public string Description { get; }

      public Blacksmith(Equipment give_equipment, Equipment receive_equipment_prefab, Sprite portrait, string displayName, string description)
      {
         GiveEquipment = give_equipment;
         ReceiveEquipmentPrefab = receive_equipment_prefab;
         Portrait = portrait;
         DisplayName = displayName;
         Description = description;
      }
   }
}