using System;
using UnityEngine;

namespace SDRL26.Battlers
{
    [Serializable]
    public class Health
    {
        [SerializeField] private int _maxHealth;

        public int CurrentHealth { get; private set; }
        public int CurrentShield { get; private set; }
        public bool IsAlive => !IsDead;
        public bool IsDead => CurrentHealth <= 0;

        public Health() : this(5) { }

        public Health(int max)
        {
            _maxHealth = max;
            CurrentHealth = _maxHealth;
        }

        public int Damage(int damage, bool trueDamage = false)
        {
            var damageToShield = trueDamage ? 0 : Mathf.Min(CurrentShield, damage);
            var damageTaken = Mathf.Min(damage, CurrentHealth);

            CurrentHealth -= damageTaken;

            return damageTaken;
        }

        public void FullyHeal()
        {
            CurrentHealth = _maxHealth;
        }

        public int Heal(int points)
        {
            var healTaken = Mathf.Min(points, _maxHealth - CurrentHealth);

            CurrentHealth += healTaken;

            return healTaken;
        }

        public int Shield(int points)
        {
            CurrentShield += points;

            return points;
        }
    }
}