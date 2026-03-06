using System.Linq;
using SDRL26.Battles.Cards;
using UnityEngine;

namespace SDRL26.Encounters
{
   [CreateAssetMenu]
   public class SorcererGenerator : ScriptableObject
   {
      [SerializeField] private Sprite[] _portraits;
      [SerializeField] private string[] _displayNames = { "Sorcerer" };
      [SerializeField] private string[] _descriptions = { "This one might give you powerful abilities." };
      [SerializeField] private AbilityCard[] _abilities;
      [SerializeField] private int _choices = 3;

      public Sorcerer GenerateSorcerer() => new(_abilities.OrderBy(_ => Random.value).Take(_choices).ToArray(),
         _portraits[Random.Range(0, _portraits.Length)],
         _displayNames[Random.Range(0, _displayNames.Length)],
         _descriptions[Random.Range(0, _descriptions.Length)]
      );
   }
}