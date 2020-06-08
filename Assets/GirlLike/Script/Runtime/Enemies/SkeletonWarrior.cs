using System.Collections;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class SkeletonWarrior : Enemy
  {
    protected override IEnumerator Attack()
    {
      OnIdle();
      canMove = false;
      canAttack = false;
      yield return base.Attack();
      yield return new WaitForSeconds(1f);
      canMove = true;
      isAttack = false;
      yield return AttackCountdown(attackCountdown);
    }
  }
}
