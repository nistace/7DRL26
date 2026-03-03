using SDRL26.Battles.Cards;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Rendering.Cards.AbilityCards
{
   [RequireComponent(typeof(CardUi))]
   public class AbilityCardUi : MonoBehaviour
   {
      [SerializeField] private CardUi _card;

      public AbilityCard Ability { get; private set; }

      public UnityEvent<AbilityCardUi> OnClick { get; } = new();
      public CardUi Card { get; set; }

      private void OnEnable() => _card.OnClick.AddListener(HandleClick);
      private void OnDisable() => _card.OnClick.RemoveListener(HandleClick);
      private void HandleClick() => OnClick.Invoke(this);

      public void SetUp(AbilityCard ability)
      {
         Ability = ability;
         _card.DisplayName = ability.DisplayName;
         _card.Portrait = ability.Portrait;
         _card.Description = ability.Description;
      }
   }
}