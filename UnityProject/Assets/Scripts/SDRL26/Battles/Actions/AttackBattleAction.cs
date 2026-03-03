using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class AttackBattleAction : BattleAction
   {
      [SerializeField] private int _damage;
      [SerializeField] private string _displayName = "Attack ([damage])";

      public override string DisplayString => _displayName.Replace("[damage]", $"{_damage}");

      public override void ApplyEffect(IActionPerformer actionDoer, IReadOnlyCollection<Battler> targets)
      {
         foreach (var target in targets)
         {
            target.Damage(_damage);
         }
      }
   }
}