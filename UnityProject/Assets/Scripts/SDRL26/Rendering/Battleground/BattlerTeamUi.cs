using System.Collections.Generic;
using SDRL26.Battles;
using UnityEngine;

namespace SDRL26.Rendering.Battleground
{
   public class BattlerTeamUi : MonoBehaviour
   {
      [SerializeField] private CanvasGroup _canvasGroup;
      [SerializeField] private Transform _container;
      [SerializeField] private BattlerTokenUi _battlerTokenPrefab;

      private BattlerTeam _team;
      private readonly List<BattlerTokenUi> _tokens = new();

      public void Setup(BattlerTeam team)
      {
         _team?.OnChanged.RemoveListener(HandleChanged);
         _team = team;
         _team.OnChanged.AddListener(HandleChanged);

         Refresh();
      }

      private void HandleChanged()
      {
         Refresh();
      }

      private void Refresh()
      {
         for (var battlerIndex = 0; battlerIndex < _team.Battlers.Count; battlerIndex++)
         {
            var battler = _team.Battlers[battlerIndex];

            if (_tokens.Count <= battlerIndex)
            {
               _tokens.Add(Instantiate(_battlerTokenPrefab, transform));
            }

            _tokens[battlerIndex].gameObject.SetActive(true);
            _tokens[battlerIndex].Setup(battler);
         }

         for (var tokenIndex = _team.Battlers.Count; tokenIndex < _tokens.Count; tokenIndex++)
         {
            _tokens[tokenIndex].gameObject.SetActive(false);
         }
      }

      public void SetVisible(bool visible) => _canvasGroup.alpha = visible ? 1 : 0;
   }
}