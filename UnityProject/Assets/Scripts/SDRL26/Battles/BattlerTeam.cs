using System;
using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Battles
{
   [Serializable]
   public class BattlerTeam
   {
      [SerializeField] private List<Battler> _battlers;

      public IReadOnlyList<Battler> Battlers => _battlers;
      public int LowestAliveHealth => _battlers.Where(t => t.Health.IsAlive).Min(t => t.Health.CurrentHealth);
      public UnityEvent OnChanged { get; } = new();

      public BattlerTeam(Battler[] battlers)
      {
         _battlers = battlers.ToList();
      }

      public BattlerTeam() : this(Array.Empty<Battler>()) { }

      public void ContinueBattle(float deltaTime)
      {
         foreach (var battler in _battlers)
         {
            battler.ContinueBattle(deltaTime);
         }
      }

      public void Add(Battler battler)
      {
         _battlers.Add(battler);
         OnChanged.Invoke();
      }

      public List<Battler> GetFirst(Func<Battler, bool> condition)
      {
         var battler = _battlers.Where(condition).FirstOrDefault();

         if (battler == null) return new List<Battler>();

         return new List<Battler> { battler };
      }

      public List<Battler> GetLast(Func<Battler, bool> condition)
      {
         var battler = _battlers.Where(condition).LastOrDefault();

         if (battler == null) return new List<Battler>();

         return new List<Battler> { battler };
      }

      public List<Battler> GetRandom(Func<Battler, bool> condition)
      {
         var battler = _battlers.Where(condition).OrderBy(_ => UnityEngine.Random.value).ToList().FirstOrDefault();

         if (battler == null) return new List<Battler>();

         return new List<Battler> { battler };
      }

      public bool IsInTeam(Battler battler) => battler.Team == this;
   }
}