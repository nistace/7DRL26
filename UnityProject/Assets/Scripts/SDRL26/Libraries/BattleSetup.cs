using SDRL26.Battles;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Libraries
{
   [CreateAssetMenu]
   public class BattleSetup : ScriptableObject
   {
      [SerializeField] private Battler[] _opponentsPrefabs;

      public BattlerTeam InstantiateOpponentTeam() => new(_opponentsPrefabs);
   }
}