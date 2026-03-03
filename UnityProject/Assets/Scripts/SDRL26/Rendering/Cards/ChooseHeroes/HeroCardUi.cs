using System.Linq;
using SDRL26.Battles.Battlers;
using SDRL26.Rendering.Battleground.Postures;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace SDRL26.Rendering.Cards.ChooseHeroes
{
   [RequireComponent(typeof(CardUi))]
   public class HeroCardUi : MonoBehaviour
   {
      [SerializeField] private CardUi _card;
      [SerializeField] private BattlerPostureButton[] _postureButtons;

      public Battler BattlerPrefab { get; private set; }
      private int PostureIndex { get; set; }

      public UnityEvent<HeroCardUi> OnClick { get; } = new();

      private void Start()
      {
         foreach (var button in _postureButtons)
         {
            button.OnPostureSelected.AddListener(HandlePostureSelected);
         }
      }

      private void OnDestroy()
      {
         foreach (var button in _postureButtons)
         {
            if (button) button.OnPostureSelected.RemoveListener(HandlePostureSelected);
         }
      }

      private void HandlePostureSelected(BattlerPosture posture)
      {
         if (BattlerPrefab == null) return;

         PostureIndex = Mathf.Clamp(BattlerPrefab.Postures.IndexOf(posture), 0, _postureButtons.Length);
         RefreshInfo();
      }

      private void OnEnable() => _card.OnClick.AddListener(HandleClick);
      private void OnDisable() => _card.OnClick.RemoveListener(HandleClick);

      private void HandleClick() => OnClick.Invoke(this);

      private void RefreshInfo()
      {
         var posture = BattlerPrefab.Postures[PostureIndex];
         _card.DisplayName = BattlerPrefab.DisplayName;
         _card.Portrait = posture.GetPortrait(BattlerPosture.Portrait.Rest);

         _card.Description = $"Targets {posture.Target}<br>"
            + $"[{posture.ChargeActionTime:0.##}s] Action<br>{string.Join("<br>", posture.Actions.Select(t => $" - {t.DisplayString}"))}<br>"
            + $"[{posture.RestTime:0.##}s] Rest";
      }

      public void SetUp(Battler battlerPrefab)
      {
         BattlerPrefab = battlerPrefab;
         PostureIndex = 0;
         RefreshInfo();

         for (var postureButtonIndex = 0; postureButtonIndex < _postureButtons.Length; ++postureButtonIndex)
         {
            var posture = postureButtonIndex < BattlerPrefab.Postures.Count ? BattlerPrefab.Postures[postureButtonIndex] : null;
            var button = _postureButtons[postureButtonIndex];
            button.gameObject.SetActive(posture != null);

            if (posture != null)
            {
               button.SetUp(posture);
            }
         }
      }
   }
}