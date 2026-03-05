using System.Collections.Generic;
using SDRL26.Battles;
using SDRL26.Battles.Cards;
using SDRL26.Battles.Equipments;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.GameControllers
{
   public static class GameData
   {
      public static BattlerTeam PlayerTeam { get; private set; } = new();
      public static Inventory Inventory { get; private set; } = new();
      public static PlayerAbilityCardDeck PlayerDeck { get; private set; } = new();
      public static int Level { get; private set; }
      public static Battle CurrentBattle { get; set; }

      public static UnityEvent<BattlerTeam> OnPlayerTeamChanged { get; } = new();
      public static UnityEvent<int> OnLevelChanged { get; } = new();
      public static UnityEvent OnInventoryReset { get; } = new();

      public static void Reset(IReadOnlyList<AbilityCard> starterCards)
      {
         PlayerTeam = new BattlerTeam();
         PlayerDeck = new PlayerAbilityCardDeck(starterCards);
         Inventory = new Inventory();
         Level = 0;
         OnPlayerTeamChanged.Invoke(PlayerTeam);
         OnLevelChanged.Invoke(Level);
         OnInventoryReset.Invoke();
      }

      public static void NextLevel()
      {
         Level++;
         OnLevelChanged.Invoke(Level);
      }

      public static void EarnCurrentBattleBounty()
      {
         Inventory.Gold += CurrentBattle.Bounty.Gold;

         foreach (var equipment in CurrentBattle.Bounty.Equipments)
         {
            Inventory[Inventory.FirstEmptySlotIndex] = Object.Instantiate(equipment);
         }

         foreach (var card in CurrentBattle.Bounty.Cards)
         {
            PlayerDeck.AddCard(card);
         }
      }
   }
}