using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using SDRL26.Battles;
using SDRL26.Libraries;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class ContinueBattleGameState : GameState
   {
      public static Battle Battle => GameData.CurrentBattle;
      private float InterruptOnTimeElapsed { get; }
      private float BattlePhaseStartTime { get; }
      public float PhaseProgress => (Battle.BattleTime - BattlePhaseStartTime) / (InterruptOnTimeElapsed - BattlePhaseStartTime);
      private UnityAction OnWon { get; }
      private UnityAction OnLost { get; }
      private UnityAction OnTimeElapsed { get; }
      private CancellationTokenSource CancellationTokenSource;

      public ContinueBattleGameState(UnityAction onWon, UnityAction onLost, UnityAction onTimeElapsed)
      {
         OnWon = onWon;
         OnLost = onLost;
         BattlePhaseStartTime = Battle.BattleTime;
         InterruptOnTimeElapsed = Battle.GetNextPauseTime(GameDataLibrary.Instance.TimeBetweenInterruptions);
         OnTimeElapsed = onTimeElapsed;
      }

      protected override void StartState()
      {
         CancellationTokenSource?.Cancel();
         CancellationTokenSource?.Dispose();
         CancellationTokenSource = new CancellationTokenSource();

         _ = ContinueBattleAsync();
      }

      private async UniTask ContinueBattleAsync()
      {
         await UniTask.NextFrame();

         while (!Battle.IsOver() && PhaseProgress < 1)
         {
            Battle.Continue(Time.deltaTime);
            await UniTask.NextFrame();
         }

         if (Battle.PlayerTeam.Battlers.All(t => t.Health.IsDead))
         {
            OnLost?.Invoke();
         }
         else if (Battle.OpponentTeam.Battlers.All(t => t.Health.IsDead))
         {
            OnWon?.Invoke();
         }
         else
         {
            OnTimeElapsed?.Invoke();
         }
      }

      protected override void EndState() { }
   }
}