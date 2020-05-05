using UnityEngine;
using TMPro;

namespace Orb.GirlLike.Hall
{
  public class ItemInfo : MonoBehaviour
  {
    [Header("Config")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshProUGUI titleMesh;
    [SerializeField] private TextMeshProUGUI descriptionMesh;

    public void Load(ItemInfoObject item)
    {
      spriteRenderer.sprite = item.itemSprite;
      titleMesh.text = item.title;
      descriptionMesh.text = item.description;
    }
  }
}
