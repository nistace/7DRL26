using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using SDRL26.Battles.Battlers;
using UnityEngine;
using Utilities;

namespace SDRL26.Rendering.Shared
{
   [CreateAssetMenu]
   public class BattlerDisplayData : ScriptableObject
   {
      [SerializeField] private string[] _actionTooltipPatternLines =
      {
         "<#59554B><i>Change posture in <b>[preparationTime]s</b>.</i></color>",
         "Select target: [target]",
         "<#59554B><i>Charge action in  <b>[chargeTime]s</b>.</i></color>",
         "Action :<br>[actions]",
         "<#59554B><i>Rest during  <b>[restTime]s</b>.</i></color>",
      };

      [SerializeField] private string _targetPattern = "[name][additional]";
      [SerializeField] private string _additionalTargetPattern = " and next <b>[number]</b>";

      [SerializeField] private SerializedDictionary<ActionTarget, string> _targetDisplayNames;
      [SerializeField] private string _defaultTargetDisplayName;

      public string GetActionTooltip(Battler battler, BattlerPosture posture)
      {
         var targets = GetTargetsTooltip(posture.Target, battler.AdditionalTargets + posture.AdditionalTargets);
         var actions = string.Join("<br>", posture.Actions.Select(t => $" - {t.DisplayString}"));

         return string.Join("<br>",
            _actionTooltipPatternLines.Select(t =>
               t
                  .Replace("[preparationTime]", posture.PreparationTime.ToStringOptionalDot())
                  .Replace("[chargeTime]", posture.ChargeActionTime.ToStringOptionalDot())
                  .Replace("[restTime]", posture.RestTime.ToStringOptionalDot())
                  .Replace("[target]", targets)
                  .Replace("[action]", actions)
            )
         );
      }

      public string GetTargetsTooltip(ActionTarget target, int additionalTargets)
      {
         return _targetPattern
            .Replace("[name]", _targetDisplayNames.GetValueOrDefault(target, _defaultTargetDisplayName))
            .Replace("[additional]", additionalTargets <= 0 ? string.Empty : _additionalTargetPattern.Replace("[number]", $"{additionalTargets}"));
      }
   }
}