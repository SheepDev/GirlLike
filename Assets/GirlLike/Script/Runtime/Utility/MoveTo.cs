using UnityEngine;

namespace Orb.GirlLike.AI
{
  public class MoveTo : MonoBehaviour
  {
    public float speed;
    public bool isLocal;
    private Transform cacheTransform;
    private Vector3 direction;

    private void Update()
    {
      var transform = GetTransform();
      var speedDelta = speed * Time.deltaTime;
      var targetPosition = Vector3.one;

      if (!isLocal)
        targetPosition = transform.position + direction * speedDelta;
      else
        targetPosition = transform.position + transform.rotation * direction * speedDelta;

      transform.position = targetPosition;
    }

    public void Direction(Vector3 to)
    {
      direction = to.normalized;
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
      {
        cacheTransform = transform;
      }

      return cacheTransform;
    }
  }
}
