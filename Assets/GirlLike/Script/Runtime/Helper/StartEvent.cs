using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Helper
{
  public class StartEvent : MonoBehaviour
  {
    public UnityEvent onStart;

    private void Start()
    {
      onStart.Invoke();
    }
  }
}
