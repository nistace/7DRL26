using UnityEngine;

namespace SDRL26.Battles.Equipments
{
   public class EquipmentUpgrade : MonoBehaviour
   {
      [SerializeField] private Equipment[] _upgrades;

      public Equipment RandomUpgrade => _upgrades[Random.Range(0, _upgrades.Length)];
      public int Count => _upgrades.Length;
   }
}