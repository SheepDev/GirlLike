using UnityEngine;
using UnityEngine.UI;

namespace Orb.GirlLike.Players.UI
{
  public class Heart : MonoBehaviour
  {
    public Image foreground;
    public Image background;

    public void On() => foreground.enabled = true;
    public void Off() => foreground.enabled = false;
  }
}
