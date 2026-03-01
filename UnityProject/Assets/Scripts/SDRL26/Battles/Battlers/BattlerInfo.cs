using UnityEngine;

namespace SDRL26.Battles.Battlers
{
   [CreateAssetMenu]
   public class BattlerInfo : ScriptableObject
   {
      [SerializeField] private Battler _prefab;

      public string DisplayName => _prefab.DisplayName;
   }
}