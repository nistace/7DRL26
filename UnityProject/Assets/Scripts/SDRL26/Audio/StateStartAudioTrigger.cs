using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Audio
{
   public class StateStartAudioTrigger : MonoBehaviour
   {
      [SerializeField] private AudioSource _source;
      [SerializeField] private StateDataList<AudioClip> _audioClips;

      private void Start()
      {
         GameState.OnStateChanged.AddListener(HandleStateChanged);
      }

      private void OnDestroy()
      {
         GameState.OnStateChanged.RemoveListener(HandleStateChanged);
      }

      private void HandleStateChanged(GameState state)
      {
         var clip = _audioClips.First(state.Types);

         if (clip != null)
         {
            _source.PlayOneShot(clip);
         }
      }
   }
}