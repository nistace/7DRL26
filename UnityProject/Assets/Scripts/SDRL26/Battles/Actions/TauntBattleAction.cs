using System.Collections.Generic;
using SDRL26.Battles.Battlers;

namespace SDRL26.Battles.Actions
{
   public class TauntBattleAction : BattleAction
   {
      public override string DisplayString => "Taunt";

      public override void ApplyEffect(IActionPerformer actionDoer, IReadOnlyCollection<Battler> targets)
      {
         if (actionDoer is not Battler battler)
         {
            return;
         }

         foreach (var target in targets)
         {
            target.SetTargets(new[] { battler });
         }
      }
   }
}