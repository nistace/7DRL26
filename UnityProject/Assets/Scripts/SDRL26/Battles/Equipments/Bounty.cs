using System;
using System.Collections.Generic;
using SDRL26.Battles.Cards;
using UnityEngine;

namespace SDRL26.Battles.Equipments
{
   [Serializable]
   public class Bounty
   {
      [SerializeField] private int _gold;
      [SerializeField] private Equipment[] _equipments;
      [SerializeField] private AbilityCard[] _cards;

      public int Gold => _gold;
      public IReadOnlyList<Equipment> Equipments => _equipments;
      public IReadOnlyList<AbilityCard> Cards => _cards;

      public Bounty() { }

      public Bounty(int gold, Equipment[] equipments, AbilityCard[] cards)
      {
         _gold = gold;
         _equipments = equipments;
         _cards = cards;
      }
   }
}