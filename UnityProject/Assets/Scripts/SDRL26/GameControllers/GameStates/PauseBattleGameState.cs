using System.Collections.Generic;
using SDRL26.Battles;
using SDRL26.Battles.Battlers;
using SDRL26.Battles.Cards;
using SDRL26.Libraries;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class PauseBattleGameState : GameState
   {
      public Battle Battle => GameData.CurrentBattle;
      public PlayerAbilityCardDeck CardDeck => GameData.PlayerDeck;
      public UnityAction OnResumed { get; }
      public IReadOnlyList<AbilityCard> Options => CardDeck.HandCards;

      public PauseBattleGameState(UnityAction onResumed)
      {
         CardDeck.DrawHand(GameDataLibrary.Instance.PauseCardsCount);
         OnResumed = onResumed;
      }

      protected override void StartState() { }

      public void Validate(AbilityCard playCard)
      {
         playCard.Play(Battle);

         CardDeck.DiscardHand();

         OnResumed?.Invoke();
      }

      protected override void EndState() { }
   }
}