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

      private void Start() => GameState.Change(new ChooseHeroState(GameDataLibrary.Instance.RandomStartBattlers, PrepareBattle));
      private void PrepareBattle() => GameState.Change(new PrepareBattleGameState(GameDataLibrary.Instance.RandomBattleSetup(GameData.Level), ChangeToContinueBattleState));
      private void ChangeToContinueBattleState() => GameState.Change(new ContinueBattleGameState(OnBattleWon, OnBattleLost, PauseBattle));
      private void PauseBattle() => GameState.Change(new PauseBattleGameState(ChangeToContinueBattleState));

      private void OnBattleWon()
      {
         Debug.Log("Battle won!");
         GameData.NextLevel();
         GameData.PlayerTeam.ResetBattlers();
         GameState.Change(new ChooseHeroState(GameDataLibrary.Instance.RandomStartBattlers, PrepareBattle));
      }

      private static void OnBattleLost()
      {
         Debug.Log("GameOver");
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
   }
}