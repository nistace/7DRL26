using System.Collections.Generic;
using UnityEngine;

namespace SDRL26.Battlers.Actions
{
    public abstract class BattleAction : MonoBehaviour
    {
        public abstract string DebugString { get; }
        public abstract int ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets);
    }
}