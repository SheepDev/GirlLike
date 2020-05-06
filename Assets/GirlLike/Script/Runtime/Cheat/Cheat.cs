using Orb.GirlLike.Combats;
using Orb.GirlLike.Players;
using UnityEngine;

namespace Orb.GirlLike.Cheat
{
  public class Cheat : MonoBehaviour
  {
    private Player player;
    [SerializeField] private Transform bossPoint;
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
    }
  }
}
