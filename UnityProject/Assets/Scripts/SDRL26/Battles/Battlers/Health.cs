using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SDRL26.Battles.Battlers
{
   [Serializable]
   public class Health
   {
      [FormerlySerializedAs("_maxHealth")]
      [SerializeField] private int _defaultMaxHealth;

      public int MaxHealth { get; set; }
      public int CurrentHealth { get; private set; }
      public int CurrentShield { get; private set; }
      public bool IsAlive => !IsDead;
      public bool IsDead => CurrentHealth <= 0;
      public int MissingHealth => MaxHealth - CurrentHealth;
      public float Ratio => (float)CurrentHealth / MaxHealth;

      public UnityEvent OnChanged { get; } = new();
      public UnityEvent OnDied { get; } = new();
      public UnityEvent OnRevived { get; } = new();

      public Health() : this(5) { }

      public Health(int default_max)
      {
         _defaultMaxHealth = default_max;
         MaxHealth = _defaultMaxHealth;
         CurrentHealth = MaxHealth;
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
         CurrentHealth = _defaultMaxHealth;

         OnChanged.Invoke();
      }

      public int Heal(int points, bool canRevive)
      {
         if (!canRevive && IsDead)
         {
            return 0;
         }

         var healTaken = Mathf.Min(points, _defaultMaxHealth - CurrentHealth);

         CurrentHealth += healTaken;

         OnChanged.Invoke();

         if (CurrentHealth == healTaken)
         {
            CurrentShield = 0;
            OnRevived.Invoke();
         }

         return healTaken;
      }

      public int Revive(float healthRatio)
      {
         if (IsAlive) return 0;

         var healTaken = Mathf.CeilToInt(healthRatio * MaxHealth);

         if (healTaken == 0) return 0;

         CurrentHealth += healTaken;

         OnChanged.Invoke();
         OnRevived.Invoke();

         return healTaken;
      }

      public int Shield(int points)
      {
         if (IsDead) return 0;

         CurrentShield += points;

         OnChanged.Invoke();

         return points;
      }

      public void RemoveAllShields()
      {
         if (CurrentShield == 0) return;

         CurrentShield = 0;

         OnChanged.Invoke();
      }

      public void ChangeMax(int additional_health)
      {
         if (additional_health == 0) return;

         MaxHealth += additional_health;
         var currentHealthChange = Mathf.Clamp(additional_health, -CurrentHealth, MaxHealth - CurrentHealth);
         CurrentHealth += currentHealthChange;

         OnChanged.Invoke();

         if (currentHealthChange != 0)
         {
            if (IsAlive && CurrentHealth == currentHealthChange) OnRevived.Invoke();
            else if (IsDead) OnDied.Invoke();
         }
      }
   }
}