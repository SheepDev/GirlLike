using System.Collections.Generic;
using UnityEngine;

namespace Orb.GirlLike.Itens
{
  public class Bag : MonoBehaviour
  {
    public List<Item> itens;

    public virtual void Add(Item item)
    {
      itens.Add(item);
      item.gameObject.SetActive(false);
    }
  }
}
