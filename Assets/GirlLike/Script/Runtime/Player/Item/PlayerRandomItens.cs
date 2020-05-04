using Orb.GirlLike.Itens;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerRandomItens : PlayerActiveItem
  {
    public Vector3 offset;
    public float itemDistance;
    public int itemCount;
    public Item[] itens;

    public override bool Use(Player player)
    {
      var transform = player.GetTransform();
      var width = itemCount * itemDistance;
      var startX = -(width / 2);

      for (int i = 0; i < itemCount; i++)
      {
        var index = Random.Range(0, itens.Length);
        var selectedItem = itens[index];

        var currentX = startX + (itemDistance * i);
        var position = transform.position + offset;
        position.x += currentX;

        var item = Instantiate(selectedItem, position, Quaternion.identity);
      }

      return true;
    }
  }
}
