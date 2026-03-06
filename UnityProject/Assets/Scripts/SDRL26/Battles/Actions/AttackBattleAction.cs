using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class AttackBattleAction : BattleAction
   {
      [SerializeField] private int _damage;
      [SerializeField] private string _displayName = "Attack ([damage])";

      public override RepeatingBehaviour Repetition => RepeatingBehaviour.Repeating;
      public override string DisplayString => _displayName.Replace("[damage]", $"{_damage}");
      public override int Amount => _damage;

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         data.AddMeasurable(BattleActionData.MeasurableData.DamageDealt, target.Damage(_damage));
      }
   }
}