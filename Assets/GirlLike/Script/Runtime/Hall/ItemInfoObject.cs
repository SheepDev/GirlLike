using UnityEngine;

namespace Orb.GirlLike.Hall
{
  [CreateAssetMenu(fileName = "ItemInfo", menuName = "GirlLike/ItemHall")]
  public class ItemInfoObject : ScriptableObject
  {
    public Sprite itemSprite;
    public string title;
    public string description;
  }
}
