using SDRL26.Encounters;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Rendering.Cards.Encounters
{
   [RequireComponent(typeof(CardUi))]
   public class EncounterCardUi : MonoBehaviour
   {
      [SerializeField] private CardUi _card;

      public int Index { get; private set; }

      public UnityEvent<EncounterCardUi> OnClick { get; } = new();

      private void OnEnable() => _card.OnClick.AddListener(HandleClick);
      private void OnDisable() => _card.OnClick.RemoveListener(HandleClick);
      private void HandleClick() => OnClick.Invoke(this);

      public void SetUp(int index, IEncounter encounter)
      {
         Index = index;
         _card.Portrait = encounter.Portrait;
         _card.Description = encounter.Description;
         _card.DisplayName = encounter.DisplayName;
      }
   }
}