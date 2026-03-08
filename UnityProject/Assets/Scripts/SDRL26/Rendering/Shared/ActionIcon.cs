using SDRL26.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Shared
{
   public class ActionIcon : MonoBehaviour
   {
      [SerializeField] private Image _icon;
      [SerializeField] private Image _moreEffects;
      [SerializeField] private TMP_Text _valueText;
      [SerializeField] private TooltipHolder _tooltipHolder;

      public void Set(Sprite icon, int value, bool hasMoreEffects, Tooltip tooltip)
      {
         _icon.sprite = icon;
         _valueText.text = value.ToString();
         _valueText.enabled = value > 0;
         _moreEffects.enabled = hasMoreEffects;
         _tooltipHolder.Tooltip = tooltip;
      }
   }
}