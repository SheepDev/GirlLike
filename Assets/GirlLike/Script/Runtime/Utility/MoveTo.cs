using UnityEngine;

namespace Orb.GirlLike.AI
{
  public class MoveTo : MonoBehaviour
  {
    public float speed;
    private Transform cacheTransform;
    private Vector3 direction;

    private void Update()
    {
      var transform = GetTransform();
      var speedDelta = speed * Time.deltaTime;
      var targetPosition = transform.position + direction * speedDelta;

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
