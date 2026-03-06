using SDRL26.Battles.Battlers;

namespace SDRL26.Battles.Actions
{
   public class ChangePostureBattleAction : BattleAction
   {
      public override RepeatingBehaviour Repetition => RepeatingBehaviour.OnceAfterRepeating;
      public override string DisplayString => "Change Posture";
      public override int Amount => 0;

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         if (data.ActionDoer is not Battler battler)
         {
            return;
         }

         battler.SelectNextPosture();
      }
   }
}