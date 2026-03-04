using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Equipments
{
   public class HealthEquipmentEffect : EquipmentEffect
   {
      [SerializeField] public int _additionalHealth = 3;

      private Health _modifiedHealth;

      public override void Apply(Battler battler)
      {
         if (_modifiedHealth != null) Undo();

         _modifiedHealth = battler.Health;
         _modifiedHealth.ChangeMax(_additionalHealth);
      }

      public override void Undo()
      {
         if (_modifiedHealth == null) return;

         _modifiedHealth.ChangeMax(-_additionalHealth);
         _modifiedHealth = null;
      }
   }
}