using SDRL26.Encounters;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class ChooseEncounterGameState : GameState
   {
      public EncounterChoice Choice { get; }
      public UnityAction<IEncounter> OnEncounterPicked { get; }
      public override GameStateTypes Types => GameStateTypes.ChooseEncounter;

      public ChooseEncounterGameState(EncounterChoice choice, UnityAction<IEncounter> onEncounterPicked)
      {
         Choice = choice;
         OnEncounterPicked = onEncounterPicked;
      }

      public void Choose(int choiceIndex)
      {
         var encounter = Choice.Options[choiceIndex];

         OnEncounterPicked.Invoke(encounter);
      }

      protected override void EndState() { }
      protected override void StartState() { }
   }
}