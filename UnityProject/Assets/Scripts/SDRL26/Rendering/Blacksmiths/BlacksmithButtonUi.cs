using SDRL26.GameControllers.GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Blacksmiths
{
   public class BlacksmithButtonUi : MonoBehaviour
   {
      [SerializeField] private Button _button;
      [SerializeField] private bool _acceptTransaction;

      private void Start() => _button.onClick.AddListener(HandleButtonClick);
      private void OnDestroy() => _button.onClick.RemoveListener(HandleButtonClick);

      private void HandleButtonClick()
      {
         if (GameState.CurrentState is not BlacksmithGameState blacksmithGameState) return;

         blacksmithGameState.End(_acceptTransaction);
      }
   }
}