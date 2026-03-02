using System.Collections.Generic;
using SDRL26.Battles.Battlers;

namespace SDRL26.Battles.Actions
{
   public class ChangePostureBattleAction : BattleAction
   {
      public override string DisplayString => "Change Posture";

      public override int ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets)
      {
         actionDoer.SelectNextPosture();

         return 1;
      }
   }
}