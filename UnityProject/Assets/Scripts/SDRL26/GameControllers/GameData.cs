using SDRL26.Battles;

namespace SDRL26.GameControllers
{
   public static class GameData
   {
      public static BattlerTeam PlayerTeam { get; private set; } = new();
      public static int Level { get; private set; }
      public static Battle CurrentBattle { get; set; }

      public static void Reset()
      {
         PlayerTeam = new BattlerTeam();
         Level = 0;
      }

      public static void NextLevel() => Level++;
   }
}