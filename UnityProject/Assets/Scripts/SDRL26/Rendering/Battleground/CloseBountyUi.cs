using SDRL26.GameControllers.GameStates;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SDRL26.Rendering.Battleground
{
   public class CloseBountyUi : MonoBehaviour
   {
      [SerializeField] private Button _button;

      private void OnEnable() => _button.onClick.AddListener(HandleStartBattleClicked);
      private void OnDisable() => _button.onClick.RemoveListener(HandleStartBattleClicked);

      private static void HandleStartBattleClicked()
      {
         if (GameState.CurrentState is BattleWonGameState battleWonGameState)
         {
            battleWonGameState.Terminate();
         }
      }
   }
}