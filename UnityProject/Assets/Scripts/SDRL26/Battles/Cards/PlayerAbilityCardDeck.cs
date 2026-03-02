using System;
using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SDRL26.Battles.Cards
{
   [Serializable]
   public class PlayerAbilityCardDeck
   {
      [SerializeField] private List<AbilityCard> _pileOfCards = new();
      [SerializeField] private List<AbilityCard> _discard = new();
      [SerializeField] private List<AbilityCard> _handCards = new();

      public PlayerAbilityCardDeck(IReadOnlyList<AbilityCard> starterCards) : this()
      {
         _discard.AddRange(starterCards);
      }

      public PlayerAbilityCardDeck() { }
      public IReadOnlyList<AbilityCard> HandCards => _handCards;

      public void DrawHand(int handSize)
      {
         while (_handCards.Count < handSize)
         {
            if (_pileOfCards.Count == 0)
            {
               _pileOfCards.AddRange(_discard.OrderBy(t => Random.value));
               _discard.Clear();
            }

            _handCards.Add(_pileOfCards[0]);
            _pileOfCards.RemoveAt(0);
         }
      }

      public void AddCard(AbilityCard card) => _discard.Add(card);

      public void Discard(AbilityCard card)
      {
         if (!_handCards.Contains(card))
         {
            return;
         }

         _handCards.Remove(card);
         _discard.Add(card);
      }

      public void DiscardHand()
      {
         _discard.AddRange(_handCards);
         _handCards.Clear();
      }
   }
}