using System;
using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Actions;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Battles.Battlers
{
   public class Battler : MonoBehaviour, IActionPerformer
   {
      public enum Phase
      {
         Prepare = 0,
         Action = 1,
         Rest = 2,
      }

      [SerializeField] private string _displayName = "Battler";
      [SerializeField] private Sprite _portrait;
      [SerializeField] private Health _health = new();
      [SerializeField] private BattlerPosture[] _postures;

      public string DisplayName => _displayName;
      public Sprite Portrait => _portrait;
      private BattleAction[] _actions;
      public Health Health => _health;
      public Phase CurrentPhase { get; private set; }
      private int PostureIndex { get; set; }
      public IReadOnlyList<BattlerPosture> Postures => _postures;
      public BattlerPosture Posture => Postures[PostureIndex];
      private float CurrentPhaseLoadUpTime { get; set; }
      public BattlerTeam Team { get; set; }
      public BattlerTeam OtherTeam { get; set; }
      public IReadOnlyCollection<Battler> Targets { get; private set; }
      public ActionTarget Target => Posture.Target;

      public float CurrentLoadRatio => Mathf.Clamp01(CurrentPhaseLoadUpTime
         / Mathf.Max(.001f,
            CurrentPhaseLoadUpTime,
            CurrentPhase switch
            {
               Phase.Prepare => Posture.PreparationTime,
               Phase.Action => Posture.ChargeActionTime,
               Phase.Rest => Posture.RestTime,
               _ => throw new ArgumentOutOfRangeException()
            }
         )
      );

      public static UnityEvent<Battler> OnTargetsChanged { get; } = new();
      public static UnityEvent<Battler> OnTargetsEvaluated { get; } = new();
      public static UnityEvent<Battler> OnActionsPerformed { get; } = new();
      public static UnityEvent<Battler> OnPhaseChanged { get; } = new();
      public static UnityEvent<Battler> OnAliveChanged { get; } = new();
      public UnityEvent<BattlerPosture> OnPostureChanged { get; } = new();

      public void Initialize()
      {
         Health.FullyHeal();
      }

      private void OnEnable()
      {
         Health.OnDied.AddListener(HandleDied);
      }

      private void OnDisable()
      {
         Health.OnDied.RemoveListener(HandleDied);
      }

      private void HandleDied() => OnAliveChanged.Invoke(this);

      public void ContinueBattle(float deltaTime)
      {
         if (_health.IsDead) return;

         CurrentPhaseLoadUpTime += deltaTime;

         switch (CurrentPhase)
         {
            case Phase.Action when CurrentPhaseLoadUpTime >= Posture.ChargeActionTime:
               ExecuteActions();
               ChangePhase(Phase.Rest);

               break;
            case Phase.Prepare when CurrentPhaseLoadUpTime >= Posture.PreparationTime:
            case Phase.Rest when CurrentPhaseLoadUpTime >= Posture.RestTime:
               RefreshTargets();
               ChangePhase(Phase.Action);

               break;
         }
      }

      public void PrepareForBattle(float additionalPreparationTime)
      {
         ChangePhase(Phase.Prepare);
         CurrentPhaseLoadUpTime = -additionalPreparationTime;
      }

      private void ExecuteActions()
      {
         foreach (var action in Posture.Actions)
         {
            action.ApplyEffect(this, Targets);
         }

         OnActionsPerformed.Invoke(this);
      }

      private void ChangePhase(Phase newPhase)
      {
         CurrentPhase = newPhase;
         CurrentPhaseLoadUpTime = 0;
         OnPhaseChanged.Invoke(this);
      }

      private void RefreshTargets()
      {
         SetTargets(EvaluateTargets());
         OnTargetsEvaluated.Invoke(this);
      }

      private IReadOnlyCollection<Battler> EvaluateTargets() => EvaluateTargets(Target);

      public IReadOnlyCollection<Battler> EvaluateTargets(ActionTarget target) => target switch
      {
         ActionTarget.Self => new[] { this },
         ActionTarget.FirstAlly => Team.GetFirst(t => t._health.IsAlive),
         ActionTarget.LastAlly => Team.GetLast(t => t._health.IsAlive),
         ActionTarget.FirstEnemy => OtherTeam.GetFirst(t => t._health.IsAlive),
         ActionTarget.LastEnemy => OtherTeam.GetLast(t => t._health.IsAlive),
         ActionTarget.AllyWithLowestHealth => Team.GetFirst(t => t.Health.IsAlive, t => t.Health.CurrentHealth),
         ActionTarget.EnemyWithLowestHealth => OtherTeam.GetFirst(t => t.Health.IsAlive, t => t.Health.CurrentHealth),
         ActionTarget.RandomAlly => Team.GetRandom(t => t.Health.IsAlive),
         ActionTarget.RandomEnemy => OtherTeam.GetRandom(t => t.Health.IsAlive),
         ActionTarget.FirstDeadAlly => Team.GetFirst(t => t.Health.IsDead),
         ActionTarget.AllyWithMostMissingHealth => Team.GetFirst(t => t.Health.IsAlive, t => -t.Health.MissingHealth),
         ActionTarget.EnemyWithMostMissingHealth => OtherTeam.GetFirst(t => t.Health.IsAlive, t => -t.Health.MissingHealth),
         ActionTarget.EnemyWithMostHealth => OtherTeam.GetFirst(t => t.Health.IsAlive, t => -t.Health.CurrentHealth),
         ActionTarget.ÀllyWithLowestHealthRatio => Team.GetFirst(t => t.Health.IsAlive, t => t.Health.Ratio),
         ActionTarget.EnemyWithLowestHealthRatio => OtherTeam.GetFirst(t => t.Health.IsAlive, t => t.Health.Ratio),
         _ => throw new ArgumentOutOfRangeException()
      };

      public void SetTargets(IReadOnlyCollection<Battler> battlers)
      {
         Targets = battlers.ToArray();

         OnTargetsChanged.Invoke(this);
      }

      public int Damage(int damage) => _health.Damage(damage);
      public int Heal(int points, bool canRevive) => _health.Heal(points, canRevive);
      public int Revive(float newRatio) => _health.Revive(newRatio);
      public int Shield(int points) => _health.Shield(points);

      public void SelectNextPosture() => SelectPosture(PostureIndex + 1);

      public void SelectPosture(BattlerPosture posture)
      {
         var index = Array.IndexOf(_postures, posture);

         if (index >= 0)
         {
            SelectPosture(index);
         }
      }

      private void SelectPosture(int index)
      {
         var newIndex = (index + _postures.Length) % _postures.Length;

         if (PostureIndex == index) return;

         PostureIndex = newIndex;
         ChangePhase(Phase.Prepare);
         OnPostureChanged.Invoke(Posture);
      }

      public void ForceRest() => ChangePhase(Phase.Rest);

      public void EndRest(bool orPreparation)
      {
         if (CurrentPhase == Phase.Action) return;
         if (CurrentPhase == Phase.Prepare && !orPreparation) return;

         RefreshTargets();
         ChangePhase(Phase.Action);
      }

      public void ProgressCurrentPhaseLoadUpTime(float progress) => CurrentPhaseLoadUpTime += progress;

      public void ResetForBattle()
      {
         Health.RemoveAllShields();
      }
   }
}