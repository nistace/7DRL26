using SDRL26.Battles;
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
         GameData.Reset();
      }

      private void Start()
      {
         GameState.Change(new ChooseHeroState(GameDataLibrary.Instance.RandomStartBattlers, HandleHeroChosen));
      }

      private static void HandleHeroChosen() => GameState.Change(new PrepareBattleGameState(GameDataLibrary.Instance.RandomBattleSetup(GameData.Level), HandleBattlePrepared));
      private static void HandleBattlePrepared(Battle battle) => GameState.Change(new ContinueBattleGameState(battle, OnBattleWon, OnBattleLost));

      private static void OnBattleWon()
      {
         Debug.Log("Battle won!");
         GameData.NextLevel();
         GameState.Change(new ChooseHeroState(GameDataLibrary.Instance.RandomStartBattlers, HandleHeroChosen));
      }

      private static void OnBattleLost()
      {
         Debug.Log("GameOver");
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
   }
}