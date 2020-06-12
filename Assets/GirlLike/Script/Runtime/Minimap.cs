using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Helper
{
  public class Minimap : MonoBehaviour
  {
    public Transform icon;
    public Transform[] positions;
    public static Minimap Current;
    public UnityEvent onClose;

    private void Awake()
    {
      Current = this;
    }

    public void SetPosition(int index)
    {
      icon.position = positions[index].position;
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        onClose.Invoke();
      }
    }
  }
}
