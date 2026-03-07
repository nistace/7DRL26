using System;
using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles.Battlers;
using SDRL26.Battles.Cards;
using SDRL26.Battles.Equipments;
using SDRL26.Encounters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SDRL26.Libraries
{
   [CreateAssetMenu]
   public class GameDataLibrary : ScriptableObject
   {
      public static GameDataLibrary Instance { get; set; }

      [Header("Pick battlers")]
      [SerializeField] private Battler[] _playerStartBattlers;

      [SerializeField] private int _battlersToPickOnStart = 1;
      [SerializeField] private int _optionsPerBattlerPick = 3;
      [SerializeField] private Battler[] _allPlayerBattlers;
      [SerializeField] private int[] _chooseHeroesInLevels = { 4, 8, 12, 16 };

      [Header("Encounters")]
      [SerializeField] private EncounterGenerator[] _levelEncounters;

      [SerializeField] private float _maxBattlerAdditionalPreparationTime = 1;
      [SerializeField] private int _timeBetweenInterruptions = 5;
      [SerializeField] private bool _canReorganizeDuringPauses = true;

      [Header("Ability Cards")]
      [SerializeField] private AbilityCard[] _starterCards;

      [SerializeField] private int _pauseCardsCount = 3;

      public IReadOnlyList<Battler> RandomStartBattlers => _playerStartBattlers.OrderBy(_ => Random.value).Take(_optionsPerBattlerPick).ToArray();
      public IReadOnlyList<Battler> RandomBattlers => _allPlayerBattlers.OrderBy(_ => Random.value).Take(_optionsPerBattlerPick).ToArray();
      public float MaxBattlerAdditionalPreparationTime => _maxBattlerAdditionalPreparationTime;
      public IReadOnlyList<AbilityCard> StarterCards => _starterCards;
      public int PauseCardsCount => _pauseCardsCount;
      public int TimeBetweenInterruptions => _timeBetweenInterruptions;
      public int BattlersToPickOnStart => _battlersToPickOnStart;
      public bool CanReorganizeDuringPauses => _canReorganizeDuringPauses;
      public int Levels => _levelEncounters.Length;

      public EncounterChoice RandomEncounterChoice(int level, IReadOnlyList<Equipment> playerEquipments) => _levelEncounters[level].GenerateChoice(playerEquipments);
      public bool HasToChooseHero(int level) => Array.IndexOf(_chooseHeroesInLevels, level + 1) >= 0;
      public bool IsLastLevelOrBeyond(int level) => level >= Levels - 1;
   }
}