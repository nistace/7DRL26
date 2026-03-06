using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public abstract class BattleAction : MonoBehaviour
   {
      public enum RepeatingBehaviour
      {
         Repeating = 0,
         RepeatingInReverse = 1,
         OnceAfterRepeating = 3,
      }

      public abstract RepeatingBehaviour Repetition { get; }
      public abstract string DisplayString { get; }
      public abstract int Amount { get; }

      public abstract void ApplyEffect(BattleActionData data, Battler target);
   }
}