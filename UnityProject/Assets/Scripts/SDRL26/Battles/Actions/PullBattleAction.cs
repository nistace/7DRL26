using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class PullBattleAction : BattleAction
   {
      [SerializeField] private int _steps = 1;
      [SerializeField] private string _displayName = "Pull ([steps])";

      public override string DisplayString => _displayName.Replace("[steps]", _steps.ToString());

      public override void ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets)
      {
         foreach (var target in targets)
         {
            target.Team.Move(target, -_steps);
         }
      }
   }
}