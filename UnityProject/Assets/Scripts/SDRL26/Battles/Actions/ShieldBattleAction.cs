using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class ShieldBattleAction : BattleAction
   {
      [SerializeField] private int _shield;
      [SerializeField] private string _displayName = "Shield ([shield])";

      public override RepeatingBehaviour Repetition => RepeatingBehaviour.Repeating;
      public override string DisplayString => _displayName.Replace("[shield]", $"{_shield}");
      public override int Amount => _shield;

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         target.Shield(_shield);
      }
   }
}