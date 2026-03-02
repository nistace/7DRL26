using SDRL26.Battles;
using SDRL26.Libraries;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class PrepareBattleGameState : GameState
   {
      public Battle Battle { get; }
      public UnityAction<Battle> OnPrepared { get; }

      public PrepareBattleGameState(BattleSetup battleSetup, UnityAction<Battle> onPrepared)
      {
         Battle = new Battle(GameData.PlayerTeam, battleSetup.InstantiateOpponentTeam());
         Battle.Prepare(GameDataLibrary.Instance.MaxBattlerAdditionalPreparationTime);
         OnPrepared = onPrepared;
      }

      protected override void StartState() { }

      public void EndPreparation() => OnPrepared?.Invoke(Battle);

      protected override void EndState() { }
   }
}