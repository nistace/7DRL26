using System;
using System.Linq;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Audio
{
   public class StateAudioSetter : MonoBehaviour
   {
      [SerializeField] private AudioSource _source;

      [SerializeField] private TrackData[] _clipsPerState;
      [SerializeField] private AudioClip _defaultClip;
      [SerializeField] private float _defaultFadeSpeed = 2;
      [SerializeField] private float _defaultVolume;

      private void Reset()
      {
         _source = GetComponent<AudioSource>();
         _defaultVolume = _source ? _source.volume : 1;
      }

      private void Update()
      {
         var types = GameState.CurrentState?.Types ?? GameStateTypes.MainMenu;

         var newTrackData = _clipsPerState.FirstOrDefault(t => t.Targets(types));
         var clip = newTrackData?.Clip ?? _defaultClip;
         var fadeSpeed = newTrackData?.FadeSpeed ?? _defaultFadeSpeed;
         var volume = newTrackData?.Volume ?? _defaultVolume;

         if (_source.clip != clip)
         {
            _source.volume = Mathf.MoveTowards(_source.volume, 0, fadeSpeed * Time.deltaTime);

            if (Mathf.Approximately(_source.volume, 0))
            {
               _source.clip = clip;

               if (_source.clip)
               {
                  _source.Play();
               }
            }
         }
         else
         {
            _source.volume = Mathf.MoveTowards(_source.volume, volume, fadeSpeed * Time.deltaTime);
         }
      }

      [Serializable]
      private class TrackData
      {
         [SerializeField] private GameStateTypes _states;
         [SerializeField] private AudioClip _clip;
         [SerializeField] private float _fadeSpeed = 2;
         [SerializeField] private float _volume = 1;

         public float FadeSpeed => _fadeSpeed;
         public AudioClip Clip => _clip;
         public float Volume => _volume;

         public bool Targets(GameStateTypes state) => _states == state;
      }
   }
}