using System;
using System.Linq;
using SDRL26.Battles.Battlers;
using SDRL26.Battles.Equipments;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace SDRL26.Battles
{
   [Serializable]
   public class Battle
   {
      [SerializeField] private BattlerTeam _playerTeam;
      [SerializeField] private BattlerTeam _opponentTeam;
      [SerializeField] private Bounty _bounty;

      public float BattleTime { get; private set; }
      public BattlerTeam PlayerTeam => _playerTeam;
      public BattlerTeam OpponentTeam => _opponentTeam;
      public Bounty Bounty => _bounty;

      public UnityEvent OnStarted { get; } = new();

      public Battle(BattlerTeam playerTeam, BattlerTeam opponentTeam, Bounty bounty)
      {
         _playerTeam = playerTeam;
         _opponentTeam = opponentTeam;
         _bounty = bounty;

         foreach (var battler in playerTeam.Battlers)
         {
            battler.Team = playerTeam;
            battler.OtherTeam = opponentTeam;
         }

         foreach (var battler in opponentTeam.Battlers)
         {
            battler.Team = opponentTeam;
            battler.OtherTeam = playerTeam;
         }
      }

      public void Prepare(float maxAdditionalPreparationTime)
      {
         foreach (var battler in _playerTeam.Battlers.Union(_opponentTeam.Battlers))
         {
            battler.PrepareForBattle(Random.value * maxAdditionalPreparationTime);
         }

         OnStarted.Invoke();
      }

      public void Continue(float deltaTime)
      {
         _playerTeam.ContinueBattle(deltaTime);
         _opponentTeam.ContinueBattle(deltaTime);
         BattleTime += deltaTime;
      }

      public bool IsOver()
      {
         return _playerTeam.Battlers.All(t => t.Health.IsDead) || _opponentTeam.Battlers.All(t => t.Health.IsDead);
      }

      public bool IsInPlayerTeam(Battler battler) => _playerTeam.IsInTeam(battler);

      public float GetNextPauseTime(int timeBetweenInterruptions) =>
         timeBetweenInterruptions * Mathf.RoundToInt((BattleTime + timeBetweenInterruptions) / timeBetweenInterruptions);
   }
}