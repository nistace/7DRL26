using UnityEngine;

namespace SDRL26.Rendering.Battleground
{
   [CreateAssetMenu]
   public class BattlerTokenStyle : ScriptableObject
   {
      [SerializeField] private Color _fillActionColor = Color.white;
      [SerializeField] private Color _fillRestColor = Color.red;
      [SerializeField] private Color _defaultActionColor = Color.black;
      [SerializeField] private Color _inactiveActionColor = Color.gray;
      [SerializeField, Range(0f, 1f)] private float _deadOpacity = .3f;
      [SerializeField, Range(0f, 1f)] private float _restOpacity = .7f;
      [SerializeField, Range(0f, 1f)] private float _defaultOpacity = 1;

      public Color FillActionColor => _fillActionColor;
      public Color FillRestColor => _fillRestColor;
      public Color DefaultActionColor => _defaultActionColor;
      public Color InactiveActionColor => _inactiveActionColor;
      public float DeadOpacity => _deadOpacity;
      public float RestOpacity => _restOpacity;
      public float DefaultOpacity => _defaultOpacity;
   }
}