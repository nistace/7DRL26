using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class GameOverGameState : GameState
   {
      public override GameStateTypes Types => GameStateTypes.GameOver;

      public bool Won { get; }
      private UnityAction OnDone { get; }

      public GameOverGameState(bool won, UnityAction onDone)
      {
         Won = won;
         OnDone = onDone;
      }

      public void Terminate() => OnDone?.Invoke();

      protected override void EndState() { }
      protected override void StartState() { }
   }
}