using SDRL26.Battles.Battlers;
using SDRL26.GameControllers;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Battleground
{
   public class BattleGroundUi : MonoBehaviour
   {
      [SerializeField] private BattlerTeamUi _playerTeam;
      [SerializeField] private BattlerTeamUi _otherTeam;

      private void Start()
      {
         GameState.OnStateChanged.AddListener(HandleGameStateChanged);

         _playerTeam.Setup(GameData.PlayerTeam);
         _playerTeam.SetVisible(true);
         _otherTeam.SetVisible(false);
      }

      private void HandleGameStateChanged(GameState newState)
      {
         if (newState is PrepareBattleGameState prepareBattleGameState)
         {
            _otherTeam.Setup(prepareBattleGameState.Battle.OpponentTeam);
            _otherTeam.SetVisible(true);
            SetTokensDisplayMode(BattlerTokenDisplayMode.Prepare);
         }
         else if (newState is ContinueBattleGameState or PauseBattleGameState)
         {
            _otherTeam.SetVisible(true);
            SetTokensDisplayMode(BattlerTokenDisplayMode.Battle);
         }
         else
         {
            _otherTeam.SetVisible(false);
            SetTokensDisplayMode(BattlerTokenDisplayMode.Default);
         }
      }

      private void SetTokensDisplayMode(BattlerTokenDisplayMode mode)
      {
         _playerTeam.SetTokensDisplayMode(mode);
         _otherTeam.SetTokensDisplayMode(mode);
      }

      public BattlerTokenUi GetToken(Battler battler) => (battler.Team == GameData.PlayerTeam ? _playerTeam : _otherTeam).GetToken(battler);
   }
}