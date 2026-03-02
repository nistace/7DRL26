using SDRL26.Battles.Battlers;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Rendering.Cards.ChooseHeroes
{
   [RequireComponent(typeof(CardUi))]
   public class HeroCardUi : MonoBehaviour
   {
      [SerializeField] private CardUi _card;

      public Battler BattlerPrefab { get; private set; }

      public UnityEvent<HeroCardUi> OnClick { get; } = new();

      private void OnEnable() => _card.OnClick.AddListener(HandleClick);
      private void OnDisable() => _card.OnClick.RemoveListener(HandleClick);

      private void HandleClick() => OnClick.Invoke(this);

      public void SetUp(Battler battlerPrefab)
      {
         BattlerPrefab = battlerPrefab;
         _card.DisplayName = battlerPrefab.DisplayName;
         _card.Portrait = battlerPrefab.Portrait;
         _card.Description = $"> Targets {battlerPrefab.Target}";
      }
   }
}