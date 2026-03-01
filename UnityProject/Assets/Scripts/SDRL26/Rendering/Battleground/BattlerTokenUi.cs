using System;
using System.Linq;
using SDRL26.Battles.Battlers;
using SDRL26.Rendering.Battleground.HealthBars;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Battleground
{
   public class BattlerTokenUi : MonoBehaviour
   {
      [SerializeField] private CanvasGroup _canvasGroup;
      [SerializeField] private Image _portrait;
      [SerializeField] private HealthBarUi _healthBar;
      [SerializeField] private TMP_Text[] _actionsTexts;
      [SerializeField] private Image _fillImage;
      [SerializeField] private BattlerTokenStyle _style;

      private Battler _battler;

      public BattlerTokenDisplayMode DisplayMode { get; set; }

      public void Setup(Battler battler)
      {
         _battler = battler;
         _portrait.sprite = battler.Portrait;
         _actionsTexts[0].text = $"> Targets {_battler.Target}";
         _actionsTexts[1].text = $"> [{_battler.ChargeActionTime:0.0}s] {string.Join(", ", battler.Actions.Select(t => t.DisplayString))}";
         _actionsTexts[2].text = $"> [{_battler.RestTime:0.0}s] Rest";
         _healthBar.Setup(battler.Health);
      }

      private void Update()
      {
         if (_battler.Health.IsDead)
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
            case BattlerTokenDisplayMode.Battle when _battler.CurrentPhase is Battler.Phase.Action:
               _canvasGroup.alpha = _style.DefaultOpacity;
               _fillImage.fillAmount = _battler.CurrentLoadRatio;
               _fillImage.color = _style.FillActionColor;
               _actionsTexts[0].color = _style.InactiveActionColor;
               _actionsTexts[1].color = _style.DefaultActionColor;
               _actionsTexts[2].color = _style.InactiveActionColor;

               break;
            case BattlerTokenDisplayMode.Battle:
               _canvasGroup.alpha = _style.RestOpacity;
               _fillImage.fillAmount = 1 - _battler.CurrentLoadRatio;
               _fillImage.color = _style.FillRestColor;
               _actionsTexts[0].color = _style.InactiveActionColor;
               _actionsTexts[1].color = _style.InactiveActionColor;
               _actionsTexts[2].color = _style.DefaultActionColor;

               break;
            case BattlerTokenDisplayMode.Prepare:
               _canvasGroup.alpha = _style.DefaultOpacity;
               _fillImage.fillAmount = 1 - _battler.CurrentLoadRatio;
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