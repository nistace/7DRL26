using SDRL26.GameControllers.GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Shared
{
   public class EndStateButtonUi : MonoBehaviour
   {
      [SerializeField] private Button _button;

      private void OnEnable() => _button.onClick.AddListener(HandleStartBattleClicked);
      private void OnDisable() => _button.onClick.RemoveListener(HandleStartBattleClicked);

      private static void HandleStartBattleClicked()
      {
         switch (GameState.CurrentState)
         {
            case BattleWonGameState battleWonGameState:
               battleWonGameState.Terminate();

               break;
            case MerchantGameState merchantGameState:
               merchantGameState.End();

               break;
            case PrepareBattleGameState prepareBattleGameState:
               prepareBattleGameState.EndPreparation();

               break;
         }
      }
   }
}