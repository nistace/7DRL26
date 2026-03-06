using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class PullBattleAction : BattleAction
   {
      [SerializeField] private int _steps = 1;
      [SerializeField] private string _displayName = "Pull ([steps])";

      public override RepeatingBehaviour Repetition => RepeatingBehaviour.Repeating;
      public override string DisplayString => _displayName.Replace("[steps]", _steps.ToString());
      public override int Amount => _steps;

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         target.Team.MoveBattlerByDelta(target, -_steps);
      }
   }
}