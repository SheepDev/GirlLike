using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Enemy : MonoBehaviour
  {
    protected Target target;
    public SpriteUtility sprite;
    public Vector3 center;

    [Header("Collider Setup")]
    public BoundsBehaviour followBounds;
    public BoundsBehaviour avoidBounds;
    public BoundsBehaviour attackBounds;
    public LayerMask mask;

    private Rigidbody2D rb2D;
    private Transform cacheTransform;

    public Rigidbody2D _rb2D => rb2D;
    public EnemyHitPoint HitPoint { get; private set; }

    protected virtual void Awake()
    {
      rb2D = GetComponent<Rigidbody2D>();
      HitPoint = GetComponentInChildren<EnemyHitPoint>();
      HitPoint.enemy = this;
      HitPoint.onDie.AddListener(OnDie);
      HitPoint.onDamage.AddListener(OnTakeDamage);

      var player = GameObject.FindGameObjectWithTag("Player");
      target = player.GetComponent<Target>();
    }

    protected void SetDirection(bool isLeft)
    {
      sprite.FlipX(isLeft);
    }

    protected bool BoundsConstains(BoundsBehaviour bounds, Vector3 point)
    {
      return bounds != null && bounds.GetBounds().Contains(point);
    }

    protected virtual void OnDie()
    {
    }

    protected virtual void OnTakeDamage()
    {
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

    public Transform GetTransform()
    {
      if (cacheTransform == null)
      {
        cacheTransform = transform;
      }

      return cacheTransform;
    }

    protected virtual void OnDrawGizmosSelected()
    {
      var center = GetCenter();
      Gizmos.color = Color.magenta;
      Gizmos.DrawSphere(center, 0.1f);
    }
  }
}
