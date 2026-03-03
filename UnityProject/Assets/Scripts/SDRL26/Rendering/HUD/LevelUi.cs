using SDRL26.GameControllers;
using TMPro;
using UnityEngine;

namespace SDRL26.Rendering.HUD
{
   public class LevelUi : MonoBehaviour
   {
      [SerializeField] private TMP_Text _levelText;

      private void Start() => RefreshText();
      private void OnEnable() => GameData.OnLevelChanged.AddListener(HandleLevelChanged);
      private void OnDisable() => GameData.OnLevelChanged.RemoveListener(HandleLevelChanged);
      private void HandleLevelChanged(int arg0) => RefreshText();
      private void RefreshText() => _levelText.text = $"Level {(GameData.Level + 1)}";
   }
}