using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Equipments;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Cards.Merchants
{
   public class MerchantUi : CardPickerUi<MerchantGameState, (int index, Equipment equipment)>
   {
      [SerializeField] private EquipmentCardUi _equipmentCardPrefab;

      protected override IReadOnlyList<(int index, Equipment equipment)> GetOptions(MerchantGameState state) => state.Merchant.EquipmentPrefabs.Select((t, i) => (i, t)).ToArray();

      protected override Transform SpawnCard((int index, Equipment equipment) option)
      {
         var newCard = Instantiate(_equipmentCardPrefab, CardMovementHandler.Spawn);
         newCard.SetUp(option.index, option.equipment, false);
         newCard.OnClick.AddListener(HandleCardClicked);

         return newCard.transform;
      }

      private static void HandleCardClicked(EquipmentCardUi card)
      {
         if (GameState.CurrentState is not MerchantGameState merchantState)
         {
            return;
         }

         if (!merchantState.Purchase(card.Index))
         {
            return;
         }

         CardMovementHandler.HideCard(card.transform);
         card.SetSold(true);
      }
   }
}