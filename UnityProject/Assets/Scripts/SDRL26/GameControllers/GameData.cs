using System.Collections.Generic;
using SDRL26.Battles;
using SDRL26.Battles.Cards;
using UnityEngine.Events;

namespace SDRL26.GameControllers
{
   public static class GameData
   {
      public static BattlerTeam PlayerTeam { get; private set; } = new();
      public static PlayerAbilityCardDeck PlayerDeck { get; private set; } = new();
      public static int Level { get; private set; }
      public static Battle CurrentBattle { get; set; }

      public static UnityEvent<BattlerTeam> OnPlayerTeamChanged { get; } = new();
      public static UnityEvent<int> OnLevelChanged { get; } = new();

      public static void Reset(IReadOnlyList<AbilityCard> starterCards)
      {
         PlayerTeam = new BattlerTeam();
         PlayerDeck = new PlayerAbilityCardDeck(starterCards);
         Level = 0;
         OnPlayerTeamChanged.Invoke(PlayerTeam);
         OnLevelChanged.Invoke(Level);
      }

      public static void NextLevel()
      {
         Level++;
         OnLevelChanged.Invoke(Level);
      }
   }
}