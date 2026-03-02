using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class ShieldBattleAction : BattleAction
   {
      [SerializeField] private int _shield;
      [SerializeField] private string _displayName = "Shield ([shield])";

      public override string DisplayString => _displayName.Replace("[shield]", $"{_shield}");

      public override void ApplyEffect(Battler actionDoer, IReadOnlyCollection<Battler> targets)
      {
         foreach (var target in targets)
         {
            target.Shield(_shield);
         }
      }
   }
}