using UnityEngine;

namespace Orb.GirlLike.Helper
{
  public class MinimapTrigger : MonoBehaviour
  {
    public void SetPosition(int index)
    {
      Minimap.Current.SetPosition(index);
    }
  }
}
