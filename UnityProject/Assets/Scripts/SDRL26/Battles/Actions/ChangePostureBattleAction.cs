using System.Collections.Generic;
using SDRL26.Battles.Battlers;

namespace SDRL26.Battles.Actions
{
   public class ChangePostureBattleAction : BattleAction
   {
      public override string DisplayString => "Change Posture";

      public override void ApplyEffect(IActionPerformer actionDoer, IReadOnlyCollection<Battler> targets)
      {
         if (actionDoer is not Battler battler)
         {
            return;
         }
         
         battler.SelectNextPosture();
      }
   }
}