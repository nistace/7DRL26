using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using SDRL26.Battles;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class ContinueBattleGameState : GameState
   {
      private Battle Battle { get; }
      private UnityAction OnWon { get; }
      private UnityAction OnLost { get; }
      private CancellationTokenSource CancellationTokenSource;
      public float BattleTime { get; private set; }

      public ContinueBattleGameState(Battle battle, UnityAction onWon, UnityAction onLost)
      {
         Battle = battle;
         OnWon = onWon;
         OnLost = onLost;
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
         while (!Battle.IsOver())
         {
            BattleTime += Time.deltaTime;
            Battle.Continue(Time.deltaTime);
            await UniTask.NextFrame();
         }

         if (Battle.PlayerTeam.Battlers.Any(t => t.Health.IsAlive))
         {
            OnWon?.Invoke();
         }
         else
         {
            OnLost?.Invoke();
         }
      }

      protected override void EndState() { }
   }
}