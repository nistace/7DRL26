using UnityEngine;

namespace SDRL26.Rendering.Battleground.DragAndDrop
{
   public class DraggableTeam : MonoBehaviour
   {
      [SerializeField] private RectTransform _dropPosition;

      public RectTransform DropPosition => _dropPosition;

      public void ShowDropPosition(int index)
      {
         _dropPosition.gameObject.SetActive(true);
         _dropPosition.SetSiblingIndex(index);
      }

      public void MoveDropPosition(int delta) => _dropPosition.SetSiblingIndex(Mathf.Clamp(GetDropSiblingIndex() + delta, 0, _dropPosition.parent.childCount));
      public int GetDropSiblingIndex() => _dropPosition.GetSiblingIndex();
      public void HideDropPosition() => _dropPosition.gameObject.SetActive(false);
   }
}