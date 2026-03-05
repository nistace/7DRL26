using System;
using UnityEngine;

namespace SDRL26.GameControllers.GameStates
{
   [Serializable]
   public class StateDataList<T>
   {
      [SerializeField] private StateData<T>[] _items;
      [SerializeField] private T _default;

      public StateDataList(T defaultValue)
      {
         _default = defaultValue;
      }

      public T First(GameStateTypes types)
      {
         foreach (var item in _items)
         {
            if (item.Is(types))
            {
               return item.Data;
            }
         }

         return _default;
      }
   }
}