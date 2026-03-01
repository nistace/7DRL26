using System;
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
      [SerializeField] private Color _fillActionColor = Color.white;
      [SerializeField] private Color _fillRestColor = Color.red;

      private Battler _battler;

      public void Setup(Battler battler)
      {
         _battler = battler;
         _portrait.sprite = battler.Portrait;
         _healthBar.Setup(battler.Health);
      }

      private void Update()
      {
         _canvasGroup.alpha = _battler.Health.IsDead
            ? .3f
            : _battler.CurrentPhase switch
            {
               Battler.Phase.Action => 1,
               Battler.Phase.Rest or Battler.Phase.Start => .6f,
               _ => throw new ArgumentOutOfRangeException()
            };

         _fillImage.fillAmount = _battler.CurrentPhase switch
         {
            Battler.Phase.Action => _battler.CurrentLoadRatio,
            Battler.Phase.Rest or Battler.Phase.Start => 1 - _battler.CurrentLoadRatio,
            _ => throw new ArgumentOutOfRangeException()
         };

         _fillImage.color = _battler.CurrentPhase switch
         {
            Battler.Phase.Action => _fillActionColor,
            Battler.Phase.Rest or Battler.Phase.Start => _fillRestColor,
            _ => throw new ArgumentOutOfRangeException()
         };
      }
   }
}