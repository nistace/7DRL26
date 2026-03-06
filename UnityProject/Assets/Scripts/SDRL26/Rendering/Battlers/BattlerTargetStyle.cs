using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Rendering.Battlers
{
   [CreateAssetMenu]
   public class BattlerTargetStyle : ScriptableObject
   {
      [SerializeField] private SerializedDictionary<ActionTarget, Sprite> _iconPerTarget;
      [SerializeField] private Sprite _defaultIcon;
      [SerializeField] private SerializedDictionary<ActionTarget, string> _textPerTarget;
      [SerializeField] private string _defaultText;

      public Sprite Sprite(ActionTarget target) => _iconPerTarget.GetValueOrDefault(target, _defaultIcon);
      public string Text(ActionTarget target) => _textPerTarget.GetValueOrDefault(target, _defaultText);
   }
}