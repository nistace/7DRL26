using System;
using System.Collections.Generic;
using System.Linq;
using SDRL26.Battles;
using SDRL26.Battles.Equipments;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SDRL26.Encounters
{
   [CreateAssetMenu]
   public class EncounterGenerator : ScriptableObject
   {
      [SerializeField] private int _options = 2;
      [SerializeField] private BattleSetup[] _possibleBattles;
      [SerializeField] private int _minBattles = 1;
      [SerializeField] private int _maxBattles = 1;
      [SerializeField] private MerchantGenerator[] _possibleMerchants;
      [SerializeField] private int _minMerchants;
      [SerializeField] private int _maxMerchants = 1;
      [SerializeField] private SorcererGenerator[] _possibleSorcerers;
      [SerializeField] private int _minSorcerers;
      [SerializeField] private int _maxSorcerers = 1;
      [SerializeField] private BlacksmithGenerator[] _possibleBlacksmiths;
      [SerializeField] private int _minBlacksmiths;
      [SerializeField] private int _maxBlacksmiths = 1;

      public EncounterChoice GenerateChoice(IReadOnlyList<Equipment> player_equipments)
      {
         var options = new List<IEncounter>();
         var moreOptions = new List<IEncounter>();

         AddMinAndAppendToMax(_possibleBattles, t => t, _minBattles, _maxBattles, ref options, ref moreOptions);
         AddMinAndAppendToMax(_possibleMerchants, t => t.GenerateMerchant(), _minMerchants, _maxMerchants, ref options, ref moreOptions);
         AddMinAndAppendToMax(_possibleSorcerers, t => t.GenerateSorcerer(), _minSorcerers, _maxSorcerers, ref options, ref moreOptions);

         var upgradableEquipments = player_equipments.Select(t => (origin: t, upgrade: t.GetComponent<EquipmentUpgrade>())).Where(t => t.upgrade && t.upgrade.Count > 0).ToArray();

         if (upgradableEquipments.Length > 0)
         {
            AddMinAndAppendToMax(_possibleBlacksmiths,
               t =>
               {
                  var upgrade = upgradableEquipments[Random.Range(0, upgradableEquipments.Length)];

                  return t.GenerateBlacksmith(upgrade.origin, upgrade.upgrade.RandomUpgrade);
               },
               _minBlacksmiths,
               _maxBlacksmiths,
               ref options,
               ref moreOptions
            );
         }

         moreOptions = moreOptions.OrderBy(_ => Random.value).ToList();

         while (moreOptions.Count > 0 && options.Count < _options)
         {
            options.Add(moreOptions[0]);
            moreOptions.RemoveAt(0);
         }

         return new EncounterChoice(options.ToArray());
      }

      private static void AddMinAndAppendToMax<T>(T[] item, Func<T, IEncounter> toEncounter, int min, int max, ref List<IEncounter> selectedOptions, ref List<IEncounter> remaining)
      {
         var optionsAdded = 0;
         var randomOptions = item.OrderBy(_ => Random.value).ToList();

         while (optionsAdded < min)
         {
            selectedOptions.Add(toEncounter(randomOptions[0]));
            randomOptions.RemoveAt(0);
            optionsAdded++;
         }

         remaining.AddRange(randomOptions.Take(max - min).Select(toEncounter));
      }
   }
}