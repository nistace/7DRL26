using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class WakeUpBattleAction : BattleAction
   {
      [SerializeField] private string _displayName = "Wake Up";

      public override RepeatingBehaviour Repetition => RepeatingBehaviour.Repeating;
      public override string DisplayString => _displayName;
      public override int Amount => 0;

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         target.EndRest(true);
      }
   }
}