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

      public override int ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets)
      {
         return targets.Sum(target => target.Damage(_damage));
      }
   }
}