using Orb.GirlLike.Itens;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerItem : Item
  {
    [SerializeField] private BonusStatus passive;
    [SerializeField] private BonusStatus active;

    public BonusStatus Passive { get => passive; set => passive = value; }
    public BonusStatus Active { get => active; set => active = value; }
  }

  [System.Serializable]
  public struct BonusStatus
  {
    public float baseHitPoint;
    public float damage;
    public float speed;
  }
}
