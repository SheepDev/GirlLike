using System.Collections;
using Orb.GirlLike.Combats;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Boss : MonoBehaviour
  {
    public Projectile projectilePrefab;
    public Vector3 offset;
    public Transform projectilePoint;
    public Transform[] teleportPoints;

    public float delay = 3;
    private bool isAttack;
    private Animator animator;
    private Transform _transform;
    private Transform targetTransform;

    private void Awake()
    {
      animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
      StartCoroutine(StartCombat());
    }

    private void Attack()
    {
      StartCoroutine(AttackProjectile());
    }

    public Transform GetTransform()
    {
      if (_transform == null)
        _transform = transform;
      return _transform;
    }

    private Transform GetRandomTeleportPoint()
    {
      var randomIndex = Random.Range(0, teleportPoints.Length);
      return teleportPoints[randomIndex];
    }

    private IEnumerator StartCombat()
    {
      yield return new WaitForSeconds(2);
      isAttack = true;
      targetTransform = GameMode.Current.GetPlayer().GetTransform();
      yield return Teleport();
    }

    private IEnumerator Teleport()
    {
      animator.Play("TeleportBegin");
      yield return new WaitForSeconds(1);

      var point = GetRandomTeleportPoint();
      var position = point.position;
      position.z = 0;
      GetTransform().position = position;

      animator.Play("TeleportEnd");
      yield return new WaitForSeconds(1);

      if (isAttack)
        Attack();
      else
        yield return Rest();
    }

    private IEnumerator Rest()
    {
      animator.Play("Idle");
      yield return new WaitForSeconds(9);
      isAttack = true;
      yield return Teleport();
    }

    private IEnumerator AttackProjectile()
    {
      animator.Play("Conjuring");

      var total = Random.Range(4, 6);
      for (int i = 0; i < total; i++)
      {
        var projectile = Instantiate(projectilePrefab, projectilePoint.position, Quaternion.identity);
        projectile.SetDirection(Vector3.forward);

        var direction = (targetTransform.position - _transform.position).normalized;
        var angle = Vector3.Angle(Vector3.up, direction);
        projectile.transform.LookAt(targetTransform.position + offset);
        yield return new WaitForSeconds(.8f);
      }

      isAttack = false;
      yield return Teleport();
    }
  }
}
