using SDRL26.GameControllers;
using SDRL26.Libraries;
using TMPro;
using UnityEngine;

namespace SDRL26.Rendering.HUD
{
   public class LevelUi : MonoBehaviour
   {
      [SerializeField] private TMP_Text _levelText;
      [SerializeField] private string _pattern = "Level [level]/[max]";

      private void OnEnable()
      {
         RefreshText();
         GameData.OnLevelChanged.AddListener(HandleLevelChanged);
      }

      private void OnDisable() => GameData.OnLevelChanged.RemoveListener(HandleLevelChanged);
      private void HandleLevelChanged(int arg0) => RefreshText();

      private void RefreshText()
      {
         _levelText.text = GameDataLibrary.Instance
            ? _pattern.Replace("[level]", (GameData.Level + 1).ToString()).Replace("[max]", $"{GameDataLibrary.Instance.Levels}")
            : string.Empty;
      }
   }
}