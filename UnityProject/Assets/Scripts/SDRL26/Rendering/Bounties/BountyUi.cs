using SDRL26.GameControllers.GameStates;
using SDRL26.Tooltips;
using UnityEngine;

namespace SDRL26.Rendering.Bounties
{
   public class BountyUi : MonoBehaviour
   {
      [SerializeField] private BountyLootUi[] _lines;

      [SerializeField] private Sprite _goldSprite;
      [SerializeField] private Sprite _cardSprite;
      [SerializeField] private string _goldPattern = "<b>[gold]</b> coins of gold";
      [SerializeField] private string _equipmentPattern = "a nice <b>[equipment]</b>";
      [SerializeField] private string _cardPattern = "a card of <b>[card]</b>";

      private void Start()
      {
         Refresh();
         GameState.OnStateChanged.AddListener(HandleGameStateChanged);
      }

      private void OnDestroy()
      {
         GameState.OnStateChanged.RemoveListener(HandleGameStateChanged);
      }

      private void HandleGameStateChanged(GameState arg0) => Refresh();

      private void Refresh()
      {
         foreach (var t in _lines)
         {
            t.gameObject.SetActive(false);
         }

         if (GameState.CurrentState is not BattleWonGameState battle_won_game_state)
         {
            return;
         }

         var bounty = battle_won_game_state.Bounty;
         var lineIndex = 0;

         if (bounty.Gold > 0)
         {
            var line = ActivateNextLine(ref lineIndex);
            line.Icon = _goldSprite;
            line.Text = _goldPattern.Replace("[gold]", bounty.Gold.ToString());
            line.Tooltip = new Tooltip("Gold", string.Empty);
         }

         foreach (var equipment in bounty.Equipments)
         {
            var line = ActivateNextLine(ref lineIndex);
            line.Icon = equipment.Icon;
            line.Text = _equipmentPattern.Replace("[equipment]", equipment.DisplayName);
            line.Tooltip = new Tooltip($"Equipment: {equipment.DisplayName}", equipment.Description);
         }

         foreach (var card in bounty.Cards)
         {
            var line = ActivateNextLine(ref lineIndex);
            line.Icon = _cardSprite;
            line.Text = _cardPattern.Replace("[card]", card.DisplayName);
            line.Tooltip = new Tooltip($"Card: {card.DisplayName}", card.Description);
         }
      }

      private BountyLootUi ActivateNextLine(ref int index)
      {
         var line = _lines[index];
         line.gameObject.SetActive(true);
         index++;

         return line;
      }
   }
}