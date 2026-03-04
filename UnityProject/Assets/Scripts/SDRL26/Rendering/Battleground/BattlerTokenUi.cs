using System;
using System.Linq;
using SDRL26.Battles.Battlers;
using SDRL26.Rendering.Battleground.HealthBars;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDRL26.Rendering.Battleground
{
   public class BattlerTokenUi : MonoBehaviour
   {
      [SerializeField] private Transform _linkAnchorOrigin;
      [SerializeField] private Transform _linkAnchorDestination;
      [SerializeField] private CanvasGroup _canvasGroup;
      [SerializeField] private Image _portrait;
      [SerializeField] private HealthBarUi _healthBar;
      [SerializeField] private TMP_Text[] _actionsTexts;
      [SerializeField] private Image _fillImage;
      [SerializeField] private BattlerTokenStyle _style;

      public BattlerTokenDisplayMode DisplayMode { get; set; }
      public Battler Battler { get; private set; }
      public Transform LinkAnchorOrigin => _linkAnchorOrigin;
      public Transform LinkAnchorDestination => _linkAnchorDestination;

      public UnityEvent OnBattlerChanged { get; } = new();

      public void Setup(Battler battler)
      {
         if (Battler) Battler.OnPostureChanged.RemoveListener(HandleBattlerPostureChanged);

         Battler = battler;
         RefreshBattlerInfo();
         _healthBar.Setup(battler.Health);

         Battler.OnPostureChanged.AddListener(HandleBattlerPostureChanged);

         OnBattlerChanged.Invoke();
      }

      private void HandleBattlerPostureChanged(BattlerPosture newPosture) => RefreshBattlerInfo();

      private void RefreshBattlerInfo()
      {
         _portrait.sprite = Battler.GetCurrentPortrait();
         _actionsTexts[0].text = $"> Targets {Battler.TargetChoice}";
         _actionsTexts[1].text = $"> [{Battler.Posture.ChargeActionTime:0.0}s] {string.Join(", ", Battler.Posture.Actions.Select(t => t.DisplayString))}";
         _actionsTexts[2].text = $"> [{Battler.Posture.RestTime:0.0}s] Rest";
      }

      private void Update()
      {
         _portrait.sprite = Battler.GetCurrentPortrait();

         if (Battler.Health.IsDead)
         {
            _canvasGroup.alpha = _style.DeadOpacity;
            _fillImage.fillAmount = 0;
            _fillImage.color = Color.clear;
            _actionsTexts[0].color = _style.InactiveActionColor;
            _actionsTexts[1].color = _style.InactiveActionColor;
            _actionsTexts[2].color = _style.InactiveActionColor;

            return;
         }

         switch (DisplayMode)
         {
            case BattlerTokenDisplayMode.Battle when Battler.CurrentPhase is Battler.Phase.Action:
               _canvasGroup.alpha = _style.DefaultOpacity;
               _fillImage.fillAmount = Battler.CurrentLoadRatio;
               _fillImage.color = _style.FillActionColor;
               _actionsTexts[0].color = _style.InactiveActionColor;
               _actionsTexts[1].color = _style.DefaultActionColor;
               _actionsTexts[2].color = _style.InactiveActionColor;

               break;
            case BattlerTokenDisplayMode.Battle:
               _canvasGroup.alpha = _style.RestOpacity;
               _fillImage.fillAmount = 1 - Battler.CurrentLoadRatio;
               _fillImage.color = _style.FillRestColor;
               _actionsTexts[0].color = _style.InactiveActionColor;
               _actionsTexts[1].color = _style.InactiveActionColor;
               _actionsTexts[2].color = _style.DefaultActionColor;

               break;
            case BattlerTokenDisplayMode.Prepare:
               _canvasGroup.alpha = _style.DefaultOpacity;
               _fillImage.fillAmount = 1 - Battler.CurrentLoadRatio;
               _fillImage.color = _style.FillRestColor;
               _actionsTexts[0].color = _style.DefaultActionColor;
               _actionsTexts[1].color = _style.DefaultActionColor;
               _actionsTexts[2].color = _style.DefaultActionColor;

               break;
            case BattlerTokenDisplayMode.Default:
               _canvasGroup.alpha = 1;
               _fillImage.fillAmount = 0;
               _fillImage.color = Color.clear;
               _actionsTexts[0].color = _style.DefaultActionColor;
               _actionsTexts[1].color = _style.DefaultActionColor;
               _actionsTexts[2].color = _style.DefaultActionColor;

               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
   }
}