using System.Collections;
using Orb.GirlLike.AI;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  [RequireComponent(typeof(MoveTo))]
  public class Lancer : Enemy
  {
#pragma warning disable CS0649
    [Header("Spear Setup")]
    [SerializeField] private Transform spearSpawnPoint;
    [SerializeField] private Spear prefab;
#pragma warning restore CS0649

    protected override IEnumerator Attack()
    {
      var isLeft = TargetIsLeft();
      SetDirection(isLeft);
      canMove = false;
      yield return base.Attack();
      canAttack = false;
      yield return AttackCountdown(attackCountdown);
      canMove = true;
    }

    private void SpawnSpear()
    {
      var spear = Instantiate(prefab, spearSpawnPoint.position, Quaternion.identity);
      var isLeft = TargetIsLeft();
      spear.SetDirection(isLeft);
      spear.gameObject.SetActive(true);
    }
  }
}
