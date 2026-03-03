using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Shared
{
   public class StateVisibleUi : MonoBehaviour
   {
      [SerializeField] private GameStateTypes _activeForTypes;

      private void Start()
      {
         RefreshVisible();
         GameState.OnStateChanged.AddListener(HandleGameStateChanged);
      }

      private void HandleGameStateChanged(GameState newBattle) => RefreshVisible();
      private void OnDestroy() => GameState.OnStateChanged.RemoveListener(HandleGameStateChanged);
      private void RefreshVisible() => gameObject.SetActive(GameState.CurrentState?.Is(_activeForTypes) ?? false);
   }
}