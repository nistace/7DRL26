using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class BloodlustBattleAction : BattleAction
   {
      [SerializeField] private string _displayName = "Bloodlust [ratio]%";
      [SerializeField] private float _damageDealtRatio = .25f;

      public override RepeatingBehaviour Repetition => RepeatingBehaviour.OnceAfterRepeating;
      public override string DisplayString => _displayName.Replace("[ratio]", $"{_damageDealtRatio * 100:0}");
      public override int Amount => 0;

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         if (data.ActionDoer is not Battler battler)
         {
            return;
         }

         battler.Heal(Mathf.CeilToInt(data[BattleActionData.MeasurableData.DamageDealt] * _damageDealtRatio), false);
      }
   }
}