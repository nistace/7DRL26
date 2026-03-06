using SDRL26.Encounters;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace SDRL26.GameControllers.GameStates
{
   public class BlacksmithGameState : GameState
   {
      public Blacksmith Blacksmith { get; }
      private UnityAction OnEnded { get; }
      public override GameStateTypes Types => GameStateTypes.Blacksmith;

      public BlacksmithGameState(Blacksmith blacksmith, UnityAction onEnded)
      {
         Blacksmith = blacksmith;
         OnEnded = onEnded;
      }

      public void End(bool accept)
      {
         if (accept)
         {
            if (GameData.Inventory.TryGetSlotIndex(Blacksmith.GiveEquipment, out var index))
            {
               GameData.Inventory[index] = null;
            }
            else
            {
               foreach (var battler in GameData.PlayerTeam.Battlers)
               {
                  if (battler.HasEquipment(Blacksmith.GiveEquipment, out index))
                  {
                     battler.RemoveEquipment(index);
                  }
               }
            }

            GameData.Inventory[GameData.Inventory.FirstEmptySlotIndex] = Object.Instantiate(Blacksmith.ReceiveEquipmentPrefab);
         }

         OnEnded.Invoke();
      }

      protected override void EndState() { }
      protected override void StartState() { }
   }
}