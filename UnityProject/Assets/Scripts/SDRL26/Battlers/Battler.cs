using System;
using System.Collections.Generic;
using SDRL26;
using SDRL26.Battlers;
using SDRL26.Battlers.Actions;
using UnityEngine;
using UnityEngine.Events;

public class Battler : MonoBehaviour
{
    private enum Phase
    {
        Start = 0,
        Action = 1,
        Rest = 2,
    }

    [SerializeField] private string _displayName = "Battler";
    [SerializeField] private Health _health = new();
    [SerializeField] private ActionTarget _target = ActionTarget.EnemyAliveFirst;
    [SerializeField] private float _chargeActionTime = 1;
    [SerializeField] private float _restTime = 2;

    public string DisplayName => _displayName;
    public BattleAction[] Actions { get; set; }
    public Health Health => _health;
    private Phase CurrentPhase { get; set; }
    private float CurrentPhaseLoadUpTime { get; set; }
    private float StartTime { get; set; }
    public BattlerTeam Team { get; set; }
    public BattlerTeam OtherTeam { get; set; }
    public IReadOnlyCollection<Battler> Targets { get; private set; }

    public static UnityEvent<Battler> OnTargetsEvaluated { get; } = new();
    public static UnityEvent<Battler> OnActionsPerformed { get; } = new();

    private void Start()
    {
        Actions = GetComponentsInChildren<BattleAction>();
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
        Targets = _target switch
        {
            ActionTarget.Self => new[] { this },
            ActionTarget.AllyAliveFirst => Team.GetFirst(t => t._health.IsAlive),
            ActionTarget.AllyAliveLast => Team.GetLast(t => t._health.IsAlive),
            ActionTarget.EnemyAliveFirst => OtherTeam.GetFirst(t => t._health.IsAlive),
            ActionTarget.EnemyAliveLast => OtherTeam.GetLast(t => t._health.IsAlive),
            _ => throw new ArgumentOutOfRangeException()
        };

        OnTargetsEvaluated.Invoke(this);
    }

    public int Damage(int damage) => _health.Damage(damage);
}