using System;
using SDRL26.Battles.Battlers;
using SDRL26.Battles.Equipments;
using SDRL26.Rendering.Battleground.HealthBars;
using SDRL26.Rendering.Equipments;
using SDRL26.Rendering.Shared;
using SDRL26.Rendering.Shared.Portraits;
using SDRL26.Tooltips;
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
      [SerializeField] private TMP_Text _actionText;
      [SerializeField] private ActionIcon _actionIcon;
      [SerializeField] private Portrait _portrait;
      [SerializeField] private HealthBarUi _healthBar;
      [SerializeField] private Image _fillImage;
      [SerializeField] private BattlerTokenStyle _style;
      [SerializeField] private EquipmentSlotUi[] _equipmentSlots;
      [SerializeField] private BattlerDisplayData _displayData;

      public BattlerTokenDisplayMode DisplayMode { get; set; }
      public Battler Battler { get; private set; }
      public Transform LinkAnchorOrigin => _linkAnchorOrigin;
      public Transform LinkAnchorDestination => _linkAnchorDestination;

      public UnityEvent OnBattlerChanged { get; } = new();

      public void Setup(Battler battler)
      {
         CleanUpCurrentBattler();

         Battler = battler;
         RefreshBattlerInfo();
         _healthBar.Setup(battler.Health);

         Battler.OnPostureChanged.AddListener(HandleBattlerPostureChanged);
         Battler.OnEquipmentChanged.AddListener(HandleEquipmentChanged);

         foreach (var equipmentSlot in _equipmentSlots)
         {
            equipmentSlot.OnSetRequested.AddListener(HandleSetRequested);
            equipmentSlot.OnRemovalRequested.AddListener(HandleRemovalRequested);
         }

         OnBattlerChanged.Invoke();
      }

      private void HandleRemovalRequested(EquipmentSlotUi slot)
      {
         if (!Battler) return;

         Battler.RemoveEquipment(slot.Index);
      }

      private void HandleSetRequested(EquipmentSlotUi slot, Equipment equipment)
      {
         if (!Battler) return;

         Battler.AddEquipment(slot.Index, equipment);
      }

      private void OnDestroy() => CleanUpCurrentBattler();

      private void CleanUpCurrentBattler()
      {
         if (!Battler)
         {
            return;
         }

         Battler.OnPostureChanged.RemoveListener(HandleBattlerPostureChanged);
         Battler.OnEquipmentChanged.RemoveListener(HandleEquipmentChanged);
      }

      private void HandleEquipmentChanged((uint index, Equipment equipment) change) => _equipmentSlots[change.index].SetEquipment(change.equipment);

      private void HandleBattlerPostureChanged(BattlerPosture newPosture) => RefreshBattlerInfo();

      private void RefreshBattlerInfo()
      {
         _portrait.Set(Battler.PortraitSize, Battler.GetCurrentPortrait());

         var posture = Battler.Posture;

         _actionIcon.Set(Battler.Posture.MainActionIcon,
            Battler.Posture.MainActionAmount,
            Battler.Posture.HasSideEffects,
            new Tooltip(posture.ActionDisplayName, _displayData.GetActionTooltip(Battler, Battler.Posture))
         );

         for (var i = 0; i < _equipmentSlots.Length; i++)
         {
            _equipmentSlots[i].SetEquipment(Battler.GetEquipment(i));
         }
      }

      private void Update()
      {
         _portrait.Set(Battler.PortraitSize, Battler.GetCurrentPortrait());

         if (Battler.Health.IsDead)
         {
            _actionText.text = "Deading";
            _actionIcon.gameObject.SetActive(false);
            _fillImage.fillAmount = 0;
            _fillImage.color = Color.clear;

            return;
         }

         _actionIcon.gameObject.SetActive(true);

         switch (DisplayMode)
         {
            case BattlerTokenDisplayMode.Battle when Battler.CurrentPhase is Battler.Phase.Action:
               _actionText.text = Battler.Posture.ActionDisplayName;
               _fillImage.fillAmount = Battler.CurrentLoadRatio;
               _fillImage.color = _style.FillActionColor;

               break;
            case BattlerTokenDisplayMode.Battle:
               _actionText.text = "Resting";
               _fillImage.fillAmount = 1 - Battler.CurrentLoadRatio;
               _fillImage.color = _style.FillRestColor;

               break;
            case BattlerTokenDisplayMode.Prepare:
               _actionText.text = "Preparing";
               _fillImage.fillAmount = 1 - Battler.CurrentLoadRatio;
               _fillImage.color = _style.FillRestColor;

               break;
            case BattlerTokenDisplayMode.Default:
               _actionText.text = string.Empty;
               _fillImage.fillAmount = 0;
               _fillImage.color = Color.clear;

               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
   }
}