using System;
using SDRL26.GameControllers.GameStates;
using TMPro;
using UnityEngine;

namespace SDRL26.Rendering.HUD
{
   public class GameOverUi : MonoBehaviour
   {
      [SerializeField] private TMP_Text _text;
      [SerializeField] private string _victoryText;
      [SerializeField] private string _defeatText;

      private void Start()
      {
         Refresh();
         GameState.OnStateChanged.AddListener(HandleGameStateChanged);
      }

      private void OnDestroy()
      {
         GameState.OnStateChanged.RemoveListener(HandleGameStateChanged);
      }

      private void HandleGameStateChanged(GameState newState) => Refresh();

      private void Refresh()
      {
         if (GameState.CurrentState is GameOverGameState gameOverState)
         {
            _text.text = gameOverState.Won ? _victoryText : _defeatText;
         }
      }
   }
}