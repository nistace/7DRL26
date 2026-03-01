using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class AttackBattleAction : BattleAction
   {
      [SerializeField] private int _damage;

      public override string DebugString => $"Damage ({_damage})";

      public override int ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets)
      {
         return targets.Sum(target => target.Damage(_damage));
      }
   }
}