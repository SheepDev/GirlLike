using UnityEngine;

namespace Orb.GirlLike.Controllers
{
  public class ControllerMap : MonoBehaviour
  {
    public AxisInfo horizontal;
    public KeyCode jumpKey;
    public KeyCode attackKey;
    public KeyCode dashKey;
    public KeyCode interactiveKey;
  }

  [System.Serializable]
  public struct AxisInfo
  {
    public string name;
    public bool isInvert;
  }
}
