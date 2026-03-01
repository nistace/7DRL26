using System;
using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Actions;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Battles.Battlers
{
   public class Battler : MonoBehaviour
   {
      public enum Phase
      {
         Start = 0,
         Action = 1,
         Rest = 2,
      }

      [SerializeField] private string _displayName = "Battler";
      [SerializeField] private Sprite _portrait;
      [SerializeField] private Health _health = new();
      [SerializeField] private ActionTarget _target = ActionTarget.FirstEnemy;
      [SerializeField] private float _chargeActionTime = 1;
      [SerializeField] private float _restTime = 2;

      public string DisplayName => _displayName;
      public Sprite Portrait => _portrait;
      private BattleAction[] _actions;
      public BattleAction[] Actions => _actions ??= GetComponentsInChildren<BattleAction>();
      public Health Health => _health;
      public Phase CurrentPhase { get; private set; }
      public float ChargeActionTime => _chargeActionTime;
      public float RestTime => _restTime;
      private float CurrentPhaseLoadUpTime { get; set; }
      private float StartTime { get; set; } = 1;
      public BattlerTeam Team { get; set; }
      public BattlerTeam OtherTeam { get; set; }
      public IReadOnlyCollection<Battler> Targets { get; private set; }
      public ActionTarget Target => _target;

      public float CurrentLoadRatio => Mathf.Clamp01(CurrentPhaseLoadUpTime
         / Mathf.Max(.001f,
            CurrentPhaseLoadUpTime,
            CurrentPhase switch
            {
               Phase.Start => StartTime,
               Phase.Action => _chargeActionTime,
               Phase.Rest => _restTime,
               _ => throw new ArgumentOutOfRangeException()
            }
         )
      );

      public static UnityEvent<Battler> OnTargetsChanged { get; } = new();
      public static UnityEvent<Battler> OnTargetsEvaluated { get; } = new();
      public static UnityEvent<Battler> OnActionsPerformed { get; } = new();

      public void Initialize()
      {
         Health.FullyHeal();
      }

      public void ContinueBattle(float deltaTime)
      {
         if (_health.IsDead) return;

         CurrentPhaseLoadUpTime += deltaTime;

         switch (CurrentPhase)
         {
            case Phase.Action when CurrentPhaseLoadUpTime >= _chargeActionTime:
               ExecuteActions();
               ChangePhase(Phase.Rest);

               break;
            case Phase.Start when CurrentPhaseLoadUpTime >= StartTime:
            case Phase.Rest when CurrentPhaseLoadUpTime >= _restTime:
               RefreshTargets();
               ChangePhase(Phase.Action);

               break;
         }
      }

      public void PrepareForBattle(float startTime)
      {
         ChangePhase(Phase.Start);
         StartTime = startTime;
      }

      private void ExecuteActions()
      {
         foreach (var action in Actions)
         {
            action.ApplyEffect(this, Targets);
         }

         OnActionsPerformed.Invoke(this);
      }

      private void ChangePhase(Phase newPhase)
      {
         CurrentPhase = newPhase;
         CurrentPhaseLoadUpTime = 0;
      }

      private void RefreshTargets()
      {
         SetTargets(_target switch
            {
               ActionTarget.Self => new[] { this },
               ActionTarget.FirstAlly => Team.GetFirst(t => t._health.IsAlive),
               ActionTarget.LastAlly => Team.GetLast(t => t._health.IsAlive),
               ActionTarget.FirstEnemy => OtherTeam.GetFirst(t => t._health.IsAlive),
               ActionTarget.LastEnemy => OtherTeam.GetLast(t => t._health.IsAlive),
               ActionTarget.AllyWithLowestHealth => Team.GetFirst(t => t.Health.CurrentHealth == Team.LowestAliveHealth),
               ActionTarget.EnemyWithLowestHealth => OtherTeam.GetFirst(t => t.Health.CurrentHealth == OtherTeam.LowestAliveHealth),
               ActionTarget.RandomAlly => Team.GetRandom(t => t.Health.IsAlive),
               ActionTarget.RandomEnemy => OtherTeam.GetRandom(t => t.Health.IsAlive),
               _ => throw new ArgumentOutOfRangeException()
            }
         );

         OnTargetsEvaluated.Invoke(this);
      }

      public void SetTargets(IReadOnlyCollection<Battler> battlers)
      {
         Targets = battlers.ToArray();

         OnTargetsChanged.Invoke(this);
      }

      public int Damage(int damage) => _health.Damage(damage);
      public int Heal(int points) => _health.Heal(points);
      public int Shield(int points) => _health.Shield(points);
   }
}