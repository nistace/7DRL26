using UnityEngine;

namespace SDRL26.Rendering.Battleground
{
   [CreateAssetMenu]
   public class BattlerTokenStyle : ScriptableObject
   {
      [SerializeField] private Color _fillActionColor = Color.white;
      [SerializeField] private Color _fillRestColor = Color.red;

      public Color FillActionColor => _fillActionColor;
      public Color FillRestColor => _fillRestColor;
   }
}