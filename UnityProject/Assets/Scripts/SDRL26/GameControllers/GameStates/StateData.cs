using System;
using UnityEngine;

namespace SDRL26.GameControllers.GameStates
{
   [Serializable]
   public class StateData<T>
   {
      [SerializeField] private GameStateTypes _types;
      [SerializeField] private T _data;

      public T Data => _data;

      public StateData(GameStateTypes types, T data)
      {
         _types = types;
         _data = data;
      }

      public bool Is(GameStateTypes types) => (_types & types) > 0;
   }
}