using System;
using System.Collections.Generic;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Actions
{
   public class SummonBattleAction : BattleAction
   {
      private enum Position
      {
         First = 0,
         Last = 1,
         Before = 2,
         After = 3
      }

      private enum TargetTeam
      {
         ActionDoer = 0,
         AllTargets = 1
      }

      [SerializeField] private Battler _battlerPrefab;
      [SerializeField] private TargetTeam _targetTeam = TargetTeam.ActionDoer;
      [SerializeField] private Position _position = Position.Last;
      [SerializeField] private string _displayName = "Summon [battler] [position]";

      public override string DisplayString => _displayName.Replace("[battler]", _battlerPrefab.DisplayName).Replace("[position]", $"{_position}");

      public override void ApplyEffect(IActionPerformer actionDoer, IReadOnlyCollection<Battler> targets)
      {
         var teamsChanged = new HashSet<BattlerTeam>();

         var destinations = _targetTeam switch
         {
            TargetTeam.ActionDoer when actionDoer is Battler battlerActionDoer => new[] { battlerActionDoer },
            TargetTeam.AllTargets => targets,
            _ => throw new ArgumentOutOfRangeException()
         };

         foreach (var target in destinations)
         {
            teamsChanged.Add(target.Team);

            var newInstance = target.Team.AddBattlerPrefabInstance(_battlerPrefab,
               _position switch
               {
                  Position.First => 0,
                  Position.Last => target.Team.Battlers.Count,
                  Position.Before => target.Team.IndexOf(target),
                  Position.After => target.Team.IndexOf(target) + 1,
                  _ => throw new ArgumentOutOfRangeException()
               },
               false
            );

            newInstance.Team = target.Team;
            newInstance.OtherTeam = target.OtherTeam;
         }

         foreach (var team in teamsChanged)
         {
            team.NotifyChanged();
         }
      }
   }
}