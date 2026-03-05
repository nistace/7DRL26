using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using SDRL26.Battles.Actions;
using UnityEngine;

namespace SDRL26.Battles.Battlers
{
   public class BattlerPosture : MonoBehaviour
   {
      public enum Portrait
      {
         Rest = 0,
         Action = 1,
         Dead = 2
      }

      [SerializeField] private Sprite _icon;
      [SerializeField] private SerializedDictionary<Portrait, Sprite> _portraits = new() { { Portrait.Rest, null }, { Portrait.Action, null }, { Portrait.Dead, null } };
      [SerializeField] private ActionTarget _target = ActionTarget.FirstEnemy;
      [SerializeField] private int _additionalTargets;
      [SerializeField] private float _preparationTime = 2f;
      [SerializeField] private float _chargeActionTime = 1;
      [SerializeField] private float _restTime = 2;

      private bool actionsInitialized;
      private BattleAction[] _actions;

      public Sprite Icon => _icon;
      public ActionTarget Target => _target;

      public IReadOnlyList<BattleAction> Actions
      {
         get
         {
            if (!actionsInitialized) RefreshActions();

            return _actions;
         }
      }

      public float PreparationTime => _preparationTime;
      public float ChargeActionTime => _chargeActionTime;
      public float RestTime => _restTime;
      public int AdditionalTargets => _additionalTargets;

      public Sprite GetPortrait(Portrait portrait) => _portraits.GetValueOrDefault(portrait);

      public void RefreshActions()
      {
         actionsInitialized = true;
         _actions = GetComponentsInChildren<BattleAction>();
      }
   }
}