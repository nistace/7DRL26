using System.Collections.Generic;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class ReviveBattleAction : BattleAction
   {
      [SerializeField, Range(0, 1)] private float _healthRatio = .2f;
      [SerializeField] private string _displayName = "Revive ([healthRatio]%)";

      public override string DisplayString => _displayName.Replace("[healthRatio]", $"{Mathf.RoundToInt(_healthRatio * 100)}");

      public override void ApplyEffect(IActionPerformer actionDoer, IReadOnlyCollection<Battler> targets)
      {
         foreach (var target in targets)
         {
            target.Revive(_healthRatio);
         }
      }
   }
}