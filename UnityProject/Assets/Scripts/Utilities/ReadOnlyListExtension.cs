using System;
using System.Collections.Generic;

namespace Utilities
{
   public static class ReadOnlyListExtension
   {
      public static int IndexOf<T>(this IReadOnlyList<T> list, T item) => list.IndexOf(t => ReferenceEquals(t, item));

      public static int IndexOf<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
      {
         for (int i = 0; i < list.Count; i++)
         {
            if (predicate(list[i]))
            {
               return i;
            }
         }

         return -1;
      }
   }
}