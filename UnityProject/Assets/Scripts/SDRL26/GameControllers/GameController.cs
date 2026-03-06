using SDRL26.Battles;
using SDRL26.Encounters;
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
      private static void PrepareBattle(BattleSetup battleSetup) => GameState.Change(new PrepareBattleGameState(battleSetup, ChangeToContinueBattleState));
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
            ChooseEncounter();
         }
      }

      private static void ChooseEncounter()
      {
         var encounterChoice = GameDataLibrary.Instance.RandomEncounterChoice(GameData.Level);

         if (encounterChoice.Options.Count > 1)
         {
            GameState.Change(new ChooseEncounterGameState(encounterChoice, OnEncounterPicked));
         }
         else
         {
            OnEncounterPicked(encounterChoice.Options[0]);
         }
      }

      private static void OnEncounterPicked(IEncounter encounter)
      {
         if (encounter is BattleSetup battleSetup)
         {
            PrepareBattle(battleSetup);
         }
         else if (encounter is Merchant merchant)
         {
            GameState.Change(new MerchantGameState(merchant, OnPacificEncounterDone));
         }
         else if (encounter is Sorcerer sorcerer)
         {
            GameState.Change(new SorcererGameState(sorcerer, OnPacificEncounterDone));
         }
         else
         {
            Debug.LogError("Encounter is not handled");
         }
      }

      private static void OnPacificEncounterDone()
      {
         GameData.NextLevel();

         if (GameDataLibrary.Instance.IsGameWon(GameData.Level))
         {
            GameOverVictory();
         }
         else
         {
            StartNextLevel();
         }
      }

      private static void OnBattleWon()
      {
         GameData.PlayerTeam.ResetAfterBattle();

         if (GameDataLibrary.Instance.IsGameWon(GameData.Level + 1))
         {
            GameData.NextLevel();
            GameOverVictory();
         }
         else
         {
            GameData.EarnCurrentBattleBounty();
            GameState.Change(new BattleWonGameState(GameData.CurrentBattle.Bounty, StartNextLevel));
         }
      }

      private static void GameOverVictory()
      {
         GameState.Change(new MainMenuGameState());
      }

      private static void OnBattleLost() => ShowMainMenu();

      private static void StartNextLevel()
      {
         GameData.NextLevel();

         if (GameDataLibrary.Instance.HasToChooseHero(GameData.Level))
         {
            GameState.Change(new ChooseHeroState(GameDataLibrary.Instance.RandomBattlers, ChooseEncounter));
         }
         else
         {
            ChooseEncounter();
         }
      }
   }
}