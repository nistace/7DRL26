using System.Collections.Generic;
using System.Linq;
using SDRL26.Encounters;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Cards.Encounters
{
   public class ChooseEncounterUi : CardPickerUi<ChooseEncounterGameState, (int index, IEncounter encounter)>
   {
      [SerializeField] private EncounterCardUi _encounterCardUi;

      protected override IReadOnlyList<(int index, IEncounter encounter)> GetOptions(ChooseEncounterGameState state) => state.Choice.Options.Select((t, i) => (i, t)).ToArray();

      protected override Transform SpawnCard((int index, IEncounter encounter) option)
      {
         var newCard = Instantiate(_encounterCardUi, CardMovementHandler.Spawn);
         newCard.SetUp(option.index, option.encounter);
         newCard.OnClick.AddListener(HandleCardClicked);

         return newCard.transform;
      }

      private static void HandleCardClicked(EncounterCardUi card)
      {
         if (GameState.CurrentState is not ChooseEncounterGameState encounterState)
         {
            return;
         }

         encounterState.Choose(card.Index);
      }
   }
}