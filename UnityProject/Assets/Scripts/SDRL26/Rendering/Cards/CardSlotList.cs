using System.Linq;
using UnityEngine;

namespace SDRL26.Rendering.Cards
{
   public class CardSlotList : MonoBehaviour
   {
      [SerializeField] private Transform[] _slots;

      public Transform this[int index] => _slots[index];

      private void Reset()
      {
         _slots = Enumerable.Range(0, transform.childCount).Select(t => transform.GetChild(t)).ToArray();
      }

      public void SetCountActive(int count)
      {
         for (var index = 0; index < _slots.Length; index++)
         {
            _slots[index].gameObject.SetActive(index < count);
         }
      }
   }
}