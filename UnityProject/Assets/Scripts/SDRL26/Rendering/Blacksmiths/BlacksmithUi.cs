using System;
using SDRL26.GameControllers.GameStates;
using SDRL26.Rendering.Bounties;
using UnityEngine;

namespace SDRL26.Rendering.Blacksmiths
{
   public class BlacksmithUi : MonoBehaviour
   {
      [SerializeField] private BountyLootUi _fromEquipmentUi;
      [SerializeField] private BountyLootUi _toEquipmentUi;

      private void Start()
      {
         Refresh();
         GameState.OnStateChanged.AddListener(HandleStateChanged);
      }

      private void HandleStateChanged(GameState arg0) => Refresh();

      private void Refresh()
      {
         if (GameState.CurrentState is not BlacksmithGameState blacksmithGameState)
         {
            return;
         }

         _fromEquipmentUi.Icon = blacksmithGameState.Blacksmith.GiveEquipment.Icon;
         _fromEquipmentUi.Text = blacksmithGameState.Blacksmith.GiveEquipment.DisplayName;
         _toEquipmentUi.Icon = blacksmithGameState.Blacksmith.ReceiveEquipmentPrefab.Icon;
         _toEquipmentUi.Text = blacksmithGameState.Blacksmith.ReceiveEquipmentPrefab.DisplayName;
      }
   }
}