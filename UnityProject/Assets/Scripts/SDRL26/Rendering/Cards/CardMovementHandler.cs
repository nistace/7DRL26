using System.Collections.Generic;
using UnityEngine;

namespace SDRL26.Rendering.Cards
{
   public class CardMovementHandler : MonoBehaviour
   {
      [SerializeField] private Transform _spawn;
      private static CardMovementHandler Instance { get; set; }

      private readonly Dictionary<Transform, Movement> _movingCards = new();
      private readonly HashSet<Transform> _doneMovements = new();
      public static Transform Spawn => Instance._spawn;

      private void Awake()
      {
         Instance = this;
      }

      private void Update()
      {
         foreach (var card in _movingCards)
         {
            if (card.Value.TickMovement(card.Key))
            {
               _doneMovements.Add(card.Key);
            }
         }

         foreach (var doneMovement in _doneMovements)
         {
            _movingCards.Remove(doneMovement);
         }

         _doneMovements.Clear();
      }

      public static void HideCard(Transform item) => Move(item, Instance._spawn);
      public static void MoveToSlot(Transform item, Transform slot) => Move(item, slot);

      public static void Move(Transform item, Transform destination)
      {
         if (!Instance._movingCards.TryGetValue(item, out var movement))
         {
            movement = new Movement();
            Instance._movingCards.Add(item, movement);
         }

         movement.Destination = destination;
      }

      private class Movement
      {
         public Transform Destination;
         private Vector3 SmoothMovement;

         public bool TickMovement(Transform target)
         {
            target.position = Vector3.SmoothDamp(target.position, Destination.position, ref SmoothMovement, .1f);

            return target.position == Destination.position;
         }
      }

      public static bool IsMoving(Transform item) => Instance._movingCards.ContainsKey(item);

   }
}