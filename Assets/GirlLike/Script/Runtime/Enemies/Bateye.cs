using System.Collections;
using System.Collections.Generic;
using Orb.GirlLike.Combats;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Bateye : Enemy
  {
#pragma warning disable CS0649
    [Header("Spear Setup")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Projectile prefab;
#pragma warning restore CS0649

    protected override IEnumerator Attack()
    {
      OnIdle();
      SetDirection(TargetIsLeft());
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
      var isLeft = TargetIsLeft();
      projectile.SetDirection(isLeft);
    }
  }
}
