using UnityEngine;

namespace SDRL26.Rendering.Shared
{
   public class SmoothMover : MonoBehaviour
   {
      private const float SmoothTime = .1f;

      [SerializeField] private Transform _target;

      private Vector3 velocity;

      public Transform Target
      {
         get => _target;
         set => _target = value;
      }

      private void Update()
      {
         if (!_target) return;

         transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref velocity, SmoothTime);
      }
   }
}