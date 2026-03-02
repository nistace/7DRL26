using System.Collections.Generic;
using SDRL26.Battles.Actions;
using UnityEngine;

namespace SDRL26.Battles.Battlers
{
   public class BattlerPosture : MonoBehaviour
   {
      [SerializeField] private Sprite _icon;
      [SerializeField] private ActionTarget _target = ActionTarget.FirstEnemy;
      [SerializeField] private float _preparationTime = 2f;
      [SerializeField] private float _chargeActionTime = 1;
      [SerializeField] private float _restTime = 2;
      [SerializeField] private BattleAction[] _actions;

      public Sprite Icon => _icon;
      public ActionTarget Target => _target;
      public IReadOnlyList<BattleAction> Actions => _actions;
      public float PreparationTime => _preparationTime;
      public float ChargeActionTime => _chargeActionTime;
      public float RestTime => _restTime;

      [ContextMenu("Load Actions")] private void LoadActions() => _actions = GetComponentsInChildren<BattleAction>();
   }
}