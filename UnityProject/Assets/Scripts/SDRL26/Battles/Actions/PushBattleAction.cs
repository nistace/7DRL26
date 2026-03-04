using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class PushBattleAction : BattleAction
   {
      [SerializeField] private int _steps = 1;
      [SerializeField] private string _displayName = "Push ([steps])";

      public override RepeatingBehaviour Repetition { get; }
      public override string DisplayString => _displayName.Replace("[steps]", _steps.ToString());

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         target.Team.MoveBattlerByDelta(target, _steps);
      }
   }
}