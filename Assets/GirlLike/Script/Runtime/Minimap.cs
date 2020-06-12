using UnityEngine;

namespace Orb.GirlLike.Helper
{
  public class Minimap : MonoBehaviour
  {
    public Transform icon;
    public Transform[] positions;
    public static Minimap Current;

    private void Awake()
    {
      Current = this;
    }

    public void SetPosition(int index)
    {
      icon.position = positions[index].position;
    }
  }
}
