using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Equipments
{
   public class AddTargetEquipmentEffect : EquipmentEffect
   {
      [SerializeField] public int _additionalTargets = 1;

      private Battler _modifiedBattler;

      public override void Apply(Battler battler)
      {
         if (_modifiedBattler != null) Undo();

         _modifiedBattler = battler;
         _modifiedBattler.AdditionalTargets += _additionalTargets;
      }

      public override void Undo()
      {
         if (_modifiedBattler == null) return;

         _modifiedBattler.AdditionalTargets -= _additionalTargets;
         _modifiedBattler = null;
      }
   }
}