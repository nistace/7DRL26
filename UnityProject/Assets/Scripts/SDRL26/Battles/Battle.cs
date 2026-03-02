using System;
using System.Linq;
using SDRL26.Battles.Battlers;
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

      public BattlerTeam PlayerTeam => _playerTeam;
      public BattlerTeam OpponentTeam => _opponentTeam;

      public UnityEvent OnStarted { get; } = new();

      public Battle(BattlerTeam playerTeam, BattlerTeam opponentTeam)
      {
         _playerTeam = playerTeam;
         _opponentTeam = opponentTeam;

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
      }

      public bool IsOver()
      {
         return _playerTeam.Battlers.All(t => t.Health.IsDead) || _opponentTeam.Battlers.All(t => t.Health.IsDead);
      }

      public bool IsInPlayerTeam(Battler battler) => _playerTeam.IsInTeam(battler);
   }
}