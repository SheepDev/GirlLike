using UnityEngine;

namespace Orb.GirlLike.Hall
{
  public class ItensHallGenerator : MonoBehaviour
  {
    public PolygonCollider2D confine;
    public Transform border;
    public ItemInfoObject[] itemInfos;
    public ItemInfo itemPrefab;
    public bool isMoveToLeft;

    private SpriteRenderer sprite;

    private void Awake()
    {
      sprite = GetComponent<SpriteRenderer>();
      Build();
    }

    private void Build()
    {
      var _transform = transform;
      var path = confine.GetPath(0);
      var size = sprite.size;
      var width = size.x;

      for (int i = 0; i < itemInfos.Length; i++)
      {
        var item = itemInfos[i];
        var itemHall = Instantiate(itemPrefab, _transform);
        itemHall.Load(item);
        itemHall.transform.localPosition = new Vector3(width * i, 0, 0);
      }

      size.x = width * itemInfos.Length;
      sprite.size = size;

      if (isMoveToLeft)
      {
        var position = _transform.position;
        position.x -= size.x;
        _transform.position = position;
        border.localPosition = Vector3.zero;
        border.localScale = new Vector3(-1, 1, 1);
        path[2].x = path[1].x = _transform.position.x;
      }
      else
      {
        border.localPosition = new Vector3(size.x, 0, 0);
        path[3].x = path[0].x = _transform.position.x + size.x;
      }

      confine.SetPath(0, path);
    }
  }
}
