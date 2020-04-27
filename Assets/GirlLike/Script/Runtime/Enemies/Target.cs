using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Target : MonoBehaviour
  {
    public Vector3 center;
    private Transform cacheTransform;

    public Vector3 GetCenter()
    {
      return center + GetTransform().position;
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
      {
        cacheTransform = transform;
      }

      return cacheTransform;
    }

    protected void OnDrawGizmos()
    {
      Gizmos.color = Color.magenta;
      Gizmos.DrawSphere(GetCenter(), 0.1f);
    }
  }
}
