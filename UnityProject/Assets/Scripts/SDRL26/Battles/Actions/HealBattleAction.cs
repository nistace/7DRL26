using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class HealBattleAction : BattleAction
   {
      [SerializeField] private int _heal;
      [SerializeField] private bool _canRevive;
      [SerializeField] private string _displayName = "Heal ([heal])";

      public override string DisplayString => _displayName.Replace("[heal]", $"{_heal}");

      public override void ApplyEffect(IActionPerformer actionDoer, IReadOnlyCollection<Battler> targets)
      {
         foreach (var target in targets)
         {
            target.Heal(_heal, _canRevive);
         }
      }
   }
}