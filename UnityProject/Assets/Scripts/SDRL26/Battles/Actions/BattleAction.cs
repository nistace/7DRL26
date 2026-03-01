using System.Collections.Generic;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public abstract class BattleAction : MonoBehaviour
   {
      public abstract string DisplayString { get; }
      public abstract int ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets);
   }
}