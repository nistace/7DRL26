using System;
using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace SDRL26.Battles
{
   [Serializable]
   public class BattlerTeam
   {
      [SerializeField] private List<Battler> _battlers;

      public IReadOnlyList<Battler> Battlers => _battlers;
      public UnityEvent OnChanged { get; } = new();

      public BattlerTeam(Battler[] battlerPrefabs)
      {
         _battlers = new List<Battler>();

         foreach (var battlerPrefab in battlerPrefabs)
         {
            AddBattlerPrefabInstance(battlerPrefab, notify: false);
         }
      }

      public BattlerTeam() : this(Array.Empty<Battler>()) { }

      public void ContinueBattle(float deltaTime)
      {
         foreach (var battler in _battlers.ToArray())
         {
            battler.ContinueBattle(deltaTime);
         }
      }

      public Battler AddBattlerPrefabInstance(Battler battlerPrefab, int? position = null, bool notify = true)
      {
         var actualPosition = position ?? _battlers.Count;
         var instance = Object.Instantiate(battlerPrefab);
         instance.Initialize();
         _battlers.Insert(actualPosition, instance);
         instance.Team = this;

         if (notify) OnChanged.Invoke();

         return instance;
      }

      public Battler GetFirst(Func<Battler, bool> where) => _battlers.Where(where).FirstOrDefault();
      public Battler GetLast(Func<Battler, bool> where) => _battlers.Where(where).LastOrDefault();
      public Battler GetRandom(Func<Battler, bool> where) => _battlers.Where(where).OrderBy(_ => UnityEngine.Random.value).ToList().FirstOrDefault();
      public Battler GetFirst(Func<Battler, int> order) => _battlers.OrderBy(order).FirstOrDefault();
      public Battler GetFirst(Func<Battler, bool> where, Func<Battler, float> order) => _battlers.Where(where).OrderBy(order).FirstOrDefault();

      public bool IsInTeam(Battler battler) => battler.Team == this;
      public int IndexOf(Battler battler) => _battlers.IndexOf(battler);
      public void NotifyChanged() => OnChanged.Invoke();

      public bool TryGetNextBattler(Battler battler, out Battler next, bool loop, bool allowSame)
      {
         next = default;
         var index = _battlers.IndexOf(battler);

         if (index < 0) return false;

         var nextIndex = (index + 1);

         if (!loop && nextIndex >= _battlers.Count) return false;

         nextIndex %= _battlers.Count;

         if (!allowSame && nextIndex == index) return false;

         next = _battlers[nextIndex];

         return true;
      }

      public void MoveBattlerToIndex(Battler battler, int index)
      {
         var currentIndex = _battlers.IndexOf(battler);

         if (currentIndex < 0) return;

         var newIndex = Mathf.Clamp(index, 0, _battlers.Count - 1);

         if (currentIndex == newIndex) return;

         _battlers.Remove(battler);
         _battlers.Insert(newIndex, battler);

         OnChanged.Invoke();
      }

      public void MoveBattlerByDelta(Battler battler, int delta)
      {
         var index = _battlers.IndexOf(battler);

         if (index < 0) return;

         MoveBattlerToIndex(battler, index + delta);
      }

      public void ResetBattlers()
      {
         foreach (var battler in _battlers)
         {
            battler.ResetForBattle();
         }
      }
   }
}