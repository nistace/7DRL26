using System;

namespace SDRL26.GameControllers.GameStates
{
   [Flags]
   public enum GameStateTypes
   {
      MainMenu = 1 << 0,
      ChooseHero = 1 << 1,
      PrepareBattle = 1 << 2,
      ContinueBattle = 1 << 3,
      PauseBattle = 1 << 4,

      InGame = ChooseHero | PrepareBattle | ContinueBattle | PauseBattle,
   }
}