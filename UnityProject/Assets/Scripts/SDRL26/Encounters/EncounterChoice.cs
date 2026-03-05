using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace SDRL26.Encounters
{
   [Serializable]
   public class EncounterChoice
   {
      private IEncounter[] _options;

      public IReadOnlyList<IEncounter> Options => _options;

      public EncounterChoice(IEncounter[] options)
      {
         _options = options.OrderBy(_ => Random.value).ToArray();
      }
   }
}