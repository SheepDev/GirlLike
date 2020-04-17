using UnityEngine;

namespace Orb.GirlLike.Utility
{
  [RequireComponent(typeof(SpriteRenderer))]
  public class SpriteUtility : MonoBehaviour
  {
    public SpriteRenderer sprite;
    public Transform flipTransform;

    public void FlipX(bool isFlip)
    {
      var scale = Vector3.one;
      if (isFlip) scale.x *= -1;

      sprite.flipX = isFlip;
      flipTransform.localScale = scale;
    }
  }
}