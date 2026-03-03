using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class MotivateBattleAction : BattleAction
   {
      [SerializeField] private float _progress = 1;
      [SerializeField] private string _displayName = "Motivate ([progress]s)";

      public override string DisplayString => _displayName.Replace("[progress]", $"{_progress:0.##}");

      public override void ApplyEffect(IActionPerformer actionDoer, IReadOnlyCollection<Battler> targets)
      {
         foreach (var target in targets)
         {
            target.ProgressCurrentPhaseLoadUpTime(_progress);
         }
      }
   }
}