using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Equipments
{
   public abstract class EquipmentEffect : MonoBehaviour
   {
      public abstract void Apply(Battler battler);
      public abstract void Undo();
   }
}