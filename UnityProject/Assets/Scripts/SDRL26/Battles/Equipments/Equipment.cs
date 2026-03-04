using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Equipments
{
   public class Equipment : MonoBehaviour
   {
      [SerializeField] private Sprite _icon;
      [SerializeField] private string _displayName;

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