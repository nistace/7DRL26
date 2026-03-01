using System.Collections.Generic;
using SDRL26.Battlers.Actions;

namespace SDRL26.Actions
{
    public class TauntBattleAction : BattleAction
    {
        public override string DebugString => $"Taunt";

        public override int ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets)
        {
            foreach (var target in targets)
            {
                target.SetTargets(new[] { actionDoer });
            }

            return targets.Count;
        }
    }
}