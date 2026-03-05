using SDRL26.Battles.Equipments;
using SDRL26.GameControllers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Rendering.Cards.Merchants
{
   [RequireComponent(typeof(CardUi))]
   public class EquipmentCardUi : MonoBehaviour
   {
      [SerializeField] private CardUi _card;
      [SerializeField] private GameObject _soldObject;
      [SerializeField] private GameObject _tooExpensiveObject;
      [SerializeField] private TMP_Text _priceText;

      public int Index { get; set; }
      public bool Sold { get; set; }
      private Equipment Equipment { get; set; }

      public UnityEvent<EquipmentCardUi> OnClick { get; } = new();

      private void OnEnable()
      {
         _card.OnClick.AddListener(HandleClick);
         GameData.Inventory.OnGoldChanged.AddListener(HandleGoldChanged);
      }

      private void OnDisable()
      {
         _card.OnClick.RemoveListener(HandleClick);
         GameData.Inventory.OnGoldChanged.RemoveListener(HandleGoldChanged);
      }

      private void HandleGoldChanged(int gold) => RefreshInfo();

      private void HandleClick() => OnClick.Invoke(this);

      public void SetUp(int index, Equipment equipment, bool sold)
      {
         Index = index;
         Equipment = equipment;
         Sold = sold;
         RefreshInfo();
      }

      public void SetSold(bool sold)
      {
         Sold = sold;
         RefreshInfo();
      }

      private void RefreshInfo()
      {
         if (!Equipment) return;

         _card.DisplayName = Equipment.DisplayName;
         _card.Portrait = Equipment.Icon;
         _card.Description = Equipment.Description;
         _priceText.text = Equipment.Price.ToString();
         _soldObject.gameObject.SetActive(Sold);
         _tooExpensiveObject.gameObject.SetActive(!Sold && GameData.Inventory.Gold < Equipment.Price);
      }
   }
}