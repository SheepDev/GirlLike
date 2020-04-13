using UnityEngine;

namespace Gameplay.Effect
{
  [DisallowMultipleComponent]
  public class Parallax : MonoBehaviour
  {
    public float maxDepth;
    public bool enableInX;
    public bool enableInY;

    private Transform[] children;

    public Parallax()
    {
      this.maxDepth = 100;
      this.enableInX = this.enableInY = true;
    }

    private void Awake()
    {
      var _transform = transform;
      children = new Transform[_transform.childCount];

      for (int i = 0; i < _transform.childCount; i++)
      {
        children[i] = _transform.GetChild(i);
      }
    }

    public void Move(Vector2 direction, float deltaSpeed)
    {
      if (!enableInX) direction.x = 0;
      if (!enableInY) direction.y = 0;

      foreach (var child in children)
      {
        float depthPercent = (child.localPosition.z / maxDepth);
        float fixedSpeed = deltaSpeed * depthPercent;
        Vector3 calculateDirection = direction * fixedSpeed;

        child.localPosition += calculateDirection;
      }
    }
  }
}
