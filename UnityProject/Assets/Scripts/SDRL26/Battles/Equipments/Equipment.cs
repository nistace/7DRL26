using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Equipments
{
   public class Equipment : MonoBehaviour
   {
      [SerializeField] private Sprite _icon;
      [SerializeField] private string _displayName;
      [SerializeField] private string _description;
      [SerializeField] private int _price = 3;

      public Sprite Icon => _icon;
      public string DisplayName => _displayName;
      public int Price => _price;
      public string Description => _description;

      public void Equip(Battler battler)
      {
         foreach (var effect in GetComponents<EquipmentEffect>())
         {
            effect.Apply(battler);
         }
      }

      public void Unequip()
      {
         foreach (var effect in GetComponents<EquipmentEffect>())
         {
            effect.Undo();
         }
      }
   }
}