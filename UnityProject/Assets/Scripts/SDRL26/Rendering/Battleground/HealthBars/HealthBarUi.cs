using System.Collections.Generic;
using SDRL26.Battles.Battlers;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Battleground.HealthBars
{
   public class HealthBarUi : MonoBehaviour
   {
      [SerializeField] private HealthBarData _data;

      private Health _health;
      private readonly List<Image> healthTokens = new();
      private readonly List<Image> shieldTokens = new();

      public void Setup(Health health)
      {
         _health?.OnChanged.RemoveListener(HandleChanged);

         _health = health;
         _health?.OnChanged.AddListener(HandleChanged);

         Refresh();
      }

      private void OnDestroy()
      {
         _health?.OnChanged.RemoveListener(HandleChanged);
      }

      private void HandleChanged() => Refresh();

      private void Refresh()
      {
         if (_health == null) return;

         while (healthTokens.Count > _health.MaxHealth)
         {
            HealthPool.Pool(healthTokens[0]);
            healthTokens.RemoveAt(0);
         }

         while (healthTokens.Count < _health.MaxHealth)
         {
            var token = HealthPool.GetToken(HealthTokenType.Health, transform);
            token.gameObject.SetActive(true);
            token.transform.SetSiblingIndex(healthTokens.Count);
            healthTokens.Add(token);
         }

         for (var i = 0; i < healthTokens.Count; ++i)
         {
            healthTokens[i].color = i < _health.CurrentHealth ? _data.FullColor : _data.EmptyColor;
            healthTokens[i].sprite = i < _health.CurrentHealth ? _data.FullSprite : _data.EmptySprite;
         }

         while (shieldTokens.Count > _health.CurrentShield)
         {
            HealthPool.Pool(shieldTokens[0]);
            shieldTokens.RemoveAt(0);
         }

         while (shieldTokens.Count < _health.CurrentShield)
         {
            var token = HealthPool.GetToken(HealthTokenType.Shield, transform);
            token.gameObject.SetActive(true);
            token.color = Color.white;
            shieldTokens.Add(token);
         }
      }
   }
}