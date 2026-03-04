using System.Collections.Generic;
using SDRL26.Battles.Actions;
using SDRL26.Battles.Battlers;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Battleground
{
   public class TargetLinksUi : MonoBehaviour
   {
      [SerializeField] private RectTransform _rect;
      [SerializeField] private BattleGroundUi _battleGround;
      [SerializeField] private RectTransform _linkPrefab;
      [SerializeField] private float _linkDestinationOffset = 20;

      private readonly Dictionary<Battler, List<(RectTransform link, BattlerTokenUi origin, BattlerTokenUi destination)>> _links = new();
      private readonly Queue<RectTransform> _linkPool = new();

      private void Start()
      {
         GameState.OnStateChanged.AddListener(HandleGameStateChanged);
      }

      private void HandleGameStateChanged(GameState newState)
      {
         Battler.OnTargetChanged.RemoveListener(HandleBattlerTargetsChanged);

         if (newState is ContinueBattleGameState)
         {
            Battler.OnTargetChanged.AddListener(HandleBattlerTargetsChanged);
            Battler.OnActionsPerformed.AddListener(HandleBattlerActionPerformed);
            Battler.OnPhaseChanged.AddListener(HandleBattlerPhaseChanged);
            Battler.OnAliveChanged.AddListener(HandleBattlerAliveChanged);
         }
         else
         {
            PoolAllLinks();
         }
      }

      private void HandleBattlerAliveChanged(Battler battler) => RefreshBattlerLinks(battler);
      private void HandleBattlerPhaseChanged(Battler battler) => RefreshBattlerLinks(battler);
      private void HandleBattlerTargetsChanged(Battler battler) => RefreshBattlerLinks(battler);
      private void HandleBattlerActionPerformed(Battler battler) => PoolBattlerLinks(battler);

      private void PoolAllLinks()
      {
         foreach (var battler in _links.Keys)
         {
            PoolBattlerLinks(battler);
         }
      }

      private void PoolBattlerLinks(Battler battler)
      {
         if (!_links.TryGetValue(battler, out var battlerLinks))
         {
            return;
         }

         foreach (var link in battlerLinks)
         {
            Pool(link.link);
         }

         battlerLinks.Clear();
      }

      private void RefreshBattlerLinks(Battler battler)
      {
         PoolBattlerLinks(battler);

         if (battler.CurrentPhase != Battler.Phase.Action || battler.Health.IsDead)
         {
            return;
         }

         if (!_links.TryGetValue(battler, out var battlerLinks))
         {
            battlerLinks = new List<(RectTransform, BattlerTokenUi, BattlerTokenUi)>();
            _links.Add(battler, battlerLinks);
         }

         foreach (var target in ActionResolver.EvaluateAllTargets(battler.Target, battler.TotalAdditionalTargets))
         {
            var linkData = (GetLink(), _battleGround.GetToken(battler), _battleGround.GetToken(target));
            battlerLinks.Add(linkData);
            RefreshLink(linkData);
         }
      }

      private void Update()
      {
         foreach (var battlerLinks in _links.Values)
         {
            foreach (var link in battlerLinks)
            {
               RefreshLink(link);
            }
         }
      }

      private void RefreshLink((RectTransform link, BattlerTokenUi origin, BattlerTokenUi destination) linkData)
      {
         RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, linkData.origin.LinkAnchorOrigin.position, null, out var localOrigin);
         RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, linkData.destination.LinkAnchorDestination.position, null, out var localDestination);

         var originToDestination = localDestination - localOrigin;

         linkData.link.gameObject.SetActive(true);
         linkData.link.anchoredPosition = localOrigin;
         linkData.link.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(originToDestination.y, originToDestination.x) * Mathf.Rad2Deg);
         linkData.link.sizeDelta = new Vector2(originToDestination.magnitude - _linkDestinationOffset, linkData.link.sizeDelta.y);
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