using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class ReviveBattleAction : BattleAction
   {
      [SerializeField, Range(0, 1)] private float _healthRatio = .2f;
      [SerializeField] private string _displayName = "Revive ([healthRatio]%)";

      public override RepeatingBehaviour Repetition => RepeatingBehaviour.Repeating;
      public override string DisplayString => _displayName.Replace("[healthRatio]", $"{Mathf.RoundToInt(_healthRatio * 100)}");
      public override int Amount => Mathf.RoundToInt(_healthRatio * 100);

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         target.Revive(_healthRatio);
      }
   }
}