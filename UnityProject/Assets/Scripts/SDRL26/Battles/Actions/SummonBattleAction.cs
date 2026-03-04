using System;
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
         Target = 1
      }

      [SerializeField] private Battler _battlerPrefab;
      [SerializeField] private TargetTeam _targetTeam = TargetTeam.ActionDoer;
      [SerializeField] private Position _position = Position.Last;
      [SerializeField] private string _displayName = "Summon [battler] [position]";
      [SerializeField] RepeatingBehaviour _repeatingBehaviour = RepeatingBehaviour.OnceAfterRepeating;

      public override RepeatingBehaviour Repetition => _repeatingBehaviour;
      public override string DisplayString => _displayName.Replace("[battler]", _battlerPrefab.DisplayName).Replace("[position]", $"{_position}");

      public override void ApplyEffect(BattleActionData data, Battler target)
      {
         var summonTarget = _targetTeam switch
         {
            TargetTeam.ActionDoer when data.ActionDoer is Battler battlerActionDoer => battlerActionDoer,
            TargetTeam.Target => target,
            _ => throw new ArgumentOutOfRangeException()
         };

         if (target == null)
         {
            return;
         }

         var newInstance = summonTarget.Team.AddBattlerPrefabInstance(_battlerPrefab,
            _position switch
            {
               Position.First => 0,
               Position.Last => summonTarget.Team.Battlers.Count,
               Position.Before => summonTarget.Team.IndexOf(summonTarget),
               Position.After => summonTarget.Team.IndexOf(summonTarget) + 1,
               _ => throw new ArgumentOutOfRangeException()
            },
            false
         );

         newInstance.Team = summonTarget.Team;
         newInstance.OtherTeam = summonTarget.OtherTeam;
         newInstance.IsSummoned = true;

         summonTarget.Team.NotifyChanged();
      }
   }
}