using System.Linq;
using SDRL26.Battles.Equipments;
using UnityEngine;

namespace SDRL26.Encounters
{
   [CreateAssetMenu]
   public class MerchantGenerator : ScriptableObject
   {
      [SerializeField] private Sprite[] _portraits;
      [SerializeField] private string[] _displayNames = { "Merchant" };
      [SerializeField] private string[] _descriptions = { "Prepare your gold, this one wants to sell!" };
      [SerializeField] private Equipment[] _equipments;
      [SerializeField] private int _choices = 3;

      public Merchant GenerateMerchant() => new(_equipments.OrderBy(_ => Random.value).Take(_choices).ToArray(),
         _portraits[Random.Range(0, _portraits.Length)],
         _displayNames[Random.Range(0, _displayNames.Length)],
         _descriptions[Random.Range(0, _descriptions.Length)]
      );
   }
}