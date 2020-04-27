using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class BoundsBehaviour : MonoBehaviour
  {
    public Vector3 center;
    public Vector3 size;
    public Color color;
    private Transform cacheTransform;

    public Bounds GetBounds()
    {
      return new Bounds(GetTransform().position + center, size);
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
      {
        cacheTransform = transform;
      }

      return cacheTransform;
    }

    private void OnValidate()
    {
      var transform = GetTransform();
      var position = transform.position;
      position.z = 0;
      transform.position = position;
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = color;
      Gizmos.DrawWireCube(GetTransform().position + center, size);
    }
  }
}
