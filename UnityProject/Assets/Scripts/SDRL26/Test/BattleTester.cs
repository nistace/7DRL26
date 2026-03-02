using SDRL26.Battles;
using SDRL26.Battles.Battlers;
using UnityEngine;
using UnityEngine.Events;

namespace SDRL26.Test
{
   public class BattleTester : MonoBehaviour
   {
      [SerializeField] private Battler[] _playerBattlers;
      [SerializeField] private Battler[] _opponentBattlers;

      public Battle Battle { get; private set; }
      public float BattleStartTime { get; private set; }
      public float BattleTime => Time.time - BattleStartTime;
      public UnityEvent OnBattleInitialized { get; } = new();

      private void Start()
      {
         BattleStartTime = Time.time;
         Battle = new Battle(new BattlerTeam(_playerBattlers), new BattlerTeam(_opponentBattlers));
         Battle.Prepare(1);
         OnBattleInitialized.Invoke();
      }

      public bool IsBattleInitialized() => Battle != null;

      private void Update()
      {
         if (!Battle.IsOver())
         {
            Battle.Continue(Time.deltaTime);
         }
      }
   }
}