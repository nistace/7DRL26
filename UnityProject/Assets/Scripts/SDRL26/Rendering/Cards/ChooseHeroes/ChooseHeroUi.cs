using System.Collections.Generic;
using SDRL26.Battles.Battlers;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Cards.ChooseHeroes
{
   public class ChooseHeroUi : CardPickerUi<ChooseHeroState, Battler>
   {
      [SerializeField] private HeroCardUi _heroCardPrefab;

      protected override IReadOnlyList<Battler> GetOptions(ChooseHeroState state) => state.Options;

      protected override Transform SpawnCard(Battler battler)
      {
         var newCard = Instantiate(_heroCardPrefab, CardMovementHandler.Spawn);
         newCard.SetUp(battler);
         newCard.OnClick.AddListener(HandleCardClicked);

         return newCard.transform;
      }

      private static void HandleCardClicked(HeroCardUi card)
      {
         if (GameState.CurrentState is ChooseHeroState chooseHeroState)
         {
            chooseHeroState.Choose(card.BattlerPrefab);
         }
      }
   }
}