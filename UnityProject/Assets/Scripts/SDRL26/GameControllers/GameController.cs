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

      public static void NewGame()
      {
         GameData.Reset(GameDataLibrary.Instance.StarterCards);
         ChooseStarterBattler();
      }

      public static void Quit() => Application.Quit();
      private static void ChooseStarterBattler() => GameState.Change(new ChooseHeroState(GameDataLibrary.Instance.RandomStartBattlers, ContinueAfterChoosingStarterBattler));
      private static void PrepareBattle() => GameState.Change(new PrepareBattleGameState(GameDataLibrary.Instance.RandomBattleSetup(GameData.Level), ChangeToContinueBattleState));
      private static void ChangeToContinueBattleState() => GameState.Change(new ContinueBattleGameState(OnBattleWon, OnBattleLost, PauseBattle));
      private static void PauseBattle() => GameState.Change(new PauseBattleGameState(ChangeToContinueBattleState));

      private static void ContinueAfterChoosingStarterBattler()
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

      private static void OnBattleWon()
      {
         GameData.PlayerTeam.ResetAfterBattle();

         if (GameDataLibrary.Instance.IsGameWon(GameData.Level))
         {
            GameState.Change(new MainMenuGameState());
         }
         else
         {
            GameData.EarnCurrentBattleBounty();
            GameState.Change(new BattleWonGameState(GameData.CurrentBattle.Bounty, OnBountyCollected));
         }
      }

      private static void OnBattleLost() => ShowMainMenu();

      private static void OnBountyCollected()
      {
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
   }
}