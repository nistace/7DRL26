using SDRL26.Encounters;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.GameControllers.GameStates
{
   public class MerchantGameState : GameState
   {
      public Merchant Merchant { get; set; }
      public UnityAction OnMerchantEnded { get; }
      public override GameStateTypes Types => GameStateTypes.Merchant;

      public MerchantGameState(Merchant merchant, UnityAction onMerchantEnded)
      {
         Merchant = merchant;
         OnMerchantEnded = onMerchantEnded;
      }

      public void End() => OnMerchantEnded.Invoke();

      public bool Purchase(int index)
      {
         if (!Merchant.TryGet(index, out var equipment)) return false;
         if (GameData.Inventory.Gold < equipment.Price) return false;

         Merchant.Sell(index);
         GameData.Inventory.Gold -= equipment.Price;
         GameData.Inventory[GameData.Inventory.FirstEmptySlotIndex] = Object.Instantiate(equipment);

         return true;
      }

      protected override void EndState() { }
      protected override void StartState() { }
   }
}