using SDRL26.GameControllers.GameStates;
using SDRL26.Libraries;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDRL26.GameControllers
{
   public class GameController : MonoBehaviour
   {
      [SerializeField] private GameDataLibrary _gameDataLibrary;

      private void Awake()
      {
         GameDataLibrary.Instance = _gameDataLibrary;
         GameData.Reset(_gameDataLibrary.StarterCards);
      }

      private void Start() => ChooseStarterBattler();
      private void ChooseStarterBattler() => GameState.Change(new ChooseHeroState(GameDataLibrary.Instance.RandomStartBattlers, ContinueAfterChoosingStarterBattler));

      private void PrepareBattle() => GameState.Change(new PrepareBattleGameState(GameDataLibrary.Instance.RandomBattleSetup(GameData.Level), ChangeToContinueBattleState));
      private void ChangeToContinueBattleState() => GameState.Change(new ContinueBattleGameState(OnBattleWon, OnBattleLost, PauseBattle));
      private void PauseBattle() => GameState.Change(new PauseBattleGameState(ChangeToContinueBattleState));

      private void ContinueAfterChoosingStarterBattler()
      {
         if (GameData.PlayerTeam.Battlers.Count < GameDataLibrary.Instance.BattlersToPickOnStart)
         {
            ChooseStarterBattler();
         }
         else
         {
            PrepareBattle();
         }
      }

      private void OnBattleWon()
      {
         GameData.PlayerTeam.ResetBattlers();
         GameData.NextLevel();

         if (GameDataLibrary.Instance.HasToChooseHero(GameData.Level))
         {
            GameState.Change(new ChooseHeroState(GameDataLibrary.Instance.RandomBattlers, PrepareBattle));
         }
         else
         {
            PrepareBattle();
         }
      }

      private static void OnBattleLost()
      {
         Debug.Log("GameOver");
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
   }
}