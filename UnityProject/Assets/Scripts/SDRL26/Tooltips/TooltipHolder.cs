using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SDRL26.Tooltips
{
   public class TooltipHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
   {
      [SerializeField] private Tooltip _tooltip;
      [SerializeField] private Transform _defaultAnchor;

      public Tooltip Tooltip
      {
         get => _tooltip;
         set
         {
            _tooltip = value;

            if (_tooltip is { Anchor: null })
            {
               _tooltip.Anchor = _defaultAnchor;
            }

            OnTooltipChanged.Invoke();
         }
      }

      public UnityEvent OnTooltipChanged { get; } = new();
      public static UnityEvent<TooltipHolder> OnTooltipEnter { get; } = new();
      public static UnityEvent<TooltipHolder> OnTooltipExit { get; } = new();

      public void OnPointerEnter(PointerEventData eventData) => OnTooltipEnter.Invoke(this);

      public void OnPointerExit(PointerEventData eventData) => OnTooltipExit.Invoke(this);
   }
}