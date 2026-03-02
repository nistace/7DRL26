using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Libraries
{
   [CreateAssetMenu]
   public class GameDataLibrary : ScriptableObject
   {
      public static GameDataLibrary Instance { get; set; }

      [SerializeField] private Battler[] _playerStartBattlers;
      [SerializeField] private int _playerStartBattlersOptions;
      [SerializeField] private GameLevelBattleSetup[] _battleSetupsPerLevel;
      [SerializeField] private float _maxBattlerAdditionalPreparationTime = 1;

      public IReadOnlyList<Battler> RandomStartBattlers => _playerStartBattlers.OrderBy(_ => Random.value).Take(_playerStartBattlersOptions).ToArray();
      public float MaxBattlerAdditionalPreparationTime => _maxBattlerAdditionalPreparationTime;

      public BattleSetup RandomBattleSetup(int level) => _battleSetupsPerLevel[level].RandomSetup;
   }
}