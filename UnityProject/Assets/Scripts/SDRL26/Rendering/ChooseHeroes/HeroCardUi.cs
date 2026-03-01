using SDRL26.Battles.Battlers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDRL26.Rendering.ChooseHeroes
{
   public class HeroCardUi : MonoBehaviour, IPointerClickHandler
   {
      [SerializeField] private Button _button;
      [SerializeField] private TMP_Text _displayNameText;
      [SerializeField] private Image _portraitImage;
      [SerializeField] private TMP_Text _descriptionText;

      public Battler BattlerPrefab { get; private set; }

      public UnityEvent<HeroCardUi> OnClick { get; } = new();

      public void SetUp(Battler battlerPrefab)
      {
         BattlerPrefab = battlerPrefab;
         _displayNameText.text = battlerPrefab.DisplayName;
         _portraitImage.sprite = battlerPrefab.Portrait;
         _descriptionText.text = $"> Targets {battlerPrefab.Target}";
      }

      public void OnPointerClick(PointerEventData eventData) => OnClick.Invoke(this);
   }
}