using SDRL26.GameControllers;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Menus
{
   public class NewGameButton : MonoBehaviour
   {
      [SerializeField] private Button _button;

      private void Start() => _button.onClick.AddListener(HandleClick);
      private void OnDestroy() => _button.onClick.RemoveListener(HandleClick);
      private static void HandleClick() => GameController.NewGame();
   }
}