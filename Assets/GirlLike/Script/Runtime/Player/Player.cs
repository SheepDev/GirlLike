using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class Player : MonoBehaviour
  {
    public PlayerHitPoint HitPoint { get; private set; }

    private void Awake()
    {
      HitPoint = GetComponent<PlayerHitPoint>();
    }
  }
}
