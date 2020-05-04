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
      base.ApplyDamage(data);

      if (IsDie) return;

      var direction = CalculateDirection(data.point);
      enemy._rb2D.AddForce(direction, ForceMode2D.Impulse);
    }

    private Vector2 CalculateDirection(Vector3 point)
    {
      var positionX = enemy.GetTransform().position.x;
      var direction = new Vector3(1, 0, 0);
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
