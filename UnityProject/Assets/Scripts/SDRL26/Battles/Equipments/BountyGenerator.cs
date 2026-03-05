using System.Linq;
using SDRL26.Battles.Cards;
using UnityEngine;

namespace SDRL26.Battles.Equipments
{
   [CreateAssetMenu]
   public class BountyGenerator : ScriptableObject
   {
      [SerializeField] private int _minGold;
      [SerializeField] private int _maxGold = 3;
      [SerializeField] private Equipment[] _possibleEquipments;
      [SerializeField] private int _minEquipments;
      [SerializeField] private int _maxEquipments;
      [SerializeField] private AbilityCard[] _possibleCards;
      [SerializeField] private int _minCards;
      [SerializeField] private int _maxCards;

      [SerializeField] private int _minEquipmentPlusCards;
      [SerializeField] private int _maxEquipmentPlusCards = 1;
      [SerializeField] private int _compensateMissingEquipmentsAndCardsWithGold = 5;

      public Bounty RandomBounty()
      {
         var equipments = Random.Range(_minEquipments, Mathf.Min(_maxEquipments, _maxEquipmentPlusCards));
         var cards = Random.Range(Mathf.Max(_minCards, _minEquipmentPlusCards - equipments), Mathf.Min(_maxCards, _maxEquipmentPlusCards - equipments));

         var gold = Random.Range(_minGold, _maxGold + 1) + Mathf.Max(_maxEquipmentPlusCards - (equipments + cards), 0) * _compensateMissingEquipmentsAndCardsWithGold;

         return new Bounty(gold, _possibleEquipments.OrderBy(_ => Random.value).Take(equipments).ToArray(), _possibleCards.OrderBy(_ => Random.value).Take(cards).ToArray());
      }
   }
}