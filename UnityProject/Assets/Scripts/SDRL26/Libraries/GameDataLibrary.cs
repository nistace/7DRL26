using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using SDRL26.Battles.Cards;
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
      [SerializeField] private AbilityCard[] _starterCards;
      [SerializeField] private int _pauseCardsCount = 3;
      [SerializeField] private int _timeBetweenInterruptions = 5;

      public IReadOnlyList<Battler> RandomStartBattlers => _playerStartBattlers.OrderBy(_ => Random.value).Take(_playerStartBattlersOptions).ToArray();
      public float MaxBattlerAdditionalPreparationTime => _maxBattlerAdditionalPreparationTime;
      public IReadOnlyList<AbilityCard> StarterCards => _starterCards;
      public int PauseCardsCount => _pauseCardsCount;
      public int TimeBetweenInterruptions => _timeBetweenInterruptions;

      public BattleSetup RandomBattleSetup(int level) => _battleSetupsPerLevel[level].RandomSetup;
   }
}