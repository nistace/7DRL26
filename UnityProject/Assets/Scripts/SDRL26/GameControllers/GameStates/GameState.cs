using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public abstract class GameState
   {
      public static GameState CurrentState { get; private set; }
      public static UnityEvent<GameState> OnStateChanged { get; } = new();
      public abstract GameStateTypes Types { get; }

      public static void Change(GameState newState)
      {
         CurrentState?.EndState();
         CurrentState = newState;
         CurrentState?.StartState();

         OnStateChanged.Invoke(newState);
      }

      protected abstract void EndState();
      protected abstract void StartState();

      public bool Is(GameStateTypes states) => (Types & states) > 0;
   }
}