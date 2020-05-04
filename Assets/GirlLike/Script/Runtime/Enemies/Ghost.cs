using System.Collections;
using Orb.GirlLike.AI;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Ghost : Enemy
  {
    [Header("Attack Config")]
    public float attackDuration;
    public float attackCountdown;

    private bool isAllowToAttack;
    private bool isAttack;
    private bool isAttackCountdown;

    public MoveTo MoveTo { get; private set; }
    public Animator Animator { get; private set; }
    private bool IsAllowToAttack => isAllowToAttack && !isAttackCountdown;

    protected override void Awake()
    {
      base.Awake();
      MoveTo = GetComponent<MoveTo>();
      Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
      Animator.Play("Spawn");
      isAllowToAttack = true;
    }

    private void Update()
    {
      if (IsStun || isAttack) return;

      var targetPoint = GetTarget().GetCenter();

      if (IsAllowToAttack && BoundsConstains(attackBounds, targetPoint))
      {
        Stop();
        StartCoroutine(Attack());
        return;
      }

      var isStop = BoundsConstains(avoidBounds, targetPoint);
      if (isStop)
      {
        var isLeft = TargetIsLeft();
        Stop();
        SetDirection(isLeft);
        return;
      }

      var isFollow = BoundsConstains(followBounds, targetPoint);
      if (isFollow)
      {
        GoToTarget();
      }
      else
      {
        Stop();
      }
    }

    protected override void OnDie()
    {
      StopAllCoroutines();
      Stop();
      Animator.Play("Die");
      base.OnDie();
    }

    protected override void OnStun()
    {
      StopAllCoroutines();
      Stop();
      isAttack = false;
      isAllowToAttack = false;
    }

    protected override void OnNormal()
    {
      StopAllCoroutines();
      Stop();
      isAttack = false;
      isAllowToAttack = true;
    }

    private void GoToTarget()
    {
      var isLeft = TargetIsLeft();
      MoveTo.enabled = true;
      MoveTo.Direction(isLeft ? Vector3.left : Vector3.right);
      SetDirection(isLeft);
      Animator.Play("Walk");
    }

    private void Stop()
    {
      MoveTo.enabled = false;
      Animator.Play("Idle");
    }

    private IEnumerator Attack()
    {
      isAttack = true;
      Animator.Play("Attack");
      yield return new WaitForSeconds(attackDuration);
      isAttack = false;
      yield return AttackCountdown();
    }

    private IEnumerator AttackCountdown()
    {
      isAttackCountdown = true;
      yield return new WaitForSeconds(attackCountdown);
      isAttackCountdown = false;
    }
  }
}
