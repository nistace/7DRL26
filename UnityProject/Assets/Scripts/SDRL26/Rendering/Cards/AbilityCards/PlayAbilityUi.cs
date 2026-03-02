using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Cards;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Cards.AbilityCards
{
   public class PlayAbilityUi : CardPickerUi<PauseBattleGameState, AbilityCard>
   {
      [SerializeField] private AbilityCardUi _abilityCardPrefab;

      protected override IReadOnlyList<AbilityCard> GetOptions(PauseBattleGameState state) => state.Options;

      protected override Transform SpawnCard(AbilityCard arg)
      {
         var newCard = Instantiate(_abilityCardPrefab, CardMovementHandler.Spawn);
         newCard.SetUp(arg);
         newCard.OnClick.AddListener(HandleCardClicked);

         return newCard.transform;
      }

      private static void HandleCardClicked(AbilityCardUi card)
      {
         if (GameState.CurrentState is PauseBattleGameState pauseState)
         {
            pauseState.Validate(card.Ability);
         }
      }
   }
}