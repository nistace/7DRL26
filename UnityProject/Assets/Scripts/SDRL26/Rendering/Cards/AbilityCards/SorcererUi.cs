using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Cards;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Cards.AbilityCards
{
   public class SorcererUi : CardPickerUi<SorcererGameState, (int index, AbilityCard ability)>
   {
      [SerializeField] private AbilityCardUi _abilityCardPrefab;

      protected override IReadOnlyList<(int index, AbilityCard ability)> GetOptions(SorcererGameState state) => state.Sorcerer.Abilities.Select((t, i) => (i, t)).ToArray();

      protected override Transform SpawnCard((int index, AbilityCard ability) option)
      {
         var newCard = Instantiate(_abilityCardPrefab, CardMovementHandler.Spawn);
         newCard.SetUp(option.ability);
         newCard.OnClick.AddListener(HandleCardClicked);

         return newCard.transform;
      }

      private static void HandleCardClicked(AbilityCardUi card)
      {
         if (GameState.CurrentState is not SorcererGameState sorcererState)
         {
            return;
         }

         sorcererState.Choose(card.Ability);
      }
   }
}