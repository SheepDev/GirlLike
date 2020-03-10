using Orb.GirlLike.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Utility
{
  public class TriggerEnter : MonoBehaviour
  {
    public Tag selectTag;

    [Header("Events")]
    public UnityEvent onEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
      var tagName = selectTag.ToString();
      if (string.IsNullOrEmpty(tagName) || other.CompareTag(tagName))
        onEnter.Invoke();
    }
  }
}
