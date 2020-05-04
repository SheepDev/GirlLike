using System.Collections.Generic;
using Orb.GirlLike.Ememies;
using Orb.GirlLike.Utility;
using UnityEngine;

namespace Orb.GirlLike.Players
{
  public class PlayerEnemyStunItem : PlayerActiveItem
  {
    public float stunTime;
    public OverlapBehaviour overlay;

    public override bool Use(Player player)
    {
      var transform = player.GetTransform();
      overlay.transform.position = transform.position;
      gameObject.SetActive(true);
      var colliders = overlay.GetOverlapColliders();
      Stun(colliders);
      return true;
    }

    private void Stun(List<Collider2D> colliders)
    {
      var enemies = new List<Enemy>();
      foreach (var collider in colliders)
      {
        var enemy = collider.GetComponentInParent<Enemy>();
        if (enemy != null && !enemies.Contains(enemy))
        {
          enemies.Add(enemy);
        }
      }

      foreach (var enemy in enemies)
      {
        enemy.Stun(stunTime);
      }
    }
  }
}
