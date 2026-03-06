using System;
using System.Collections.Generic;
using SDRL26.Battles.Cards;
using SDRL26.Battles.Equipments;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SDRL26.Encounters
{
   [Serializable]
   public class Sorcerer : IEncounter
   {
      public Sprite Portrait { get; }
      public string DisplayName { get; }
      public string Description { get; }

      private AbilityCard[] _abilities;
      public IReadOnlyList<AbilityCard> Abilities => _abilities;

      public UnityEvent OnChanged { get; } = new();

      public Sorcerer(AbilityCard[] abilitiesPrefabs, Sprite portrait, string displayName, string description)
      {
         _abilities = abilitiesPrefabs;
         Portrait = portrait;
         DisplayName = displayName;
         Description = description;
      }

      public bool Has(AbilityCard ability) => Array.IndexOf(_abilities, ability) >= 0;
   }
}