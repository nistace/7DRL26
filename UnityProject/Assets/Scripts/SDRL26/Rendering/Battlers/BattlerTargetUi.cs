using SDRL26.Battles.Battlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Battlers
{
   public class BattlerTargetUi : MonoBehaviour
   {
      [SerializeField] private Image _icon;
      [SerializeField] private TMP_Text _text;
      [SerializeField] private TMP_Text _additionalTargetsText;
      [SerializeField] private BattlerTargetStyle _style;

      public void Set(ActionTarget target, int additionalTargets)
      {
         _icon.sprite = _style.Sprite(target);
         _text.text = _style.Text(target);
         _additionalTargetsText.enabled = additionalTargets > 0;
         _additionalTargetsText.text = $"+{additionalTargets}";
      }
   }
}