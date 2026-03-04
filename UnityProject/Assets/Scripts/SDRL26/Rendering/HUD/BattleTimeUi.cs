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
      [SerializeField] private string _runningTextPattern = "Time: [time]";
      [SerializeField] private string _pauseTextPattern = "Paused";

      private void Update()
      {
         switch (GameState.CurrentState)
         {
            case PauseBattleGameState:
               _text.text = _pauseTextPattern.Replace("[time]", $"{ContinueBattleGameState.Battle.BattleTime:0.0}");
               _progressImage.fillAmount = 0;

               break;
            case ContinueBattleGameState battleState:
               _text.text = _runningTextPattern.Replace("[time]", $"{ContinueBattleGameState.Battle.BattleTime:0.0}");
               _progressImage.fillAmount = battleState.PhaseProgress;

               break;
         }
      }
   }
}