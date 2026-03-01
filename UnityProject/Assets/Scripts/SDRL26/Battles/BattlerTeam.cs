using System;
using System.Collections.Generic;
using System.Linq;
using SDRL26.Battlers;
using UnityEngine;

namespace SDRL26
{
    [Serializable]
    public class BattlerTeam
    {
        [SerializeField] private List<Battler> _battlers;

        public BattlerTeam(Battler[] battlers)
        {
            _battlers = battlers.ToList();
        }

        public IReadOnlyList<Battler> Battlers => _battlers;
        public int LowestAliveHealth => _battlers.Where(t => t.Health.IsAlive).Min(t => t.Health.CurrentHealth);

        public void ContinueBattle(float deltaTime)
        {
            foreach (var battler in _battlers)
            {
                battler.ContinueBattle(deltaTime);
            }
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

        public bool IsInTeam(Battler battler) => battler.Team == this;
    }
}