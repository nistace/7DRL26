using System.Linq;
using SDRL26.Battles.Battlers;
using SDRL26.Rendering.Battlers;
using SDRL26.Rendering.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

namespace SDRL26.Rendering.Cards.ChooseHeroes
{
   [RequireComponent(typeof(CardUi))]
   public class HeroCardUi : MonoBehaviour
   {
      [SerializeField] private CardUi _card;
      [SerializeField] private Button _swapPostureButton;
      [SerializeField] private BattlerTargetUi _target;
      [SerializeField] private TMP_Text _healthText;
      [SerializeField] private TMP_Text _preparationTime;
      [SerializeField] private TMP_Text _actionTime;
      [SerializeField] private TMP_Text _restTime;
      [SerializeField] private ActionIcon _actionIcon;

      public Battler BattlerPrefab { get; private set; }
      private int PostureIndex { get; set; }

      public UnityEvent<HeroCardUi> OnClick { get; } = new();

      private void Start() => _swapPostureButton.onClick.AddListener(HandleSwapPostureButtonClicked);
      private void OnDestroy() => _swapPostureButton.onClick.RemoveListener(HandleSwapPostureButtonClicked);

      private void HandleSwapPostureButtonClicked()
      {
         if (!BattlerPrefab)
         {
            return;
         }

         PostureIndex = Mathf.Clamp((PostureIndex + 1) % BattlerPrefab.Postures.Count, 0, BattlerPrefab.Postures.Count);
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
         _target.Set(posture.Target, posture.AdditionalTargets);
         _healthText.text = $"{BattlerPrefab.Health.DefaultMaxHealth}";
         _preparationTime.text = posture.PreparationTime.ToStringOptionalDot();
         _actionTime.text = posture.ChargeActionTime.ToStringOptionalDot();
         _restTime.text = posture.PreparationTime.ToStringOptionalDot();
         _actionIcon.Set(posture.MainActionIcon, posture.MainActionAmount, posture.HasSideEffects);

         _card.Description = $"Targets {posture.Target}<br>"
            + $"[{posture.ChargeActionTime:0.##}s] Action<br>{string.Join("<br>", posture.Actions.Select(t => $" - {t.DisplayString}"))}<br>"
            + $"[{posture.RestTime:0.##}s] Rest";
      }

      public void SetUp(Battler battlerPrefab)
      {
         BattlerPrefab = battlerPrefab;
         PostureIndex = 0;
         RefreshInfo();
         _swapPostureButton.gameObject.SetActive(battlerPrefab.Postures.Count > 1);
      }
   }
}