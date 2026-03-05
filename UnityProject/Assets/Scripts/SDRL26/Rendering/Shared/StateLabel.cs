using SDRL26.GameControllers.GameStates;
using TMPro;
using UnityEngine;

namespace SDRL26.Rendering.Shared
{
   public class StateLabel : MonoBehaviour
   {
      [SerializeField] private TMP_Text _text;
      [SerializeField] private StateDataList<string> _labels = new("Continue");

      private void Reset()
      {
         _text = GetComponentInChildren<TMP_Text>();
      }

      private void Start()
      {
         Refresh();
         GameState.OnStateChanged.AddListener(HandleStateChanged);
      }

      private void OnDestroy() => GameState.OnStateChanged.RemoveListener(HandleStateChanged);
      private void HandleStateChanged(GameState arg0) => Refresh();
      private void Refresh() => _text.text = _labels.First(GameState.CurrentState?.Types ?? GameStateTypes.MainMenu);
   }
}