using SDRL26.GameControllers;
using TMPro;
using UnityEngine;

namespace SDRL26.Rendering.HUD
{
   public class GoldUi : MonoBehaviour
   {
      [SerializeField] private TMP_Text _text;

      private void OnEnable()
      {
         RefreshText();
         GameData.Inventory.OnGoldChanged.AddListener(HandleLevelChanged);
      }

      private void OnDisable() => GameData.OnLevelChanged.RemoveListener(HandleLevelChanged);
      private void HandleLevelChanged(int gold) => RefreshText();
      private void RefreshText() => _text.text = $"{(GameData.Inventory?.Gold ?? 0)}";
   }
}