using System.Collections;
using Orb.GirlLike.AI;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  [RequireComponent(typeof(MoveTo))]
  public class Lancer : Enemy
  {
    [Header("Spear Setup")]
    [SerializeField] private Transform spearSpawnPoint;
    [SerializeField] private Spear prefab;

    private bool isAttack;
    private bool isAttackCountdown;
    private bool isAllowToAttack;

    public bool IsAllowToAttack => isAllowToAttack && !isAttackCountdown;
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
      if (IsAllowToAttack && BoundsConstains(attackBounds, targetPoint))
      {
        Stop();
        StartCoroutine(Attack());
        return;
      }

      var isAvoid = BoundsConstains(avoidBounds, targetPoint);
      if (isAvoid)
      {
        AvoidTarget();
        return;
      }

      var isFollow = BoundsConstains(followBounds, targetPoint);
      if (isFollow)
      {
        GoToTarget();
        return;
      }

      Stop();
    }

    private IEnumerator Attack()
    {
      var isLeft = TargetIsLeft();
      SetDirection(isLeft);

      isAttack = true;
      Animator.Play("Attack");
      yield return new WaitForSeconds(2);
      isAttack = false;
      isAttackCountdown = true;
      yield return new WaitForSeconds(2);
      isAttackCountdown = false;
    }

    protected override void OnDie()
    {
      StopAllCoroutines();
      Animator.Play("Die");
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

    private void Stop(bool isAnimated = false)
    {
      MoveTo.enabled = false;

      if (isAnimated)
        Animator.Play("Idle");
    }

    private void SpawnSpear()
    {
      var spear = Instantiate(prefab, spearSpawnPoint.position, Quaternion.identity);
      var isLeft = TargetIsLeft();
      spear.SetDirection(isLeft);
      spear.gameObject.SetActive(true);
    }

    private void AvoidTarget()
    {
      var isLeft = TargetIsLeft();
      GoTo(!isLeft);
    }

    private void GoToTarget()
    {
      var isLeft = TargetIsLeft();
      GoTo(isLeft);
    }

    private void GoTo(bool isLeft)
    {
      MoveTo.enabled = true;
      MoveTo.Direction(isLeft ? Vector3.left : Vector3.right);
      SetDirection(isLeft);
      Animator.Play("Walk");
    }
  }
}
