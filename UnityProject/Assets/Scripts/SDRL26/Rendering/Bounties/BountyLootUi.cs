using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Bounties
{
   public class BountyLootUi : MonoBehaviour
   {
      [SerializeField] private Image _icon;
      [SerializeField] private TMP_Text _text;

      public Sprite Icon
      {
         get => _icon.sprite;
         set
         {
            _icon.enabled = value;
            _icon.sprite = value;
         }
      }

      public string Text
      {
         get => _text.text;
         set => _text.text = value;
      }
   }
}