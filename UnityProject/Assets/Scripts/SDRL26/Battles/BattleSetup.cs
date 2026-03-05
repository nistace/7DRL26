using SDRL26.Battles.Battlers;
using SDRL26.Battles.Equipments;
using UnityEngine;

namespace SDRL26.Battles
{
   [CreateAssetMenu]
   public class BattleSetup : ScriptableObject
   {
      [SerializeField] private Battler[] _opponentsPrefabs;
      [SerializeField] private BountyGenerator[] _bountyGenerators;

      public BattlerTeam InstantiateOpponentTeam() => new(_opponentsPrefabs);
      public Bounty RandomBounty => _bountyGenerators[Random.Range(0, _bountyGenerators.Length)].RandomBounty();
   }
}