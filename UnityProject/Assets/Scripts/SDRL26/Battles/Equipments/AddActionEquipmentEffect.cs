using System.Collections.Generic;
using SDRL26.Battles.Actions;
using SDRL26.Battles.Battlers;
using UnityEngine;

namespace SDRL26.Battles.Equipments
{
   public class AddActionEquipmentEffect : EquipmentEffect
   {
      [SerializeField] private BattleAction _action;

      private readonly HashSet<(BattlerPosture posture, BattleAction action)> _addedActions = new();

      public override void Apply(Battler battler)
      {
         Undo();

         foreach (var posture in battler.Postures)
         {
            _addedActions.Add((posture, Instantiate(_action, posture.transform)));
            posture.RefreshActions();
         }
      }

      public override void Undo()
      {
         foreach (var _addedAction in _addedActions)
         {
            Destroy(_addedAction.action.gameObject);
            _addedAction.posture.RefreshActions();
         }

         _addedActions.Clear();
      }
   }
}