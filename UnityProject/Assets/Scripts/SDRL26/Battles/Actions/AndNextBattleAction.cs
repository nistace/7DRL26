using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class AndNextBattleAction : BattleAction
   {
      [SerializeField] private bool _allowSameTarget;
      [SerializeField] private bool _loop;

      public override string DisplayString => "And Next Target";

      public override void ApplyEffect(IActionPerformer actionDoer, IReadOnlyCollection<Battler> targets)
      {
         var actions = GetComponents<BattleAction>();

         var nextTargets = targets
            .Select(t => (exists: t.Team.TryGetNextBattler(t, out var next, _loop, _allowSameTarget), next))
            .Where(t => t.exists)
            .Select(t => t.next)
            .ToArray();

         foreach (var action in actions)
         {
            if (action is AndNextBattleAction) continue;

            action.ApplyEffect(actionDoer, nextTargets);
         }
      }
   }
}