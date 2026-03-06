using UnityEngine;

namespace SDRL26.Rendering.Battleground.HealthBars
{
   [CreateAssetMenu]
   public class HealthBarData : ScriptableObject
   {
      [SerializeField] private Sprite _fullSprite;
      [SerializeField] private Sprite _emptySprite;
      [SerializeField] private Color _fullColor = Color.white;
      [SerializeField] private Color _emptyColor = new Color(1, 1, 1, .5f);
      [SerializeField] private Sprite _shieldSprite;

      public Sprite FullSprite => _fullSprite;
      public Sprite EmptySprite => _emptySprite;
      public Color FullColor => _fullColor;
      public Color EmptyColor => _emptyColor;
      public Sprite ShieldSprite => _shieldSprite;
   }
}