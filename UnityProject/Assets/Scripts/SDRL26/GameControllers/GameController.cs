using SDRL26.GameControllers.GameStates;
using SDRL26.Libraries;
using UnityEngine;

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

      private void Start() => ShowMainMenu();

      private static void ShowMainMenu() => GameState.Change(new MainMenuGameState());

      public void NewGame()
      {
         GameData.Reset(_gameDataLibrary.StarterCards);
         ChooseStarterBattler();
      }

      public void Quit() => Application.Quit();

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
         else if (GameDataLibrary.Instance.IsGameWon(GameData.Level))
         {
            GameState.Change(new MainMenuGameState());
         }
         else
         {
            PrepareBattle();
         }
      }

      private static void OnBattleLost() => ShowMainMenu();
   }
}