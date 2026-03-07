using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles;
using SDRL26.Battles.Battlers;
using SDRL26.Rendering.Shared;
using UnityEngine;

namespace SDRL26.Rendering.Battleground
{
   public class BattlerTeamUi : MonoBehaviour
   {
      [SerializeField] private CanvasGroup _canvasGroup;
      [SerializeField] private Transform _tokenActualContainer;
      [SerializeField] private BattlerTokenUi _battlerTokenPrefab;
      [SerializeField] private VerticalLayoutHelper _verticalLayoutHelper;

      private BattlerTeam _team;
      private readonly Dictionary<Battler, BattlerTokenUi> _tokens = new();
      private readonly Queue<BattlerTokenUi> _tokenPool = new();
      private BattlerTokenDisplayMode TokensDisplayMode { get; set; }

      public void Setup(BattlerTeam team)
      {
         _team?.OnChanged.RemoveListener(HandleChanged);
         _team = team;
         _team.OnChanged.AddListener(HandleChanged);

         foreach (var battlerToRemove in _tokens.Keys.ToArray())
         {
            Pool(battlerToRemove);
         }

         Refresh();
      }

      private void HandleChanged()
      {
         Refresh();
      }

      private void Pool(Battler battler)
      {
         var token = _tokens[battler];
         _tokenPool.Enqueue(token);
         token.gameObject.SetActive(false);
         token.transform.localPosition = Vector3.zero;
         _tokens.Remove(battler);
      }

      private void Refresh()
      {
         foreach (var battlerToRemove in _tokens.Keys.Except(_team.Battlers).ToArray())
         {
            Pool(battlerToRemove);
         }

         for (var battlerIndex = 0; battlerIndex < _team.Battlers.Count; battlerIndex++)
         {
            var battler = _team.Battlers[battlerIndex];

            if (!_tokens.TryGetValue(battler, out var token))
            {
               if (!_tokenPool.TryDequeue(out token))
               {
                  token = Instantiate(_battlerTokenPrefab, _tokenActualContainer);
               }

               _tokens.Add(battler, token);
            }

            token.gameObject.SetActive(true);
            token.transform.SetSiblingIndex(battlerIndex);
            token.Setup(battler);
            token.DisplayMode = TokensDisplayMode;

            token.GetComponent<SmoothMover>().Target = _verticalLayoutHelper.GetChild(battlerIndex);
         }

         _verticalLayoutHelper.SetChildrenInUse( _team.Battlers.Count);
      }

      public void SetVisible(bool visible) => _canvasGroup.alpha = visible ? 1 : 0;

      public void SetTokensDisplayMode(BattlerTokenDisplayMode mode)
      {
         TokensDisplayMode = mode;

         foreach (var token in _tokens.Values)
         {
            token.DisplayMode = TokensDisplayMode;
         }
      }

      public BattlerTokenUi GetToken(Battler battler) => _tokens.FirstOrDefault(t => t.Key == battler).Value;
   }
}