using System.Collections.Generic;
using UnityEngine;

namespace SDRL26.Tooltips
{
   public class TooltipsUi : MonoBehaviour
   {
      [SerializeField] private float _deadTime = 1;
      [SerializeField] private float _fadeInTime = .2f;
      [SerializeField] private float _fadeOutTime = .5f;
      [SerializeField] private float _smoothMovementTime = .3f;
      [SerializeField] private TooltipItemUi _tooltipItemUi;
      [SerializeField] private Vector3 _tooltipOffsetWithPosition;

      private readonly List<TooltipHolder> _tooltipStack = new();

      private Tooltip CurrentTooltip => _tooltipStack.Count > 0 ? _tooltipStack[0].Tooltip : null;
      private bool HasTooltip => CurrentTooltip != null;

      private float deadTimeProgress;
      private Vector3 tooltipVelocity;

      private void Start()
      {
         _tooltipItemUi.Opacity = 0;
         TooltipHolder.OnTooltipEnter.AddListener(HandleTooltipEnter);
         TooltipHolder.OnTooltipExit.AddListener(HandleTooltipExit);
      }

      private void OnDestroy()
      {
         TooltipHolder.OnTooltipEnter.RemoveListener(HandleTooltipEnter);
         TooltipHolder.OnTooltipExit.RemoveListener(HandleTooltipExit);
      }

      private void HandleTooltipEnter(TooltipHolder tooltip)
      {
         if (_tooltipStack.Count > 0)
         {
            _tooltipStack[0].OnTooltipChanged.RemoveListener(HandleCurrentTooltipChanged);
         }

         _tooltipStack.Insert(0, tooltip);

         tooltip.OnTooltipChanged.AddListener(HandleCurrentTooltipChanged);

         RefreshTooltip();
      }

      private void HandleTooltipExit(TooltipHolder tooltip)
      {
         tooltip.OnTooltipChanged.RemoveListener(HandleCurrentTooltipChanged);
         _tooltipStack.Remove(tooltip);

         if (_tooltipStack.Count > 0)
         {
            _tooltipStack[0].OnTooltipChanged.AddListener(HandleCurrentTooltipChanged);
         }

         RefreshTooltip();
      }

      private void HandleCurrentTooltipChanged() => RefreshTooltip();

      private void RefreshTooltip()
      {
         if (!HasTooltip)
         {
            return;
         }

         _tooltipItemUi.Text = CurrentTooltip.Body;
         _tooltipItemUi.Title = CurrentTooltip.Title;
      }

      private void Update()
      {
         UpdateTooltipOpacity();
         UpdateTooltipPosition();
      }

      private void UpdateTooltipPosition()
      {
         if (!HasTooltip)
         {
            return;
         }

         if (Mathf.Approximately(_tooltipItemUi.Opacity, 0))
         {
            _tooltipItemUi.transform.position = CurrentTooltip.Anchor.position + _tooltipOffsetWithPosition;
         }
         else
         {
            var screenPos = RectTransformUtility.WorldToScreenPoint(null, CurrentTooltip.Anchor.position + _tooltipOffsetWithPosition);

            _tooltipItemUi.RectTransform.pivot = new Vector2(Mathf.Clamp01(screenPos.x / Screen.width), 1);

            _tooltipItemUi.transform.position = Vector3.SmoothDamp(_tooltipItemUi.transform.position,
               CurrentTooltip.Anchor.position + _tooltipOffsetWithPosition,
               ref tooltipVelocity,
               _smoothMovementTime
            );
         }
      }

      private void UpdateTooltipOpacity()
      {
         if (HasTooltip)
         {
            deadTimeProgress += Time.deltaTime;

            if (deadTimeProgress <= _deadTime)
            {
               return;
            }

            _tooltipItemUi.Opacity = Mathf.MoveTowards(_tooltipItemUi.Opacity, 1, Time.deltaTime / _fadeInTime);

            return;
         }

         _tooltipItemUi.Opacity = Mathf.MoveTowards(_tooltipItemUi.Opacity, 0, Time.deltaTime / _fadeOutTime);

         if (Mathf.Approximately(_tooltipItemUi.Opacity, 0))
         {
            deadTimeProgress = 0;
         }
      }
   }
}