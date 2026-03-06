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

      public void Set(Sprite icon, int value, bool hasMoreEffects)
      {
         _icon.sprite = icon;
         _valueText.text = value.ToString();
         _valueText.enabled = true;
         _moreEffects.enabled = hasMoreEffects;
      }
   }
}