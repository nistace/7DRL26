using SDRL26.Battles;
using SDRL26.Libraries;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class PrepareBattleGameState : GameState
   {
      public Battle Battle { get; }
      public UnityAction OnPrepared { get; }
      public override GameStateTypes Types => GameStateTypes.PrepareBattle;

      public PrepareBattleGameState(BattleSetup battleSetup, UnityAction onPrepared)
      {
         Battle = new Battle(GameData.PlayerTeam, battleSetup.InstantiateOpponentTeam());
         Battle.Prepare(GameDataLibrary.Instance.MaxBattlerAdditionalPreparationTime);
         OnPrepared = onPrepared;
      }

      protected override void StartState() { }

      public void EndPreparation()
      {
         GameData.CurrentBattle = Battle;
         OnPrepared?.Invoke();
      }

      protected override void EndState() { }
   }
}