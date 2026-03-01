using System.Collections.Generic;
using System.Linq;
using SDRL26.Battlers.Actions;
using UnityEngine;

namespace SDRL26.Actions
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