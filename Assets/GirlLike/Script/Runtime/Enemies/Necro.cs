using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orb.GirlLike.Ememies
{
  public class Necro : Enemy
  {
    public float maxEnemies;
#pragma warning disable CS0649
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private Enemy[] prefabs;
#pragma warning restore CS0649
    [SerializeField] private List<Enemy> enemies;

    protected override void Awake()
    {
      enemies = new List<Enemy>();
      base.Awake();
    }

    protected override void Update()
    {
      if (enemies.Count < maxEnemies)
        canAttack = true;
      else
        foreach (var enemy in enemies)
          if (enemy.HitPoint.IsDie)
          {
            canAttack = true;
            break;
          }

      base.Update();
    }

    protected override IEnumerator Attack()
    {
      OnIdle();
      canMove = false;
      canAttack = false;
      RemoveDeadEnemies();
      var enemiesCount = enemies.Count;

      if (enemiesCount >= maxEnemies)
        yield break;

      yield return base.Attack();
      yield return new WaitForSeconds(1f);
      canMove = true;
      isAttack = false;
    }

    private void SpawnEnemies_AnimTrigger()
    {
      var max = maxEnemies - enemies.Count;
      var spawnIndex = 0;
      var isLeft = TargetIsLeft();

      for (int i = 0; i < max; i++)
      {
        var prefab = GetRandom<Enemy>(prefabs);
        var spawnPoint = spawnPoints[spawnIndex].position;
        var enemy = Instantiate(prefab, spawnPoint, Quaternion.identity);
        enemy.SetDirection(isLeft);
        enemies.Add(enemy);
      }
    }

    private T GetRandom<T>(T[] items)
    {
      var randomIndex = Random.Range(0, items.Length);
      return items[randomIndex];
    }

    private void RemoveDeadEnemies()
    {
      enemies.RemoveAll((enemy) =>
      {
        var isNull = enemy == null;
        var isDelete = !isNull && enemy.HitPoint.IsDie;
        if (isDelete)
          Destroy(enemy.gameObject, 2f);
        return isNull || isDelete;
      });
    }
  }
}
