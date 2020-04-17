using System;
using Orb.GirlLike.Itens;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerStatus : MonoBehaviour
  {
    public float defaultSpeed;
    public float defaultAttackDamage;

    public Bag Bag { get; private set; }

    private void Awake()
    {
      Bag = GetComponent<Bag>();
    }

    public float CalculeDamage(float baseDamage)
    {
      return baseDamage;
    }
  }
}
