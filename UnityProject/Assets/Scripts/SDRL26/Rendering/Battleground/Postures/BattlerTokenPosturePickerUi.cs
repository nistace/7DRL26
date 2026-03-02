using SDRL26.Battles.Battlers;
using SDRL26.GameControllers.GameStates;
using UnityEngine;

namespace SDRL26.Rendering.Battleground.Postures
{
   public class BattlerTokenPosturePickerUi : MonoBehaviour
   {
      [SerializeField] private BattlerTokenUi _token;
      [SerializeField] private BattlerPostureButton[] _postureButtons;

      private Battler _battler;

      private void OnEnable()
      {
         RefreshBattler();
         _token.OnBattlerChanged.AddListener(HandleBattlerChanged);
         GameState.OnStateChanged.AddListener(HandleStateChanged);

         foreach (var button in _postureButtons)
         {
            button.OnPostureSelected.AddListener(HandlePostureClicked);
         }
      }

      private void HandlePostureClicked(BattlerPosture posture)
      {
         if (GameState.CurrentState is PrepareBattleGameState)
         {
            _battler.SelectPosture(posture);
         }
      }

      private void RefreshBattler()
      {
         if (_battler)
         {
            _battler.OnPostureChanged.RemoveListener(HandleBattlerPostureChanged);
         }

         _battler = _token.Battler;

         if (_battler)
         {
            _battler.OnPostureChanged.AddListener(HandleBattlerPostureChanged);
         }

         RefreshButtons();
      }

      private void OnDisable()
      {
         _token.OnBattlerChanged.RemoveListener(HandleBattlerChanged);

         if (_token.Battler != null)
         {
            _token.Battler.OnPostureChanged.AddListener(HandleBattlerPostureChanged);
         }

         GameState.OnStateChanged.RemoveListener(HandleStateChanged);

         foreach (var button in _postureButtons)
         {
            button.OnPostureSelected.RemoveListener(HandlePostureClicked);
         }
      }

      private void HandleBattlerChanged() => RefreshBattler();

      private void HandleBattlerPostureChanged(BattlerPosture newPosture) => RefreshButtons();
      private void HandleStateChanged(GameState newState) => RefreshButtons();

      private void RefreshButtons()
      {
         if (_token.Battler == null)
         {
            return;
         }

         if (GameState.CurrentState is PrepareBattleGameState)
         {
            for (var i = 0; i < _token.Battler.Postures.Count; ++i)
            {
               var button = _postureButtons[i];
               var posture = _token.Battler.Postures[i];
               button.SetUp(posture);
               //   button.Selected = posture == _token.Battler.Posture;
               button.gameObject.SetActive(true);
            }

            for (var i = _token.Battler.Postures.Count; i < _postureButtons.Length; ++i)
            {
               _postureButtons[i].gameObject.SetActive(true);
            }
         }
         else
         {
            _postureButtons[0].SetUp(_token.Battler.Posture);
            //     _postureButtons[0].Selected = true;
            _postureButtons[0].gameObject.SetActive(true);

            for (var i = _token.Battler.Postures.Count; i < _postureButtons.Length; ++i)
            {
               _postureButtons[i].gameObject.SetActive(true);
            }
         }
      }
   }
}