using System;
using UnityEngine;

namespace SDRL26.Battlers
{
    [Serializable]
    public class Health
    {
        [SerializeField] private int _maxHealth;

        public int CurrentHealth { get; private set; }
        public bool IsAlive => !IsDead;
        public bool IsDead => CurrentHealth <= 0;

        public Health() : this(5) { }

        public Health(int max)
        {
            _maxHealth = max;
            CurrentHealth = _maxHealth;
        }

        public int Damage(int damage)
        {
            var damageTaken = Mathf.Min(damage, CurrentHealth);

            CurrentHealth -= damageTaken;

            return damageTaken;
        }

        public void FullyHeal()
        {
            CurrentHealth = _maxHealth;
        }
    }
}