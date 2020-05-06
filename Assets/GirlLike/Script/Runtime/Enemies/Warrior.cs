using System.Collections;
using Orb.GirlLike.AI;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Warrior : Enemy
  {
    [Header("Attack Config")]
    public float attackDuration;
    public float attackCountdown;

    private bool isAllowToAttack;
    private bool isAttack;

    public MoveTo MoveTo { get; private set; }
    public Animator Animator { get; private set; }

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
      if (isAttack || IsStun) return;

      var targetPoint = GetTarget().GetCenter();

      if (isAllowToAttack && BoundsConstains(attackBounds, targetPoint))
      {
        Stop();
        StartCoroutine(Attack());
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
      var transform = GetTransform();
      var targetPoint = GetTarget().GetCenter();
      var isLeft = targetPoint.x < transform.position.x;

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
      isAllowToAttack = false;
      isAttack = true;
      Animator.Play("Attack");
      yield return new WaitForSeconds(attackDuration);

      yield return AttackCountdown();
    }

    private IEnumerator AttackCountdown()
    {
      yield return new WaitForSeconds(attackCountdown);
      Stop();
      isAttack = false;
      isAllowToAttack = true;
    }
  }
}
