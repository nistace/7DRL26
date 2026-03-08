using System;
using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Actions;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Cards
{
   public class AbilityCard : MonoBehaviour, IActionPerformer
   {
      [SerializeField] private string _displayName;
      [SerializeField] private string _description;
      [SerializeField] private Sprite _icon;
      [SerializeField] private CardTargets _targets;
      [SerializeField] private ActionTarget _actionTarget;
      [SerializeField] private BattleAction _valueAction;
      [SerializeField] private bool _hasMoreEffectsThanOnIcon;

      public CardTargets Targets => _targets;
      public ActionTarget ActionTarget => _actionTarget;
      public string DisplayName => _displayName;
      public int ActionAmount => _valueAction ? _valueAction.Amount : 0;
      public bool HasMoreEffectsThanOnIcon => _hasMoreEffectsThanOnIcon;
      public Sprite Icon => _icon;
      public string Description => _description;
      public IReadOnlyList<BattleAction> Actions => GetComponents<BattleAction>();

      public void Play(Battle battle)
      {
         foreach (var target in EvaluateTargets(battle))
         {
            ActionResolver.Resolve(GetComponents<BattleAction>(), this, target);
         }
      }

      private IReadOnlyCollection<Battler> EvaluateTargets(Battle battle) => _targets switch
      {
         CardTargets.All => battle.PlayerTeam.Battlers.Union(battle.OpponentTeam.Battlers).ToArray(),
         CardTargets.AllAllies => battle.PlayerTeam.Battlers,
         CardTargets.AllEnemies => battle.OpponentTeam.Battlers,
         CardTargets.ActionTarget => new[] { battle.PlayerTeam.Battlers[0].EvaluateTarget(_actionTarget) },
         CardTargets.LastOfBothTeams => new[] { battle.PlayerTeam.Battlers.Last(), battle.OpponentTeam.Battlers.Last() },
         CardTargets.FirstOfBothTeams => new[] { battle.PlayerTeam.Battlers.First(), battle.OpponentTeam.Battlers.First() },
         _ => throw new ArgumentOutOfRangeException()
      };
   }
}