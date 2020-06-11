using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Helper
{
  public class EnableEvent : MonoBehaviour
  {
    public UnityEvent onEnable;

    private void OnEnable()
    {
      onEnable.Invoke();
    }
  }
}
