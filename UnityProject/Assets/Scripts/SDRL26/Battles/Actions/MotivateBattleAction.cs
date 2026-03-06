using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class MotivateBattleAction : BattleAction
   {
      [SerializeField] private float _progress = 1;
      [SerializeField] private string _displayName = "Motivate ([progress]s)";

      public override RepeatingBehaviour Repetition => RepeatingBehaviour.Repeating;
      public override string DisplayString => _displayName.Replace("[progress]", $"{_progress:0.##}");
      public override int Amount => 0;

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         target.ProgressCurrentPhaseLoadUpTime(_progress);
      }
   }
}