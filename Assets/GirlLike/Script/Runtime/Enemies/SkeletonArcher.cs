using System.Collections;
using Orb.GirlLike.Combats;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class SkeletonArcher : Enemy
  {
#pragma warning disable CS0649
    [Header("Spear Setup")]
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private Projectile prefab;
#pragma warning restore CS0649

    protected override IEnumerator Attack()
    {
      OnIdle();
      var isLeft = TargetIsLeft();
      SetDirection(isLeft);
      canMove = false;
      yield return base.Attack();
      yield return AttackCountdown(1f);
      canAttack = false;
      canMove = true;
      yield return AttackCountdown(attackCountdown);
    }

    private void SpawnArrow_AnimTrigger()
    {
      var arrow = Instantiate(prefab, arrowSpawnPoint.position, Quaternion.identity);
      var isLeft = TargetIsLeft();
      arrow.gameObject.SetActive(true);
      arrow.SetDirection(isLeft ? Vector3.left : Vector3.right);
    }
  }
}
