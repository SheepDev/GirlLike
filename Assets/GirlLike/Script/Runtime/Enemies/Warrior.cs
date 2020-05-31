using System.Collections;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Warrior : Enemy
  {
    public float attackStunDelay;

    protected override IEnumerator Attack()
    {
      OnIdle();
      canMove = false;
      canAttack = false;
      yield return base.Attack();
      yield return new WaitForSeconds(attackStunDelay);
      canMove = true;
      isAttack = false;
      yield return AttackCountdown(attackCountdown);
    }
  }
}
