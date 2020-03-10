using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Utility
{
  public class TriggerEnter : MonoBehaviour
  {
    [Header("Events")]
    public UnityEvent onEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
      onEnter.Invoke();
    }
  }
}
