using SDRL26.Battles.Equipments;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class BattleWonGameState : GameState
   {
      public override GameStateTypes Types => GameStateTypes.BattleWon;

      public Bounty Bounty { get; }
      public UnityAction OnDone { get; }

      public BattleWonGameState(UnityAction onDone)
      {
         Bounty = GameData.CurrentBattle.Bounty;

         foreach (var battler in GameData.CurrentBattle.OpponentTeam.Battlers)
         {
            Object.Destroy(battler.gameObject);
         }

         OnDone = onDone;
      }

      public void Terminate() => OnDone?.Invoke();

      protected override void EndState() { }

      protected override void StartState() { }
   }
}