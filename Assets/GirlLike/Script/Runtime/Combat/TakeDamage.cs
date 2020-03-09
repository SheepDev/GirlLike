using System;
using Orb.GirlLike.Players;
using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Combats
{
  public class TakeDamage : MonoBehaviour
  {
    [Header("Event")]
    public FloatEvent onDamage;
    public PointDamageEvent onPointDamage;

    public void Damage(float damage)
    {
      onDamage.Invoke(damage);
    }

    public void PointDamage(float damage, Vector3 position)
    {
      onPointDamage.Invoke(new PointDamageData(position, damage));
    }
  }

  [Serializable]
  public class PointDamageEvent : UnityEvent<PointDamageData> { }

  public struct PointDamageData
  {
    public Vector3 point;
    public float damage;

    public PointDamageData(Vector3 point, float damage)
    {
      this.point = point;
      this.damage = damage;
    }
  }
}
