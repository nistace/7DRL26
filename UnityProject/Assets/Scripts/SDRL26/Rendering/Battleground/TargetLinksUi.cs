using System.Collections.Generic;
using SDRL26.Battles.Battlers;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Battleground
{
   public class TargetLinksUi : MonoBehaviour
   {
      [SerializeField] private BattleGroundUi _battleGround;
      [SerializeField] private RectTransform _linkPrefab;

      private Dictionary<Battler, List<(RectTransform link, BattlerTokenUi origin, BattlerTokenUi destination)>> _links = new();
      private Queue<RectTransform> _linkPool = new();

      private void Start()
      {
         GameState.OnStateChanged.AddListener(HandleGameStateChanged);
      }

      private void HandleGameStateChanged(GameState newState)
      {
         if (newState is ContinueBattleGameState battleState)
         {
            Battler.OnTargetsChanged.AddListener(HandleBattlerTargetsChanged);
         }
         else { }
      }

      private void HandleBattlerTargetsChanged(Battler battler)
      {
         if (_links.TryGetValue(battler, out var battlerLinks))
         {
            foreach (var link in battlerLinks)
            {
               Pool(link.link);
            }

            battlerLinks.Clear();
         }
         else
         {
            battlerLinks = new List<(RectTransform, BattlerTokenUi, BattlerTokenUi)>();
            _links.Add(battler, battlerLinks);
         }

         foreach (var target in battler.Targets)
         {
            var linkData = (GetLink(), _battleGround.GetToken(battler), _battleGround.GetToken(target));
            battlerLinks.Add(linkData);
            RefreshLink(linkData);
         }
      }

      private void RefreshLink((RectTransform link, BattlerTokenUi origin, BattlerTokenUi destination) link_data)
      {
      }

      private RectTransform GetLink()
      {
         if (!_linkPool.TryDequeue(out var link))
         {
            link = Instantiate(_linkPrefab, transform);
         }

         link.gameObject.SetActive(true);

         return link;
      }

      private void Pool(RectTransform link)
      {
         _linkPool.Enqueue(link);
         link.gameObject.SetActive(false);
      }
   }
}