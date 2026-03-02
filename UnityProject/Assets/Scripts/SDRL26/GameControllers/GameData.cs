using System.Collections.Generic;
using SDRL26.Battles;
using SDRL26.Battles.Cards;

namespace SDRL26.GameControllers
{
   public static class GameData
   {
      public static BattlerTeam PlayerTeam { get; private set; } = new();
      public static PlayerAbilityCardDeck PlayerDeck { get; private set; } = new();
      public static int Level { get; private set; }
      public static Battle CurrentBattle { get; set; }

      public static void Reset(IReadOnlyList<AbilityCard> starterCards)
      {
         PlayerTeam = new BattlerTeam();
         PlayerDeck = new(starterCards);
         Level = 0;
      }

      public static void NextLevel() => Level++;
   }
}