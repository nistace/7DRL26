using UnityEngine;

namespace Utilities
{
   public static class NumbersExtension
   {
      public static bool ApproximatelyInt(this float f) => Mathf.Approximately(f, Mathf.RoundToInt(f));

      public static string ToStringOptionalDot(this float f) => f.ApproximatelyInt() ? $"{Mathf.RoundToInt(f)}" : $"{f:0.#}";
   }
}