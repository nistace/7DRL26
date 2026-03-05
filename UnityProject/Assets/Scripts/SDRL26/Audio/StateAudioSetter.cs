using System;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Audio
{
   public class StateAudioSetter : MonoBehaviour
   {
      [SerializeField] private AudioSource _source;
      [SerializeField] private StateDataList<TrackData> _dataPerState;

      private void Reset()
      {
         _source = GetComponent<AudioSource>();
         _dataPerState = new StateDataList<TrackData>(_source ? new(_source.clip, 2, _source.volume) : new());
      }

      private void Update()
      {
         var newTrackData = _dataPerState.First(GameState.CurrentState?.Types ?? GameStateTypes.MainMenu);

         if (_source.clip != newTrackData.Clip)
         {
            _source.volume = Mathf.MoveTowards(_source.volume, 0, newTrackData.FadeSpeed * Time.deltaTime);

            if (Mathf.Approximately(_source.volume, 0))
            {
               _source.clip = newTrackData.Clip;

               if (_source.clip)
               {
                  _source.Play();
               }
            }
         }
         else
         {
            _source.volume = Mathf.MoveTowards(_source.volume, newTrackData.Volume, newTrackData.FadeSpeed * Time.deltaTime);
         }
      }

      [Serializable]
      private class TrackData
      {
         [SerializeField] private AudioClip _clip;
         [SerializeField] private float _fadeSpeed = 2;
         [SerializeField] private float _volume = 1;

         public float FadeSpeed => _fadeSpeed;
         public AudioClip Clip => _clip;
         public float Volume => _volume;

         public TrackData(AudioClip clip, float fade_speed, float volume)
         {
            _clip = clip;
            _fadeSpeed = fade_speed;
            _volume = volume;
         }

         public TrackData() { }
      }
   }
}