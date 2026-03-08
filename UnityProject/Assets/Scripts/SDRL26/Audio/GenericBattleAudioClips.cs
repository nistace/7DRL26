using UnityEngine;

namespace SDRL26.Audio
{
   [CreateAssetMenu]
   public class GenericBattleAudioClips : ScriptableObject
   {
      [SerializeField] private AudioClip _damageToShield;
      [SerializeField] private AudioClip _shielded;
      [SerializeField] private AudioClip _healed;

      public AudioClip DamageToShield => _damageToShield;
      public AudioClip Shielded => _shielded;
      public AudioClip Healed => _healed;
   }
}