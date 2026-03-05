using UnityEngine;

namespace SDRL26.Encounters
{
   public interface IEncounter
   {
      Sprite Portrait { get; }
      string DisplayName { get; }
      string Description { get; }
   }
}