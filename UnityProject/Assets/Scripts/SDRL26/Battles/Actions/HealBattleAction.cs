using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class HealBattleAction : BattleAction
   {
      [SerializeField] private int _heal;

      public override string DebugString => $"Heal ({_heal})";

      public override int ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets)
      {
         return targets.Sum(target => target.Heal(_heal));
      }
   }
}