using System.Linq;
using SDRL26.Battles;
using SDRL26.Encounters;
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

         foreach (var battler in _gameDataLibrary.AllPlayerBattlers)
         {
            foreach (var posture in battler.Postures)
            {
               posture.RefreshActions();
            }
         }

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
      private static void ChangeToContinueBattleState() => GameState.Change(new ContinueBattleGameState(OnBattleWon, GameOverDefeat, PauseBattle));

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
         var playerEquipments = GameData.Inventory.AllEquipments.Union(GameData.PlayerTeam.Battlers.SelectMany(t => t.Equipments)).Where(t => t).ToArray();

         var encounterChoice = GameDataLibrary.Instance.RandomEncounterChoice(GameData.Level, playerEquipments);

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
         else if (encounter is Blacksmith blacksmith)
         {
            GameState.Change(new BlacksmithGameState(blacksmith, OnPacificEncounterDone));
         }
         else
         {
            Debug.LogError("Encounter is not handled");
         }
      }

      private static void OnPacificEncounterDone()
      {
         if (GameDataLibrary.Instance.IsLastLevelOrBeyond(GameData.Level))
         {
            GameData.NextLevel();
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

         if (GameDataLibrary.Instance.IsLastLevelOrBeyond(GameData.Level))
         {
            GameData.NextLevel();
            GameOverVictory();
         }
         else
         {
            GameData.EarnCurrentBattleBounty();
            GameState.Change(new BattleWonGameState(StartNextLevel));
         }
      }

      private static void GameOverVictory() => GameOver(true);
      private static void GameOverDefeat() => GameOver(false);
      private static void GameOver(bool win) => GameState.Change(new GameOverGameState(win, GoBackToMenu));
      private static void GoBackToMenu() => SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);

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