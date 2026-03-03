using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDRL26.Rendering.Cards
{
   public class CardUi : MonoBehaviour, IPointerClickHandler
   {
      [SerializeField] private TMP_Text _displayNameText;
      [SerializeField] private Image _portraitImage;
      [SerializeField] private TMP_Text _descriptionText;

      public string DisplayName
      {
         get => _displayNameText.text;
         set => _displayNameText.text = value;
      }

      public string Description
      {
         get => _descriptionText.text;
         set => _descriptionText.text = value;
      }

      public Sprite Portrait
      {
         get => _portraitImage.sprite;
         set => _portraitImage.sprite = value;
      }

      public UnityEvent OnClick { get; } = new();

      public void OnPointerClick(PointerEventData eventData) => OnClick.Invoke();
   }
}