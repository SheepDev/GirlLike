using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Players
{
  public class InteractivePlayer : MonoBehaviour
  {
    public UnityEvent onInteractive;

    public virtual void Interactive(Player player)
    {
      onInteractive.Invoke();
    }
  }
}
