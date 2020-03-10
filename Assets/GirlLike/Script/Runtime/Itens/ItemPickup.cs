using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Itens
{
  public class ItemPickup : MonoBehaviour
  {
    public ContactFilter2D filter2D;
    public Collider2D searchArea;

    [Header("Events")]
    public ItemEvent onPickup;

    public void Pickup()
    {
      Pickup(searchArea);
    }

    public void Pickup(Collider2D searchArea)
    {
      if (SearchCloseItem(searchArea, out var item))
      {
        onPickup.Invoke(item);
      }
    }

    private bool SearchCloseItem(Collider2D collider, out Item foundItem)
    {
      var colliders = new List<Collider2D>();
      collider.OverlapCollider(filter2D, colliders);

      foreach (var itemCollider in colliders)
      {
        var item = itemCollider.GetComponent<Item>();

        if (item != null)
        {
          foundItem = item;
          return true;
        }
      }

      foundItem = default;
      return false;
    }

    [System.Serializable]
    public class ItemEvent : UnityEvent<Item> { }
  }
}
