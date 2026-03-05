using System;

namespace SDRL26.GameControllers.GameStates
{
   [Flags]
   public enum GameStateTypes
   {
      MainMenu = 1 << 0,

      ChooseHero = 1 << 1,
      ChooseEncounter = 1 << 6,

      Merchant = 1 << 7,
      Sorcerer = 1 << 8,
      Blacksmith = 1 << 9,
      MiscEvent = 1 << 10,

      PrepareBattle = 1 << 2,
      ContinueBattle = 1 << 3,
      PauseBattle = 1 << 4,
      BattleWon = 1 << 5,

      NonBattleEncounter = Merchant | Sorcerer | Blacksmith | MiscEvent,
      InBattle = PrepareBattle | ContinueBattle | PauseBattle | BattleWon,
      InGame = InBattle | NonBattleEncounter | ChooseEncounter | ChooseHero,
   }
}