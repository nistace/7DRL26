using System.Collections.Generic;
using SDRL26.Battles.Battlers;

namespace SDRL26.Battles.Actions
{
   public static class ActionResolver
   {
      public static void Resolve(IReadOnlyList<BattleAction> actions, IActionPerformer performer, Battler initialTarget, int additionalTargets = 0)
      {
         BattleActionData data = new(performer);
         var allTargets = EvaluateAllTargets(initialTarget, additionalTargets);

         foreach (var action in actions)
         {
            if (action.Repetition == BattleAction.RepeatingBehaviour.Repeating)
            {
               foreach (var target in allTargets)
               {
                  action.ApplyEffect(data, target);
               }
            }
            else if (action.Repetition == BattleAction.RepeatingBehaviour.RepeatingInReverse)
            {
               for (var index = allTargets.Length - 1; index >= 0; index--)
               {
                  action.ApplyEffect(data, allTargets[index]);
               }
            }
         }

         foreach (var action in actions)
         {
            if (action.Repetition == BattleAction.RepeatingBehaviour.OnceAfterRepeating)
            {
               action.ApplyEffect(data, initialTarget);
            }
         }
      }

      public static Battler[] EvaluateAllTargets(Battler initialTarget, int additionalTargets = 0)
      {
         if (additionalTargets == 0) return new[] { initialTarget };

         var allTargets = new List<Battler> { initialTarget };

         var lastTargetAdded = initialTarget;

         for (var i = 0; lastTargetAdded && i < additionalTargets; i++)
         {
            if (initialTarget.Team.TryGetNextBattler(lastTargetAdded, out var nextTarget, true, false))
            {
               if (allTargets.Contains(nextTarget))
               {
                  lastTargetAdded = null;
               }
               else
               {
                  allTargets.Add(nextTarget);
                  lastTargetAdded = nextTarget;
               }
            }
         }

         return allTargets.ToArray();
      }
   }
}