using System.Collections.Generic;
using SDRL26.Battles.Battlers;

namespace SDRL26.Battles.Actions
{
   public class TauntBattleAction : BattleAction
   {
      public override string DisplayString => "Taunt";

      public override int ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets)
      {
         foreach (var target in targets)
         {
            target.SetTargets(new[] { actionDoer });
         }

         return targets.Count;
      }
   }
}