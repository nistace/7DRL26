using SDRL26.Battles.Battlers;
using SDRL26.Battles.Equipments;
using UnityEngine;

namespace SDRL26.Battles
{
   [CreateAssetMenu]
   public class BattleSetup : ScriptableObject
   {
      [SerializeField] private Battler[] _opponentsPrefabs;
      [SerializeField] private Bounty[] _bounties;

      public BattlerTeam InstantiateOpponentTeam() => new(_opponentsPrefabs);
      public Bounty RandomBounty => _bounties[Random.Range(0, _bounties.Length)];
   }
}