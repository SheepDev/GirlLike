using Orb.GirlLike.Combats;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class EnemyHitPoint : HitPoint
  {
    public float forceDamage;
    internal Enemy enemy;

    public override void ApplyDamage(PointDamageData data)
    {
      var direction = CalculateDirection(data.point);
      enemy._rb2D.AddForce(direction);

      base.ApplyDamage(data);
    }

    private Vector2 CalculateDirection(Vector3 point)
    {
      var positionX = enemy.GetTransform().position.x;
      var direction = Vector2.one.normalized;
      direction *= forceDamage;

      if (positionX < point.x)
        direction.x *= -1;

      return direction;
    }

    public override void Die()
    {
      gameObject.SetActive(false);
      base.Die();
    }
  }
}
