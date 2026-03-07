using AYellowpaper.SerializedCollections;
using SDRL26.Libraries;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Shared.Portraits
{
   public class Portrait : MonoBehaviour
   {
      [SerializeField] private SerializedDictionary<PortraitSize, Image> _imagePerSize;

      public void Set(PortraitSize size, Sprite sprite)
      {
         foreach (var image in _imagePerSize)
         {
            image.Value.enabled = size == image.Key;
            image.Value.sprite = sprite;
         }
      }
   }
}