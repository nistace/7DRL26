using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Shared
{
   public class VerticalLayoutHelper : MonoBehaviour
   {
      [SerializeField] private RectTransform _rectTransform;
      [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
      [SerializeField] private float _itemHeight;
      [SerializeField] private float _maxSpacing;

      private readonly List<Transform> _slots = new();
      private int _childrenInUse;

      private Transform CreateSlot()
      {
         var go = new GameObject("Slot", typeof(RectTransform));
         go.transform.SetParent(_rectTransform);
         var rect = go.GetComponent<RectTransform>();

         rect.anchorMin = Vector2.zero;
         rect.anchorMax = Vector2.one;
         rect.pivot = Vector2.zero;
         rect.sizeDelta = new Vector2(0, _itemHeight);

         return rect;
      }

      private void Update()
      {
         if (_childrenInUse <= 1) return;

         var containerHeight = _rectTransform.rect.height;
         var childrenHeight = _childrenInUse * _itemHeight;
         _verticalLayoutGroup.spacing = Mathf.Min((containerHeight - childrenHeight) / (_childrenInUse - 1), _maxSpacing);
      }

      public Transform GetChild(int index)
      {
         while (_slots.Count <= index)
         {
            _slots.Add(CreateSlot());
         }

         return _slots[index];
      }

      public void SetChildrenInUse(int childrenInUse)
      {
         _childrenInUse = childrenInUse;

         for (var i = 0; i < transform.childCount; i++)
         {
            transform.GetChild(i).gameObject.SetActive(i < childrenInUse);
         }
      }
   }
}