using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Utility
{
  public class OverlapBehaviour : MonoBehaviour
  {
    [SerializeField] private bool isEnable;
    public ContactFilter2D filter2D;
    public Collider2D _collider2D;

    [Header("Events")]
    public OverlapEvent onOverlap;

    public bool IsEnable { get => isEnable; set => isEnable = value; }

    protected virtual void Awake()
    {
      if (_collider2D == null)
        _collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
      if (isEnable)
      {
        Overlap();
      }
    }

    public void Overlap()
    {
      var colliders = GetOverlapColliders();

      if (colliders.Count > 0)
        onOverlap.Invoke(colliders);
    }

    public List<Collider2D> GetOverlapColliders()
    {
      var colliders = new List<Collider2D>();
      _collider2D.OverlapCollider(filter2D, colliders);
      return colliders;
    }

    [System.Serializable]
    public class OverlapEvent : UnityEvent<List<Collider2D>> { }
  }
}
