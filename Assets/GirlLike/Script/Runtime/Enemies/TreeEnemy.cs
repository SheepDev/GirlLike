using System.Collections;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class TreeEnemy : Enemy
  {
    public AudioSource audioSpawn;

    protected override void Update()
    {
      if (IsAttack || IsStun) return;

      var targetPoint = GetTarget().GetCenter();

      if (CanSpawn(targetPoint))
      {
        StartCoroutine(DelaySpawn(1f));
      }
      else if (CanAttack(targetPoint))
      {
        StartCoroutine(Attack());
      }
    }

    protected override IEnumerator Attack()
    {
      var isLeft = TargetIsLeft();
      isAttack = true;
      canAttack = false;
      Animator.Play(isLeft ? "AttackLeft" : "AttackRight");
      yield return AttackCountdown(attackCountdown);
    }

    protected bool CanSpawn(Vector3 target)
    {
      return !isSpawn && BoundsConstains(attackBounds, target);
    }

    protected override IEnumerator AttackCountdown(float attackCountdown)
    {
      yield return new WaitForSeconds(attackCountdown);
      isAttack = false;
      canAttack = true;
    }

    protected override IEnumerator DelaySpawn(float delay)
    {
      isSpawn = true;
      Animator.Play("Idle");
      Animator.Play("Spawn");
      audioSpawn.Play();
      yield return new WaitForSeconds(delay);
      canAttack = true;
    }

    protected override void OnEnable()
    {
    }
  }
}
