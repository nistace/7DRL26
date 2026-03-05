using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Bounties
{
   public class BountyUi : MonoBehaviour
   {
      [SerializeField] private BountyLootUi[] _lines;

      [SerializeField] private Sprite _goldSprite;
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
            _lines[lineIndex].Icon = _goldSprite;
            _lines[lineIndex].Text = _goldPattern.Replace("[gold]", bounty.Gold.ToString());
            _lines[lineIndex].gameObject.SetActive(true);

            lineIndex++;
         }

         foreach (var equipment in bounty.Equipments)
         {
            _lines[lineIndex].Icon = equipment.Icon;
            _lines[lineIndex].Text = _equipmentPattern.Replace("[equipment]", equipment.DisplayName);
            _lines[lineIndex].gameObject.SetActive(true);

            lineIndex++;
         }

         foreach (var card in bounty.Cards)
         {
            _lines[lineIndex].Icon = card.Icon;
            _lines[lineIndex].Text = _cardPattern.Replace("[card]", card.DisplayName);
            _lines[lineIndex].gameObject.SetActive(true);

            lineIndex++;
         }
      }
   }
}