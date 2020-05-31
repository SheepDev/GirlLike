using System.Collections;
using UnityEngine.Events;

namespace Orb.GirlLike.Ememies
{
  public class Ghost : Enemy
  {
    public UnityEvent onDie;

    protected override IEnumerator Attack()
    {
      var isLeft = TargetIsLeft();
      SetDirection(isLeft);
      OnIdle();
      canMove = false;
      yield return base.Attack();
      canAttack = false;
      yield return AttackCountdown(attackCountdown);
      canMove = true;
    }

    protected override void OnDie()
    {
      base.OnDie();
      RB2D.gravityScale = .2f;
      onDie.Invoke();
    }
  }
}
