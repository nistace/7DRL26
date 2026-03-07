using SDRL26.GameControllers.GameStates;
using SDRL26.Rendering.Bounties;
using SDRL26.Tooltips;
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

         var give = blacksmithGameState.Blacksmith.GiveEquipment;
         _fromEquipmentUi.Icon = give.Icon;
         _fromEquipmentUi.Text = give.DisplayName;
         _fromEquipmentUi.Tooltip = new Tooltip($"Equipment: {give.DisplayName}", give.Description);

         var receive = blacksmithGameState.Blacksmith.ReceiveEquipmentPrefab;
         _toEquipmentUi.Icon = receive.Icon;
         _toEquipmentUi.Text = receive.DisplayName;
         _fromEquipmentUi.Tooltip = new Tooltip($"Equipment: {receive.DisplayName}", receive.Description);
      }
   }
}