using System.Collections.Generic;
using SDRL26.Battles.Battlers;

namespace SDRL26.Battles.Actions
{
   public class StunBattleAction : BattleAction
   {
      public override string DisplayString => "Stun";

      public override void ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets)
      {
         foreach (var target in targets)
         {
            target.ForceRest();
         }
      }
   }
}