using SDRL26.Battles.Battlers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDRL26.Rendering.Battleground.Postures
{
   public class BattlerPostureButton : MonoBehaviour
   {
      [SerializeField] private Button _button;
      [SerializeField] private Image _backgroundImage;
      [SerializeField] private Image _foregroundImage;

      private BattlerPosture _posture;
      public UnityEvent<BattlerPosture> OnPostureSelected { get; } = new();

      private void OnEnable()
      {
         _button.onClick.AddListener(HandleButtonClicked);
      }

      private void HandleButtonClicked() => OnPostureSelected.Invoke(_posture);

      public void SetUp(BattlerPosture posture)
      {
         _posture = posture;
         _foregroundImage.sprite = _posture.Icon;
      }
   }
}