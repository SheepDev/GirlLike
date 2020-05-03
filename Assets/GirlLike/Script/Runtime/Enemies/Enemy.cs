using Orb.GirlLike.Players;
using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Enemy : MonoBehaviour
  {
    private Target target;
    public SpriteUtility sprite;
    public Vector3 center;
    [Header("Drop Setup")]
    public int minCoin;
    public int maxCoin;
    public Coin coinPrefab;

    [Header("Collider Setup")]
    public BoundsBehaviour followBounds;
    public BoundsBehaviour avoidBounds;
    public BoundsBehaviour attackBounds;

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
      var count = Random.Range(minCoin, maxCoin + 1);
      var transform = GetTransform();

      for (int i = 0; i < count; i++)
      {
        var coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        coin.ApplyRandomForce();
      }
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

    protected virtual void OnDrawGizmosSelected()
    {
      var center = GetCenter();
      Gizmos.color = Color.magenta;
      Gizmos.DrawSphere(center, 0.1f);
    }
  }
}
