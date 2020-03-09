using UnityEngine;
using UnityEngine.Events;

namespace Orb.GirlLike.Combats
{
  public class HitPoint : MonoBehaviour
  {
    [SerializeField] protected float hitPoint;

    public bool IsDie => hitPoint <= 0;

    [Header("Event")]
    public UnityEvent onDie;

    public virtual void ApplyDamage(float damage)
    {
      hitPoint -= damage;

      if (IsDie)
        Die();
    }

    public virtual void ApplyDamage(PointDamageData data)
    {
      hitPoint -= data.damage;

      if (IsDie)
        Die();
    }

    public virtual void Die()
    {
      onDie.Invoke();
    }
  }
}
