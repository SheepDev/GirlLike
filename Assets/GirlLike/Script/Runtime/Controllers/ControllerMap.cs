using UnityEngine;

namespace Orb.GirlLike.Controllers
{
  public class ControllerMap : MonoBehaviour
  {
    public AxisInfo horizontal;
    public KeyCode jumpKey;
  }

  [System.Serializable]
  public struct AxisInfo
  {
    public string name;
    public bool isInvert;
  }
}
