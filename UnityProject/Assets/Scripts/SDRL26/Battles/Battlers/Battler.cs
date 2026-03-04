using System;
using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Actions;
using SDRL26.Battles.Equipments;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Battles.Battlers
{
   public class Battler : MonoBehaviour, IActionPerformer
   {
      private const int DefaultEquipmentSlots = 2;

      public enum Phase
      {
         Prepare = 0,
         Action = 1,
         Rest = 2,
      }

      [SerializeField] private string _displayName = "Battler";
      [SerializeField] private Health _health = new();
      [SerializeField] private BattlerPosture[] _postures;
      [SerializeField] private Equipment[] _equipmentSlots = new Equipment[DefaultEquipmentSlots];

      public string DisplayName => _displayName;
      private BattleAction[] _actions;
      public Health Health => _health;
      public Phase CurrentPhase { get; private set; }
      private int PostureIndex { get; set; }
      public IReadOnlyList<BattlerPosture> Postures => _postures;
      public BattlerPosture Posture => Postures[PostureIndex];
      private float CurrentPhaseLoadUpTime { get; set; }
      public BattlerTeam Team { get; set; }
      public BattlerTeam OtherTeam { get; set; }
      public bool IsSummoned { get; set; }
      public Battler Target { get; private set; }
      public ActionTarget TargetChoice => Posture.Target;
      public int AdditionalTargets { get; set; }
      public int TotalAdditionalTargets => AdditionalTargets + Posture.AdditionalTargets;
      public IReadOnlyList<Equipment> Equipments => _equipmentSlots;

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
      public UnityEvent<(int index, Equipment equipment)> OnEquipmentChanged { get; } = new();

      public void Initialize()
      {
         Health.FullyHeal();

         foreach (var posture in Postures)
         {
            posture.RefreshActions();
         }

         foreach (var equipment in Equipments)
         {
            if (equipment)
            {
               equipment.Equip(this);
            }
         }
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
         ActionResolver.Resolve(Posture.Actions, this, Target, TotalAdditionalTargets);

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
         SetTarget(EvaluateTarget());
         OnTargetsEvaluated.Invoke(this);
      }

      private Battler EvaluateTarget() => EvaluateTarget(TargetChoice);

      public Battler EvaluateTarget(ActionTarget target) => target switch
      {
         ActionTarget.Self => this,
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

      public void SetTarget(Battler battler)
      {
         Target = battler;

         OnTargetChanged.Invoke(this);
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
         ChangePhase(Phase.Prepare);
      }

      public Sprite GetCurrentPortrait()
      {
         if (Health.IsDead)
         {
            return Posture.GetPortrait(BattlerPosture.Portrait.Dead);
         }

         if (CurrentPhase is Phase.Action)
         {
            return Posture.GetPortrait(BattlerPosture.Portrait.Action);
         }

         return Posture.GetPortrait(BattlerPosture.Portrait.Rest);
      }

      public void AddEquipment(int index, Equipment equipment)
      {
         if (index < 0 || index >= _equipmentSlots.Length) return;
         if (equipment == null) return;
         if (_equipmentSlots[index]) return;

         _equipmentSlots[index] = equipment;
         equipment.Equip(this);

         OnEquipmentChanged.Invoke((index, equipment));
      }

      public bool TryRemoveEquipment(int index, out Equipment equipment)
      {
         equipment = default;

         if (index < 0 || index >= _equipmentSlots.Length) return false;
         if (!_equipmentSlots[index]) return false;

         equipment = _equipmentSlots[index];
         equipment.Unequip();
         _equipmentSlots[index] = null;

         OnEquipmentChanged.Invoke((index, null));

         return true;
      }
   }
}