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
      [SerializeField] private int _chooseBattlersOptions = 3;
      [SerializeField] private Battler[] _allPlayerBattlers;
      [SerializeField] private GameLevelBattleSetup[] _battleSetupsPerLevel;
      [SerializeField] private float _maxBattlerAdditionalPreparationTime = 1;
      [SerializeField] private AbilityCard[] _starterCards;
      [SerializeField] private int _pauseCardsCount = 3;
      [SerializeField] private int _timeBetweenInterruptions = 5;
      [SerializeField] private int _chooseHeroOnLevelMultiples = 4;

      public IReadOnlyList<Battler> RandomStartBattlers => _playerStartBattlers.OrderBy(_ => Random.value).Take(_chooseBattlersOptions).ToArray();
      public IReadOnlyList<Battler> RandomBattlers => _allPlayerBattlers.OrderBy(_ => Random.value).Take(_chooseBattlersOptions).ToArray();
      public float MaxBattlerAdditionalPreparationTime => _maxBattlerAdditionalPreparationTime;
      public IReadOnlyList<AbilityCard> StarterCards => _starterCards;
      public int PauseCardsCount => _pauseCardsCount;
      public int TimeBetweenInterruptions => _timeBetweenInterruptions;

      public BattleSetup RandomBattleSetup(int level) => _battleSetupsPerLevel[level].RandomSetup;
      public bool HasToChooseHero(int level) => level % _chooseHeroOnLevelMultiples == 0;
   }
}