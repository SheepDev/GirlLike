using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class InteractivePlayer : MonoBehaviour
  {
    public virtual void Interactive(Player player)
    {
      Debug.Log("Player");
    }
  }
}
