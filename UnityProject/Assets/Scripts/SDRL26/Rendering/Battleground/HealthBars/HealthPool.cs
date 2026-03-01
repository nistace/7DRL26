using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

namespace SDRL26.Rendering.Battleground.HealthBars
{
   public class HealthPool : MonoBehaviour
   {
      private static HealthPool Instance { get; set; }

      [SerializeField] private Image _tokenPrefab;
      [SerializeField] private SerializedDictionary<HealthTokenType, Sprite> _tokenSprites;

      private Queue<Image> PoolOfHealthTokens { get; } = new();

      private void Awake()
      {
         Instance = this;
      }

      public static Image GetToken(HealthTokenType tokenType)
      {
         if (!Instance.PoolOfHealthTokens.TryDequeue(out var token))
         {
            token = Instantiate(Instance._tokenPrefab);
         }

         token.sprite = Instance._tokenSprites.GetValueOrDefault(tokenType);
         token.gameObject.SetActive(true);

         return token;
      }

      public static void Pool(Image token)
      {
         if (Instance.PoolOfHealthTokens.Contains(token))
         {
            return;
         }

         Instance.PoolOfHealthTokens.Enqueue(token);
         token.gameObject.SetActive(false);
         token.transform.SetParent(Instance.transform);
      }
   }
}