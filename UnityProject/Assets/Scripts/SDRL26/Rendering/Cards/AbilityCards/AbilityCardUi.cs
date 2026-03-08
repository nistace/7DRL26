using SDRL26.Battles.Cards;
using SDRL26.Rendering.Shared;
using SDRL26.Tooltips;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Rendering.Cards.AbilityCards
{
   [RequireComponent(typeof(CardUi))]
   public class AbilityCardUi : MonoBehaviour
   {
      [SerializeField] private AbilityDisplayData _displayData;
      [SerializeField] private CardUi _card;
      [SerializeField] private ActionIcon _actionIcon;

      public AbilityCard Ability { get; private set; }

      public UnityEvent<AbilityCardUi> OnClick { get; } = new();

      private void OnEnable() => _card.OnClick.AddListener(HandleClick);
      private void OnDisable() => _card.OnClick.RemoveListener(HandleClick);
      private void HandleClick() => OnClick.Invoke(this);

      public void SetUp(AbilityCard ability)
      {
         Ability = ability;
         _card.DisplayName = ability.DisplayName;
         _card.Portrait = ability.Icon;
         _card.Description = ability.Description;

         _actionIcon.Set(ability.Icon, ability.ActionAmount, ability.HasMoreEffectsThanOnIcon, new Tooltip(string.Empty, _displayData.GetActionTooltip(ability)));
      }
   }
}