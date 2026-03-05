using SDRL26.Battles.Equipments;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class BattleWonGameState : GameState
   {
      public override GameStateTypes Types => GameStateTypes.BattleWon;

      public Bounty Bounty { get; }
      public UnityAction OnDone { get; }

      public BattleWonGameState(Bounty bounty, UnityAction onDone)
      {
         Bounty = bounty;
         OnDone = onDone;
      }

      public void Terminate() => OnDone?.Invoke();

      protected override void EndState() { }

      protected override void StartState() { }
   }
}