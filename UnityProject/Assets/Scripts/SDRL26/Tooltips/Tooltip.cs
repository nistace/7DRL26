using System;
using UnityEngine;

namespace SDRL26.Tooltips
{
   [Serializable]
   public class Tooltip
   {
      [SerializeField] private Transform _anchor;
      [SerializeField] private string _title;
      [SerializeField] private string _body;

      public Transform Anchor
      {
         get => _anchor;
         set => _anchor = value;
      }

      public string Title => _title;
      public string Body => _body;

      public Tooltip(string title, string body, Transform anchor = default)
      {
         _title = title;
         _body = body;
         _anchor = anchor;
      }
   }
}