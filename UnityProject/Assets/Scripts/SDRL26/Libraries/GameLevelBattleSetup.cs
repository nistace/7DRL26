using System;
using SDRL26.Battles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SDRL26.Libraries
{
   [Serializable]
   public class GameLevelBattleSetup
   {
      [SerializeField] private BattleSetup[] _setups;

      public BattleSetup RandomSetup => _setups[Random.Range(0, _setups.Length)];
   }
}