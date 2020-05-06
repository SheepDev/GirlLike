using Orb.GirlLike.Combats;
using Orb.GirlLike.Itens;
using Orb.GirlLike.Players;
using UnityEngine;

namespace Orb.GirlLike.Cheat
{
  public class Cheat : MonoBehaviour
  {
    private Player player;
    [SerializeField] private Transform bossPoint;
    [SerializeField] private Item[] itens;
    private int indexItem;
    private GameObject hitpointGameObject;

    private void Start()
    {
      player = GameMode.Current.GetPlayer();
      hitpointGameObject = player.GetComponentInChildren<TakeDamage>(true).gameObject;
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.I))
      {
        hitpointGameObject.SetActive(!hitpointGameObject.activeInHierarchy);
      }
      if (Input.GetKeyDown(KeyCode.B))
      {
        player.GetTransform().position = bossPoint.position;
      }
      if (Input.GetKeyDown(KeyCode.O))
      {
        var itemPrefab = itens[indexItem];
        Instantiate(itemPrefab, player.GetTransform().position + new Vector3(0, 2, 0), Quaternion.identity);
        indexItem = (int)Mathf.Repeat(indexItem + 1, itens.Length);
      }
    }
  }
}
