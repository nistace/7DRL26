using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Tooltips
{
   public class TooltipItemUi : MonoBehaviour
   {
      [SerializeField] private RectTransform _rectTransform;
      [SerializeField] private ContentSizeFitter _fitter;
      [SerializeField] private CanvasGroup _canvasGroup;
      [SerializeField] private TMP_Text _title;
      [SerializeField] private TMP_Text _text;

      public RectTransform RectTransform => _rectTransform;

      public float Opacity
      {
         get => _canvasGroup.alpha;
         set => _canvasGroup.alpha = value;
      }

      public string Title
      {
         get => _title.text;
         set
         {
            _title.text = value;
            _title.enabled = !string.IsNullOrEmpty(value);
         }
      }

      public string Text
      {
         get => _text.text;
         set
         {
            _text.text = value;
            _text.enabled = !string.IsNullOrEmpty(value);
         }
      }

      private void Update()
      {
         _fitter.enabled = false;
         _fitter.enabled = true;
      }
   }
}