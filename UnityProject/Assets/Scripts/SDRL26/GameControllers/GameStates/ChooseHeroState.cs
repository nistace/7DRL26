using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class ChooseHeroState : GameState
   {
      public IReadOnlyList<Battler> Options { get; }
      private UnityAction OnHeroChosen { get; }

      public ChooseHeroState(IReadOnlyList<Battler> options, UnityAction OnHeroChosen)
      {
         Options = options;
         this.OnHeroChosen = OnHeroChosen;
      }

      protected override void StartState() { }
      protected override void EndState() { }

      public void Choose(Battler selected)
      {
         if (!Options.Contains(selected))
         {
            return;
         }

         GameData.PlayerTeam.AddBattlerPrefabInstance(selected);

         OnHeroChosen?.Invoke();
      }
   }
}