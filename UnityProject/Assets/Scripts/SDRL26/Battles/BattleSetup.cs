using SDRL26.Battles.Battlers;
using SDRL26.Battles.Equipments;
using SDRL26.Encounters;
using UnityEngine;

namespace SDRL26.Battles
{
   [CreateAssetMenu]
   public class BattleSetup : ScriptableObject, IEncounter
   {
      [SerializeField] private Sprite _portrait;
      [SerializeField] private string _displayName = "A group of foes";
      [SerializeField] private string _description = "These... creatures... Looks like they want to fight!";
      [SerializeField] private Battler[] _opponentsPrefabs;
      [SerializeField] private BountyGenerator[] _bountyGenerators;

      public Sprite Portrait => _portrait;
      public string DisplayName => _displayName;
      public string Description => _description;

      public BattlerTeam InstantiateOpponentTeam() => new(_opponentsPrefabs);
      public Bounty RandomBounty => _bountyGenerators[Random.Range(0, _bountyGenerators.Length)].RandomBounty();
   }
}