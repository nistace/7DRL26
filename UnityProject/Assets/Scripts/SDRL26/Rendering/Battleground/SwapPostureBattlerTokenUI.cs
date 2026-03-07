using SDRL26.GameControllers.GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Battleground
{
   public class SwapPostureBattlerTokenUI : MonoBehaviour
   {
      [SerializeField] private Button _button;
      [SerializeField] private GameStateTypes _duringTypes = GameStateTypes.PrepareBattle | GameStateTypes.PauseBattle;
      [SerializeField] private BattlerTokenUi _token;

      private void Start()
      {
         _button.onClick.AddListener(HandleButtonClicked);
         GameState.OnStateChanged.AddListener(HandleStateChanged);
         _button.gameObject.SetActive(GameState.CurrentState?.Is(_duringTypes) ?? false);
      }

      private void HandleStateChanged(GameState state) => _button.gameObject.SetActive(state.Is(_duringTypes));

      private void HandleButtonClicked()
      {
         if (GameState.CurrentState.Is(_duringTypes))
         {
            _token.Battler.SelectNextPosture();
         }
      }
   }
}