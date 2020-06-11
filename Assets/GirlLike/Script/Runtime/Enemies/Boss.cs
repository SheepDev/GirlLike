using System.Collections;
using Orb.GirlLike.Combats;
using Orb.GirlLike.Players;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("Events")]
    public UnityEvent onLand;

    private float groundHeight;
    private Player player;
    private Target target;
    private HitPoint hitPoint;
    private Transform cacheTransform;

    private bool isMeleePhase;
    private int meleeAttackCount;
    private bool isDie;

    public Animator Animator { get; private set; }

    private void Awake()
    {
      Animator = GetComponent<Animator>();
      hitPoint = GetComponentInChildren<HitPoint>(true);

      cacheTransform = transform;
      groundHeight = cacheTransform.position.y;
      player = GameMode.Current.GetPlayer();
      target = player.GetComponent<Target>();
      hitPoint.onDie.AddListener(OnDie);
    }

    private void OnDie()
    {
      StopAllCoroutines();
      hitPoint.gameObject.SetActive(false);
      isDie = true;
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

      meleeAttackCount++;
    }

    private void FinishSpawn_AnimTrigger()
    {
      if (isMeleePhase)
      {
        Animator.Play("Explosion");
      }
      else if (hitPoint.CurrentHitPoint > 5)
      {
        Phase1();
      }
      else
      {
        Phase2();
      }
    }

    private void Phase2()
    {
      StartLevitate();
    }

    private void Phase1()
    {
      var random = UnityEngine.Random.Range(0f, 100f);

      if (random < 50f)
      {
        MeleeStart();
      }
      else
      {
        StartLevitate();
      }
    }

    private void MeleeStart()
    {
      Animator.Play("Teleport");
      isMeleePhase = true;
      meleeAttackCount = 0;

      StartCoroutine(MeleePhase());
    }

    private void SpawnRandomPoint_AnimTrigger()
    {
      Vector3 position = Vector3.zero;

      if (isMeleePhase)
      {
        position = player.GetTransform().position;
        position.y = groundHeight;
      }
      else
      {
        var randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        position = spawnPoints[randomIndex].position;
      }

      cacheTransform.position = position;
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

      yield return ProjectilePhase();
    }

    private IEnumerator ProjectilePhase()
    {
      var attackPhase = 0;
      if (hitPoint.CurrentHitPoint < 5)
        attackPhase = Random.Range(0, 2);

      switch (attackPhase)
      {
        case 0:
          yield return ProjectilePhase1();
          break;
        case 1:
          yield return ProjectilePhase2();
          break;
        default:
          yield return ProjectilePhase1();
          break;
      }
      yield return FallPhase();
    }

    private IEnumerator ProjectilePhase1()
    {
      var projectileCount = UnityEngine.Random.Range(12, 20);
      var delay = new WaitForSeconds(.3f);

      for (int i = 0; i < projectileCount; i++)
      {
        var projectile = CreateProjectile(TargetDirection());
        yield return delay;
      }
    }

    private IEnumerator ProjectilePhase2()
    {
      var projectileCount = UnityEngine.Random.Range(12, 20);
      var delay1 = new WaitForSeconds(.1f);
      var delay2 = new WaitForSeconds(.3f);

      for (int j = 0; j < 6; j++)
      {
        var direction = TargetDirection();
        for (int i = 0; i < 8; i++)
        {
          var projectile = CreateProjectile(direction);
          yield return delay1;
        }

        yield return delay2;
      }
    }

    private Projectile CreateProjectile(Vector3 direction)
    {
      var projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
      projectile.gameObject.SetActive(true);
      projectile.MoveTo.Direction(direction);
      projectile.MoveTo.speed = ProjectileSpeed();
      return projectile;
    }

    private float ProjectileSpeed()
    {
      var hitpoint = hitPoint.CurrentHitPoint;
      if (hitpoint < 2f)
        return 15f;
      else if (hitpoint < 4f)
        return 12f;
      else if (hitpoint < 7f)
        return 10f;

      return 8f;
    }

    private IEnumerator MeleePhase()
    {
      while (meleeAttackCount < 3)
      {
        yield return null;
      }

      isMeleePhase = false;
      yield return Vulnerable();
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

      onLand.Invoke();
      player.Rigidbody.AddForce(Vector3.up * 5, ForceMode2D.Impulse);

      yield return Vulnerable();
    }

    private IEnumerator Vulnerable()
    {
      Animator.Play("Idle");

      hitPoint.gameObject.SetActive(true);
      var maxTakeDamage = hitPoint.CurrentHitPoint - 2;
      var time = 7f;

      while (time > 0)
      {
        time -= Time.deltaTime;
        if (hitPoint.CurrentHitPoint <= maxTakeDamage)
          break;
        yield return null;
      }

      hitPoint.gameObject.SetActive(false);
      Animator.Play("Explosion");
    }
  }
}
