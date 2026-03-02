using System.Collections.Generic;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class WakeUpBattleAction : BattleAction
   {
      [SerializeField] private string _displayName = "Wake Up";

      public override string DisplayString => _displayName;

      public override void ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets)
      {
         foreach (var target in targets)
         {
            if (target.CurrentPhase is Battler.Phase.Rest or Battler.Phase.Prepare)
            {
               target.EndRest(true);
            }
         }
      }
   }
}