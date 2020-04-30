using Orb.GirlLike.Itens;
using UnityEngine;
using UnityEngine.UI;

namespace Orb.GirlLike.Players.UI
{
  public class ItemSlot : MonoBehaviour
  {
    [SerializeField] private Image imgIcon;
    [SerializeField] private Image imgSelected;
    [SerializeField] private Item saved;
    public bool IsAvailable => saved == null;
    public Item CurrentItem => saved;

    public void Save(Item item)
    {
      imgIcon.sprite = item.GetComponent<SpriteRenderer>().sprite;
      imgIcon.enabled = true;
      saved = item;
    }

    public void Select(bool isSelect)
    {
      imgSelected.enabled = isSelect;
    }
  }
}
