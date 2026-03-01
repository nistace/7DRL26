using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SDRL26.Battles.Battlers;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.ChooseHeroes
{
   public class ChooseHeroUi : MonoBehaviour
   {
      [SerializeField] private Transform[] _slots;
      [SerializeField] private Transform _spawn;
      [SerializeField] private HeroCardUi _heroCardPrefab;
      [SerializeField] private float _spawnDelay = .3f;

      private CancellationTokenSource moveCardsCancellationTokenSource;

      private readonly List<HeroCardUi> _visibleCards = new();

      private void OnEnable()
      {
         GameState.OnStateChanged.AddListener(HandleStateChanged);
      }

      private void OnDisable()
      {
         GameState.OnStateChanged.RemoveListener(HandleStateChanged);
      }

      private void HandleStateChanged(GameState newState)
      {
         if (newState is ChooseHeroState chooseHeroState)
         {
            moveCardsCancellationTokenSource?.Cancel();
            moveCardsCancellationTokenSource?.Dispose();
            moveCardsCancellationTokenSource = new CancellationTokenSource();
            _ = ShowCardsAsync(chooseHeroState.Options, moveCardsCancellationTokenSource.Token);
         }
         else if (_visibleCards.Count > 0)
         {
            moveCardsCancellationTokenSource?.Cancel();
            moveCardsCancellationTokenSource?.Dispose();
            moveCardsCancellationTokenSource = new CancellationTokenSource();
            _ = HideCardsAsync(moveCardsCancellationTokenSource.Token);
         }
      }

      private async UniTask HideCardsAsync(CancellationToken token)
      {
         foreach (var card in _visibleCards)
         {
            CardMovementHandler.Move(card.transform, _spawn);
         }

         while (_visibleCards.Count > 0)
         {
            for (var cardIndex = 0; cardIndex < _visibleCards.Count; cardIndex++)
            {
               if (CardMovementHandler.IsMoving(_visibleCards[cardIndex].transform))
               {
                  continue;
               }

               Destroy(_visibleCards[cardIndex].gameObject);
               _visibleCards.RemoveAt(cardIndex);
            }

            await UniTask.NextFrame(cancellationToken: token);
         }
      }

      private async UniTask ShowCardsAsync(IReadOnlyList<Battler> options, CancellationToken token)
      {
         for (var index = 0; index < _slots.Length; index++)
         {
            _slots[index].gameObject.SetActive(index < options.Count);
         }

         for (var i = 0; i < options.Count; ++i)
         {
            var newCard = Instantiate(_heroCardPrefab, _spawn);
            newCard.SetUp(options[i]);
            newCard.OnClick.AddListener(HandleCardClicked);
            _visibleCards.Add(newCard);
            CardMovementHandler.Move(newCard.transform, _slots[i]);
            await UniTask.WaitForSeconds(_spawnDelay, cancellationToken: token);
         }
      }

      private static void HandleCardClicked(HeroCardUi card)
      {
         if (GameState.CurrentState is ChooseHeroState chooseHeroState)
         {
            chooseHeroState.Choose(card.BattlerPrefab);
         }
      }
   }
}