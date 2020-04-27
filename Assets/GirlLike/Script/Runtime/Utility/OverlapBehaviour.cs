using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Utility
{
  public class OverlapBehaviour : MonoBehaviour
  {
    public Collider2D _collider2D;
    public LayerMask mask;

    [Header("Events")]
    public UnityEvent onOverlap;

    protected virtual void Awake()
    {
      if (_collider2D == null)
        _collider2D = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
      Overlap();
    }

    public Collider2D[] Overlap()
    {
      var bounds = _collider2D.bounds;
      var colliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0, mask);

      if (colliders.Length > 0)
        onOverlap.Invoke();

      return colliders;
    }
  }
}
