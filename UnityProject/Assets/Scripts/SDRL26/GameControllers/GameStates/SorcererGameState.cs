using SDRL26.Battles.Cards;
using SDRL26.Encounters;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class SorcererGameState : GameState
   {
      public Sorcerer Sorcerer { get; set; }
      private UnityAction OnSorcererEnded { get; }
      public override GameStateTypes Types => GameStateTypes.Sorcerer;

      public SorcererGameState(Sorcerer sorcerer, UnityAction onSorcererEnded)
      {
         Sorcerer = sorcerer;
         OnSorcererEnded = onSorcererEnded;
      }

      public bool Choose(AbilityCard ability)
      {
         if (!Sorcerer.Has(ability)) return false;

         GameData.PlayerDeck.AddCard(Object.Instantiate(ability));
         OnSorcererEnded.Invoke();

         return true;
      }

      protected override void EndState() { }
      protected override void StartState() { }
   }
}