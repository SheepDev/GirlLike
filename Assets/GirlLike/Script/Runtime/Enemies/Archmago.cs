using System.Collections;
using Orb.GirlLike.Combats;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Archmago : Enemy
  {
#pragma warning disable CS0649
    [Header("Spear Setup")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Projectile prefab;
#pragma warning restore CS0649
    private Vector3 attackDirection;

    protected override IEnumerator Attack()
    {
      OnIdle();
      SetDirection(TargetIsLeft());
      attackDirection = GetTargetDirection(spawnPoint.position);
      canMove = false;
      yield return base.Attack();
    }

    private void FinishAttack_AnimTrigger()
    {
      StartCoroutine(FinishAttack());
    }

    protected IEnumerator FinishAttack()
    {
      Animator.Play("Idle");
      canMove = true;
      isAttack = false;
      yield return AttackCountdown(attackCountdown);
    }

    private void SpawnFireBall_AnimTrigger()
    {
      var projectile = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
      projectile.gameObject.SetActive(true);
      projectile.SetDirection(attackDirection);
    }
  }
}
