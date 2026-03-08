using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using SDRL26.Battles.Battlers;
using SDRL26.Battles.Cards;
using UnityEngine;

namespace SDRL26.Rendering.Cards.AbilityCards
{
   [CreateAssetMenu]
   public class AbilityDisplayData : ScriptableObject
   {
      [SerializeField] private string[] _actionTooltipPatternLines =
      {
         "Targets: [target]",
         "Effects:<br>[actions]",
      };

      [SerializeField] private SerializedDictionary<CardTargets, string> _cardTargetDisplayNames;
      [SerializeField] private SerializedDictionary<ActionTarget, string> _targetDisplayNames;
      [SerializeField] private string _defaultTargetDisplayName;

      public string GetActionTooltip(AbilityCard card)
      {
         var targets = GetTargetsTooltip(card.Targets, card.ActionTarget);
         var actions = string.Join("<br>", card.Actions.Select(t => $" - {t.DisplayString}"));

         return string.Join("<br>", _actionTooltipPatternLines.Select(t => t.Replace("[target]", targets).Replace("[action]", actions)));
      }

      private string GetTargetsTooltip(CardTargets cardTarget, ActionTarget actionTarget)
      {
         if (cardTarget == CardTargets.ActionTarget)
         {
            return _targetDisplayNames.GetValueOrDefault(actionTarget, _defaultTargetDisplayName);
         }

         return _cardTargetDisplayNames.GetValueOrDefault(cardTarget, _defaultTargetDisplayName);
      }
   }
}