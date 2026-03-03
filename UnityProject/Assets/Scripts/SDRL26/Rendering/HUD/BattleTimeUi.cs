using SDRL26.GameControllers.GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.HUD
{
   public class BattleTimeUi : MonoBehaviour
   {
      [SerializeField] private TMP_Text _text;
      [SerializeField] private Image _progressImage;

      private void Start()
      {
         GameState.OnStateChanged.AddListener(HandleGameStateChanged);
         RefreshWithCurrentState();
      }

      private void OnDestroy()
      {
         GameState.OnStateChanged.RemoveListener(HandleGameStateChanged);
      }

      private void HandleGameStateChanged(GameState newState) => RefreshWithCurrentState();

      private void RefreshWithCurrentState() => gameObject.SetActive(GameState.CurrentState is PauseBattleGameState or ContinueBattleGameState);

      private void Update()
      {
         switch (GameState.CurrentState)
         {
            case PauseBattleGameState:
               _text.text = "Paused";
               _progressImage.fillAmount = 0;

               break;
            case ContinueBattleGameState battleState:
               _text.text = $"Battle time: {ContinueBattleGameState.Battle.BattleTime:0.0}";
               _progressImage.fillAmount = battleState.PhaseProgress;

               break;
         }
      }
   }
}