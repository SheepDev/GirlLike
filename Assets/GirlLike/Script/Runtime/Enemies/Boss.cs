using System.Collections;
using Orb.GirlLike.Combats;
using Orb.GirlLike.Players;
using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Boss : MonoBehaviour
  {
    public CircleCollider2D circleExplosion;
    public Transform[] spawnPoints;
    public float impulseForce;

    public float maxLevitateOffsetY;
    public AnimationCurve levitateSpeedCurve;
    public AnimationCurve fallCurve;
    public Projectile projectilePrefab;
    public Transform spawnPoint;

    private float groundHeight;
    private Player player;
    private Target target;
    private HitPoint hitPoint;
    private Transform cacheTransform;

    public Animator Animator { get; private set; }

    private void Awake()
    {
      Animator = GetComponent<Animator>();
      hitPoint = GetComponentInChildren<HitPoint>();
      cacheTransform = transform;
      groundHeight = cacheTransform.position.y;
      player = GameMode.Current.GetPlayer();
      target = player.GetComponent<Target>();
    }

    public void StartLevitate()
    {
      StartCoroutine(Levitate());
    }

    private void OnEnable()
    {
      Animator.Play("Spawn");
    }

    private void Explosion_AnimTrigger()
    {
      var isOverlap = circleExplosion.OverlapPoint(target.GetCenter());

      if (isOverlap)
      {
        player.HitPoint.ApplyDamage(new PointDamageData(cacheTransform.position, 2f));

        player.Rigidbody.velocity = Vector3.zero;
        var direction = cacheTransform.position.x > target.GetCenter().x ? Vector3.left : Vector3.right;
        direction.y = 1;
        direction.Normalize();
        player.Rigidbody.AddForce(direction * impulseForce, ForceMode2D.Impulse);
      }
    }

    private void FinishSpawn_AnimTrigger()
    {
      StartLevitate();
    }

    private void SpawnRandomPoint_AnimTrigger()
    {
      var randomIndex = Random.Range(0, spawnPoints.Length);
      var spawnPoint = spawnPoints[randomIndex];
      cacheTransform.position = spawnPoint.position;
      Animator.Play("Spawn");
    }

    private IEnumerator Levitate()
    {
      Animator.Play("StartConjuring");

      var offsetY = 0f;
      var position = cacheTransform.position;

      while (offsetY < maxLevitateOffsetY)
      {
        var speed = levitateSpeedCurve.Evaluate(offsetY / maxLevitateOffsetY);
        offsetY += Time.deltaTime * speed;
        cacheTransform.position = position + new Vector3(0, offsetY, 0);
        yield return null;
      }

      yield return AttackPhase();
    }

    private IEnumerator AttackPhase()
    {
      var projectileCount = Random.Range(12, 20);
      var delay = new WaitForSeconds(.3f);

      for (int i = 0; i < projectileCount; i++)
      {
        var projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectile.gameObject.SetActive(true);
        projectile.MoveTo.Direction(TargetDirection());

        yield return delay;
      }

      yield return FallPhase();
    }

    private Vector3 TargetDirection()
    {
      return (target.GetCenter() - spawnPoint.position).normalized;
    }

    private IEnumerator FallPhase()
    {
      Animator.Play("Fall");
      var currentPoint = hitPoint.CurrentHitPoint;

      var position = cacheTransform.position;
      var targetPosition = position;
      targetPosition.y = groundHeight;

      var time = 0f;
      while (time < fallCurve.keys[fallCurve.length - 1].time)
      {
        time += Time.deltaTime;
        var t = fallCurve.Evaluate(time);
        cacheTransform.position = Vector3.Lerp(position, targetPosition, t);
        yield return null;
      }

      Animator.Play("Idle");

      time = 3f;
      while (time > 0)
      {
        time -= Time.deltaTime;
        yield return null;
      }

      Animator.Play("Explosion");
    }
  }
}
