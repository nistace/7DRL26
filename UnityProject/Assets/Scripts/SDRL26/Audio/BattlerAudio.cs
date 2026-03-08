using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Audio
{
   [RequireComponent(typeof(Battler))]
   [RequireComponent(typeof(AudioSource))]
   public class BattlerAudio : MonoBehaviour
   {
      [SerializeField] private GenericBattleAudioClips _genericClips;
      [SerializeField] private Battler _battler;
      [SerializeField] private AudioClip _defaultClip;
      [SerializeField] private AudioClip _hurtClip;
      [SerializeField] private AudioClip _deadClip;
      [SerializeField] private AudioSource _audioSource;

      private void Reset()
      {
         _battler = GetComponent<Battler>();
         _audioSource = GetComponent<AudioSource>();
      }

      private void Start() => TryPlay(_defaultClip);

      private void OnEnable()
      {
         _battler.Health.OnDamaged.AddListener(HandleDamaged);
         _battler.Health.OnDied.AddListener(HandleDied);
         _battler.OnPhaseChanged.AddListener(HandlePhaseChanged);
         _battler.OnPostureChanged.AddListener(HandlePostureChanged);
         _battler.OnActionsPerformed.AddListener(HandleActionsPerformed);
      }

      private void OnDisable()
      {
         if (!_battler) return;

         _battler.Health.OnDamaged.RemoveListener(HandleDamaged);
         _battler.Health.OnDied.RemoveListener(HandleDied);
         _battler.OnPhaseChanged.RemoveListener(HandlePhaseChanged);
         _battler.OnPostureChanged.RemoveListener(HandlePostureChanged);
         _battler.OnActionsPerformed.RemoveListener(HandleActionsPerformed);
      }

      private void HandleActionsPerformed() => TryPlay(_battler.Posture.PerformClip);

      private void HandlePhaseChanged(Battler.Phase newPhase)
      {
         if (newPhase is Battler.Phase.Action)
         {
            TryPlay(_battler.Posture.PrepareClip);
         }
      }

      private void HandleDamaged((bool health, bool shields) data)
      {
         if (!_battler.Health.IsAlive)
         {
            return;
         }

         if (data.health && TryPlay(_hurtClip)) return;

         if (data.shields) TryPlay(_genericClips.DamageToShield);
      }

      private void HandlePostureChanged(BattlerPosture arg0) => TryPlay(_defaultClip);
      private void HandleDied() => TryPlay(_deadClip);

      private bool TryPlay(AudioClip clip)
      {
         if (!clip) return false;

         _audioSource.PlayOneShot(clip);

         return true;
      }
   }
}