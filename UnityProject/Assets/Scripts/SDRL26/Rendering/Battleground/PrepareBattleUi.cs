using SDRL26.GameControllers.GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Battleground
{
   public class PrepareBattleUi : MonoBehaviour
   {
      [SerializeField] private Button _startBattleButton;

      private void Start()
      {
         GameState.OnStateChanged.AddListener(HandleStateChanged);
      }

      private void OnEnable()
      {
         _startBattleButton.onClick.AddListener(HandleStartBattleClicked);
      }

      private void OnDisable()
      {
         _startBattleButton.onClick.RemoveListener(HandleStartBattleClicked);
      }

      private static void HandleStartBattleClicked()
      {
         if (GameState.CurrentState is PrepareBattleGameState prepareBattleGameState)
         {
            prepareBattleGameState.EndPreparation();
         }
      }

      private void HandleStateChanged(GameState newState) => _startBattleButton.gameObject.SetActive(newState is PrepareBattleGameState);
   }
}