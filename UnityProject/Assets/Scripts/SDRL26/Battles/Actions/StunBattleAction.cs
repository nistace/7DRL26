using SDRL26.Battles.Battlers;

namespace SDRL26.Battles.Actions
{
   public class StunBattleAction : BattleAction
   {
      public override RepeatingBehaviour Repetition => RepeatingBehaviour.Repeating;
      public override string DisplayString => "Stun";
      public override int Amount => 0;

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         target.ForceRest();
      }
   }
}