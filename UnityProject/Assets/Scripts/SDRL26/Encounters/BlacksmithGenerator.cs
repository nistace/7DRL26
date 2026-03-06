using SDRL26.Battles.Equipments;
using UnityEngine;

namespace SDRL26.Encounters
{
   [CreateAssetMenu]
   public class BlacksmithGenerator : ScriptableObject
   {
      [SerializeField] private Sprite[] _portraits;
      [SerializeField] private string[] _displayNames = { "Blacksmith" };
      [SerializeField] private string[] _descriptions = { "He will offer to kindly upgrade one of your objects." };

      public Blacksmith GenerateBlacksmith(Equipment fromEquipment, Equipment upgrade) => new(fromEquipment,
         upgrade,
         _portraits[Random.Range(0, _portraits.Length)],
         _displayNames[Random.Range(0, _displayNames.Length)],
         _descriptions[Random.Range(0, _descriptions.Length)]
      );
   }
}