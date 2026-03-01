using System.Linq;
using SDRL26.Battles;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26
{
   [CreateAssetMenu]
   public class BattleSetup : ScriptableObject
   {
      [SerializeField] private Battler[] _opponentsPrefabs;

      private Battler[] InstantiateOpponents() => _opponentsPrefabs.Select(Instantiate).ToArray();

      public BattlerTeam InstantiateOpponentTeam() => new(InstantiateOpponents());
   }
}