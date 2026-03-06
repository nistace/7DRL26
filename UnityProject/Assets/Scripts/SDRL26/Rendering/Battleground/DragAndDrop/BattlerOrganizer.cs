using SDRL26.GameControllers;
using SDRL26.GameControllers.GameStates;
using SDRL26.Libraries;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SDRL26.Rendering.Battleground.DragAndDrop
{
   public class BattlerOrganizer : MonoBehaviour
   {
      [SerializeField] private GameStateTypes _enabledInStates = GameStateTypes.PrepareBattle | GameStateTypes.PauseBattle;
      [SerializeField] private DraggableTeam _organizingTeam;
      [SerializeField] private Transform _topTransform;
      [SerializeField] private float _changePositionOffset = 15;

      private DraggableBattlerToken _draggedToken;
      private Vector3 localPositionOnStartDrag;

      private void Start()
      {
         GameState.OnStateChanged.AddListener(HandleStateChanged);
         RefreshEnabled();
      }

      private void OnDestroy()
      {
         GameState.OnStateChanged.RemoveListener(HandleStateChanged);
      }

      private void HandleStateChanged(GameState arg0) => RefreshEnabled();
      private void RefreshEnabled() => enabled = GameState.CurrentState.Is(_enabledInStates);

      private void OnEnable()
      {
         DraggableBattlerToken.OnDrag.AddListener(HandleTokenDragged);
         DraggableBattlerToken.OnDrop.AddListener(HandleTokenDropped);
      }

      private void OnDisable()
      {
         DraggableBattlerToken.OnDrag.RemoveListener(HandleTokenDragged);
         DraggableBattlerToken.OnDrop.RemoveListener(HandleTokenDropped);
      }

      private void HandleTokenDropped(DraggableBattlerToken token)
      {
         if (_draggedToken != token) return;

         var battler = _draggedToken.Token.Battler;
         var dropIndex = _organizingTeam.GetDropSiblingIndex();
         _draggedToken.transform.SetParent(_organizingTeam.transform);
         _draggedToken.transform.SetSiblingIndex(dropIndex);
         _organizingTeam.HideDropPosition();
         _draggedToken = null;

         GameData.PlayerTeam.MoveBattlerToIndex(battler, dropIndex);
      }

      private void HandleTokenDragged(DraggableBattlerToken token)
      {
         _draggedToken = token;
         localPositionOnStartDrag = _draggedToken.transform.position - (Vector3)Mouse.current.position.ReadValue();
         _organizingTeam.ShowDropPosition(_draggedToken.transform.GetSiblingIndex());
         _draggedToken.transform.SetParent(_topTransform);
      }

      private void Update()
      {
         if (!_draggedToken) return;

         var mousePosition = (Vector3)Mouse.current.position.ReadValue();
         _draggedToken.transform.position = mousePosition + localPositionOnStartDrag;

         if (mousePosition.y > _organizingTeam.DropPosition.position.y + _organizingTeam.DropPosition.rect.yMax + _changePositionOffset)
         {
            _organizingTeam.MoveDropPosition(-1);
         }
         else if (mousePosition.y < _organizingTeam.DropPosition.position.y + _organizingTeam.DropPosition.rect.yMin - _changePositionOffset)
         {
            _organizingTeam.MoveDropPosition(1);
         }
      }
   }
}