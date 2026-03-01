using UnityEngine;

namespace SDRL26.Rendering.Battleground.HealthBars
{
   [CreateAssetMenu]
   public class HealthBarData : ScriptableObject
   {
      [SerializeField] private Color _fullColor = Color.white;
      [SerializeField] private Color _emptyColor = new Color(1, 1, 1, .5f);

      public Color FullColor => _fullColor;
      public Color EmptyColor => _emptyColor;
   }
}