using System;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Battles.Battlers
{
   [Serializable]
   public class Health
   {
      [SerializeField] private int _maxHealth;

      public int CurrentHealth { get; private set; }
      public int CurrentShield { get; private set; }
      public bool IsAlive => !IsDead;
      public bool IsDead => CurrentHealth <= 0;
      public int MaxHealth => _maxHealth;

      public UnityEvent OnChanged { get; } = new();
      public UnityEvent OnDied { get; } = new();

      public Health() : this(5) { }

      public Health(int max)
      {
         _maxHealth = max;
         CurrentHealth = _maxHealth;
      }

      public int Damage(int damage, bool trueDamage = false)
      {
         var damageToShield = trueDamage ? 0 : Mathf.Min(CurrentShield, damage);

         CurrentShield -= damageToShield;

         var damageToHealth = Mathf.Min(damage - damageToShield, CurrentHealth);

         CurrentHealth -= damageToHealth;

         if (damageToShield > 0 || damageToHealth > 0)
         {
            OnChanged.Invoke();

            if (IsDead)
            {
               OnDied.Invoke();
            }
         }


         return damageToHealth + damageToShield;
      }

      public void FullyHeal()
      {
         CurrentHealth = _maxHealth;

         OnChanged.Invoke();
      }

      public int Heal(int points)
      {
         var healTaken = Mathf.Min(points, _maxHealth - CurrentHealth);

         CurrentHealth += healTaken;

         OnChanged.Invoke();

         return healTaken;
      }

      public int Shield(int points)
      {
         CurrentShield += points;

         OnChanged.Invoke();

         return points;
      }
   }
}