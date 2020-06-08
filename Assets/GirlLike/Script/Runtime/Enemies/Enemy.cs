using System.Collections;
using Orb.GirlLike.AI;
using Orb.GirlLike.Players;
using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public abstract class Enemy : MonoBehaviour
  {
    public bool canAttack;
    public bool canMove;
    public float delaySpawn;
    public float attackCountdown;
    public bool isDirection;
    public Vector3 center;
    public SpriteUtility sprite;

#pragma warning disable CS0649
    [Header("Drop Setup")]
    [SerializeField] private int minCoin;
    [SerializeField] private int maxCoin;
    [SerializeField] private Coin coinPrefab;
#pragma warning restore CS0649

    [Header("Collider Setup")]
    public BoundsBehaviour[] followBounds;
    public BoundsBehaviour[] stopBounds;
    public BoundsBehaviour[] avoidBounds;
    public BoundsBehaviour[] attackBounds;

    protected bool isNotPlayWalkAnimation;
    private bool isStun;
    protected bool isAttack;
    protected bool isSpawn;
    private Target target;
    private Transform cacheTransform;

    public bool IsStun => isStun;
    public bool IsAttack => isAttack;
    public Animator Animator { get; private set; }
    public Rigidbody2D RB2D { get; private set; }
    public EnemyHitPoint HitPoint { get; private set; }
    public MoveTo MoveTo { get; private set; }

    protected Enemy()
    {
      delaySpawn = .5f;
    }

    protected virtual void Awake()
    {
      Animator = GetComponent<Animator>();
      RB2D = GetComponent<Rigidbody2D>();
      MoveTo = GetComponent<MoveTo>();
      HitPoint = GetComponentInChildren<EnemyHitPoint>();
      HitPoint.enemy = this;
      HitPoint.onDie.AddListener(OnDie);
      HitPoint.onDamage.AddListener(OnTakeDamage);
    }

    protected virtual void Update()
    {
      if (isSpawn || IsAttack || IsStun) return;

      var targetPoint = GetTarget().GetCenter();

      if (CanAttack(targetPoint))
      {
        StartCoroutine(Attack());
      }
      else if (CanAvoid(targetPoint))
      {
        AvoidTarget();
      }
      else if (IsStop(targetPoint))
      {
        OnIdle();
      }
      else if (CanFollow(targetPoint))
      {
        GoToTarget();
      }
      else
      {
        OnIdle();
      }
    }

    public void Stun(float stunDelay)
    {
      OnStun();
      StartCoroutine(StunDelay(stunDelay));
    }

    internal void SetDirection(bool isLeft)
    {
      sprite.FlipX(isLeft);
    }

    protected bool BoundsConstains(BoundsBehaviour bounds, Vector3 point)
    {
      return bounds != null && bounds.GetBounds().Contains(point);
    }

    protected bool BoundsConstains(BoundsBehaviour[] bounds, Vector3 point)
    {
      foreach (var bound in bounds)
      {
        return bound != null && bound.GetBounds().Contains(point);
      }

      return false;
    }

    protected bool CanAttack(Vector3 target)
    {
      return canAttack && BoundsConstains(attackBounds, target);
    }

    protected bool CanFollow(Vector3 target)
    {
      return canMove && BoundsConstains(followBounds, target);
    }

    protected bool CanAvoid(Vector3 target)
    {
      return canMove && BoundsConstains(avoidBounds, target);
    }

    protected bool IsStop(Vector3 target)
    {
      return BoundsConstains(stopBounds, target);
    }

    protected virtual IEnumerator Attack()
    {
      isAttack = true;
      canAttack = false;
      Animator.Play("Attack");
      yield return null;
    }

    protected virtual IEnumerator AttackCountdown(float attackCountdown)
    {
      yield return new WaitForSeconds(attackCountdown);
      isAttack = false;
      canAttack = true;
      OnIdle();
    }

    protected virtual void OnEnable()
    {
      Animator.Play("Spawn");
      StartCoroutine(DelaySpawn(delaySpawn));
    }

    protected virtual void OnIdle()
    {
      if (MoveTo != null)
        MoveTo.enabled = false;
      Animator.Play("Idle");
    }

    protected virtual void OnDie()
    {
      OnIdle();
      var count = Random.Range(minCoin, maxCoin + 1);
      var transform = GetTransform();

      for (int i = 0; i < count; i++)
      {
        var coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        coin.ApplyRandomForce();
      }

      enabled = false;
      StopAllCoroutines();
      Animator.Play("Die");
    }

    protected virtual void OnTakeDamage()
    {
    }

    protected virtual void OnStun()
    {
      StopAllCoroutines();
      OnIdle();
      canMove = false;
      canAttack = false;
    }

    protected virtual void OnNormal()
    {
      canMove = true;
      canAttack = true;
    }

    private Vector3 GetCenter()
    {
      return center + GetTransform().position;
    }

    public bool TargetIsLeft()
    {
      var transform = GetTransform();
      var targetPoint = target.GetCenter();
      return targetPoint.x < transform.position.x;
    }

    public Vector3 GetTargetDirection()
    {
      return GetTargetDirection(GetTarget().GetCenter());
    }

    public Vector3 GetTargetDirection(Vector3 worldPoint)
    {
      return (target.GetCenter() - worldPoint).normalized;
    }

    protected virtual void GoToTarget()
    {
      if (isDirection)
      {
        GoTo(GetTargetDirection());
        return;
      }

      var isLeft = TargetIsLeft();
      GoTo(isLeft);
    }

    protected virtual void AvoidTarget()
    {
      if (isDirection)
      {
        GoTo(GetTargetDirection() * -1f);
        return;
      }

      var isLeft = !TargetIsLeft();
      GoTo(isLeft);
    }

    private void GoTo(bool isLeft)
    {
      MoveTo.enabled = true;
      MoveTo.Direction(isLeft ? Vector3.left : Vector3.right);
      SetDirection(isLeft);

      if (!isNotPlayWalkAnimation)
        Animator.Play("Walk");
    }

    private void GoTo(Vector3 direction)
    {
      MoveTo.enabled = true;
      MoveTo.Direction(direction);
      SetDirection(direction.x < 0f);
      Animator.Play("Walk");
    }

    protected Target GetTarget()
    {
      if (target == null)
        target = GameMode.Current.GetPlayer().GetComponent<Target>();
      return target;
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
      {
        cacheTransform = transform;
      }

      return cacheTransform;
    }

    private IEnumerator StunDelay(float delay)
    {
      isStun = true;
      yield return new WaitForSeconds(delay);
      isStun = false;
      OnNormal();
    }

    protected virtual IEnumerator DelaySpawn(float delay)
    {
      isSpawn = true;
      yield return new WaitForSeconds(delay);
      canAttack = true;
      canMove = true;
      isSpawn = false;
    }

    protected virtual void OnDrawGizmosSelected()
    {
      var center = GetCenter();
      Gizmos.color = Color.magenta;
      Gizmos.DrawSphere(center, 0.1f);
    }
  }
}
