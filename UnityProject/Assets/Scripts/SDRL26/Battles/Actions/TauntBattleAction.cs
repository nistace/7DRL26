using SDRL26.Battles.Battlers;

namespace SDRL26.Battles.Actions
{
   public class TauntBattleAction : BattleAction
   {
      public override RepeatingBehaviour Repetition => RepeatingBehaviour.Repeating;
      public override string DisplayString => "Taunt";
      public override int Amount => 0;

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         if (data.ActionDoer is not Battler battler)
         {
            return;
         }

         target.SetTarget(battler);
      }
   }
}