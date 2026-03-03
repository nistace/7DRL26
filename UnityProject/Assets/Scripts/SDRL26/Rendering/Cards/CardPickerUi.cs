using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Cards
{
   public abstract class CardPickerUi<TState, TDataType> : MonoBehaviour where TState : GameState
   {
      [SerializeField] private float _spawnDelay = .3f;

      private readonly List<Transform> _visibleCards = new();
      private CancellationTokenSource moveCardsCancellationTokenSource;

      private void OnEnable() => GameState.OnStateChanged.AddListener(HandleStateChanged);

      private void OnDisable() => GameState.OnStateChanged.RemoveListener(HandleStateChanged);

      private void HandleStateChanged(GameState newState)
      {
         if (newState is TState theState)
         {
            Show(GetOptions(theState).Select(SpawnCard).ToArray());
         }
         else
         {
            Hide();
         }
      }

      protected abstract IReadOnlyList<TDataType> GetOptions(TState state);
      protected abstract Transform SpawnCard(TDataType arg);

      private void OnDestroy()
      {
         moveCardsCancellationTokenSource?.Cancel();
         moveCardsCancellationTokenSource?.Dispose();
         moveCardsCancellationTokenSource = null;
      }

      private void Show(Transform[] cards)
      {
         moveCardsCancellationTokenSource?.Cancel();
         moveCardsCancellationTokenSource?.Dispose();
         moveCardsCancellationTokenSource = new CancellationTokenSource();
         _ = ShowCardsAsync(cards, moveCardsCancellationTokenSource.Token);
      }

      private void Hide()
      {
         moveCardsCancellationTokenSource?.Cancel();
         moveCardsCancellationTokenSource?.Dispose();
         moveCardsCancellationTokenSource = new CancellationTokenSource();

         if (_visibleCards.Count > 0)
         {
            _ = HideCardsAsync(moveCardsCancellationTokenSource.Token);
         }
      }

      private async UniTask HideCardsAsync(CancellationToken token)
      {
         foreach (var card in _visibleCards)
         {
            CardMovementHandler.HideCard(card.transform);
         }

         while (_visibleCards.Count > 0)
         {
            if (!CardMovementHandler.IsMoving(_visibleCards[0].transform))
            {
               Destroy(_visibleCards[0].gameObject);
               _visibleCards.RemoveAt(0);
            }

            await UniTask.NextFrame(cancellationToken: token);
         }
      }

      private async UniTask ShowCardsAsync(Transform[] Cards, CancellationToken token)
      {
         CardMovementHandler.SetSlotCountActive(Cards.Length);

         for (var i = 0; i < Cards.Length; ++i)
         {
            _visibleCards.Add(Cards[i]);
            CardMovementHandler.MoveToSlot(Cards[i].transform, i);
            await UniTask.WaitForSeconds(_spawnDelay, cancellationToken: token);
         }
      }
   }
}