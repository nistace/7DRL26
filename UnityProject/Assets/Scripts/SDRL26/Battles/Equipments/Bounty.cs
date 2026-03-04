using System;
using UnityEngine;

namespace SDRL26.Battles.Equipments
{
   [Serializable]
   public class Bounty
   {
      [SerializeField] private int _gold;
      [SerializeField] private Equipment[] _equipments;

      public int Gold => _gold;
      public Equipment[] Equipments => _equipments;
   }
}