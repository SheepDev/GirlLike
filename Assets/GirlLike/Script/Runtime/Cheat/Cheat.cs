using System.Collections;
using System.Collections.Generic;
using Orb.GirlLike.Combats;
using Orb.GirlLike.Players;
using UnityEngine;

namespace Orb.GirlLike.Cheat
{
  public class Cheat : MonoBehaviour
  {
    private Player player;
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
    }
  }
}
