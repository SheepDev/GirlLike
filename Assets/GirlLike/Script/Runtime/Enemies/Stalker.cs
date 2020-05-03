using System.Collections;
using Orb.GirlLike.AI;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Stalker : MonoBehaviour
  {
    [Header("Setup")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Vector3 center;
    public Collider2D target;

    [Header("Attack Config")]
    public float attackDuration;
    public float attackCountdown;


    [Header("Collider Setup")]
    public BoundsData closeBounds;
    public BoundsData attackBounds;

    private Transform cacheTransform;
    private bool isAttack;
    private bool isAttackCountdown;

    public MoveTo MoveTo { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
      MoveTo = GetComponent<MoveTo>();
      Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
      Animator.Play("Spawn");
    }

    private void Update()
    {
      var point = GetTargetClosetPoint();
      var isClose = IsTargetClose(point);

      if (!isClose || isAttack)
      {
        Stop(!isAttack);
        return;
      }

      var transform = GetTransform();
      var isCloseToAttack = IsCloseToAttack(point);
      var isLeft = point.x < transform.position.x;

      if (!isAttackCountdown && isCloseToAttack)
      {
        Stop();
        SetTargetDirection(isLeft);
        StartCoroutine(Attack());
      }
      else if (!isCloseToAttack)
      {
        SetTargetDirection(isLeft);
        MoveTo.enabled = true;
        Animator.Play("Walk");
      }
      else
      {
        Stop(true);
      }
    }


    private IEnumerator Attack()
    {
      isAttack = true;
      Animator.Play("Attack");
      yield return new WaitForSeconds(attackDuration);
      isAttack = false;
      isAttackCountdown = true;
      yield return new WaitForSeconds(attackCountdown);
      isAttackCountdown = false;
    }

    private void SetTargetDirection(bool isLeft)
    {
      var direction = isLeft ? Vector3.left : Vector3.right;
      var scala = Vector3.one;
      if (!isLeft) scala.x = -1;

      GetTransform().localScale = scala;

      MoveTo.Direction(direction);
    }

    private void Stop(bool isAnimated = false)
    {
      MoveTo.enabled = false;

      if (isAnimated)
        Animator.Play("Idle");
    }

    private bool IsTargetClose(Vector3 point)
    {
      var bounds = GetBounds(closeBounds);
      return bounds.Contains(point);
    }

    private bool IsCloseToAttack(Vector3 point)
    {
      var bounds = GetBounds(attackBounds);
      return bounds.Contains(point);
    }

    private Bounds GetTargetBounds()
    {
      return target.bounds;
    }

    private Vector3 GetTargetClosetPoint()
    {
      var targetBounds = GetTargetBounds();
      return targetBounds.ClosestPoint(GetCenter());
    }

    private Bounds GetBounds(BoundsData data)
    {
      var transform = GetTransform();
      return new Bounds(transform.position + data.center, data.size);
    }

    private Vector3 GetCenter()
    {
      return center + GetTransform().position;
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
      {
        cacheTransform = transform;
      }

      return cacheTransform;
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.magenta;
      Gizmos.DrawSphere(GetCenter(), 0.1f);

      Gizmos.color = Color.yellow;
      var bounds = GetBounds(closeBounds);
      Gizmos.DrawWireCube(bounds.center, bounds.size);

      Gizmos.color = Color.red;
      bounds = GetBounds(attackBounds);
      Gizmos.DrawWireCube(bounds.center, bounds.size);
    }


    [System.Serializable]
    public struct BoundsData
    {
      public Vector3 center;
      public Vector3 size;
    }
  }
}
